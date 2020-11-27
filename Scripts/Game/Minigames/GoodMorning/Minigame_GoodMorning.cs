using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame_GoodMorning : MonoBehaviour
{
    public Level level;
    [Space]
    public Animator animBedNormal;
    public Animator animBedHospital;
    public AnimatorController mary;
    public AnimatorController p3;
    public AnimatorController tessa;
    public AnimatorController amara;
    public GameObject tessaMove;
    public GameObject amaraMove;
    public GameObject bryce;
    public GameObject flop;
    public GameObject bryceWin;
    public GameObject flopWin;
    public SpriteController glowAmara;
    public Camera cameraFocus;
    [Space]
    public AudioController audioStep;
    public AudioController audioAmb;
    [Space]
    public Transform startTessa;
    public Transform endTessa;
    public Transform endAmara;
    public Transform endTessaAmara;
    [Space]
    public Minigame_GoodMorning_Object[] objects;

    private bool setup;
    private bool end;

    private Lerp<Vector3> lerpPosTessa;
    private Lerp<Vector3> lerpPosAmara;

    private readonly float endingBreakpoint = 0.6f;
    private enum Ending
    {
        NORMAL, //Mary & P3 > 0, but < breakpoint, few losses
        LOSS, //Mary & P3 > breakpoint
        MARY, //Mary > breakpoint
        P3, //Mary < breakpoint & P3 > breakpoint
        TESSA, //Mary & P3 & losses == 0
        AMARA, //Amara == max
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Lerp.Apply(lerpPosTessa, SetPosTessa))
        {
            lerpPosTessa = null;
        }

        if (Lerp.Apply(lerpPosAmara, SetPosAmara))
        {
            lerpPosAmara = null;
        }

        if (!setup) return;
        InputUpdate();
        if(Level.DEBUG) DebugUpdate();
    }

    private void OnDrawGizmos()
    {
        if (startTessa == null || endTessa == null) return;
        Gizmos.color = Color.red.SetAlpha(0.5f);
        Gizmos.DrawSphere(startTessa.position, 0.5f);
        Gizmos.DrawSphere(endTessa.position, 0.5f);
        Gizmos.DrawLine(startTessa.position, endTessa.position);

        if (amaraMove == null || endAmara == null || endTessaAmara == null) return;
        Gizmos.color = new Color(0.5f, 0f, 1f, 0.5f);
        Gizmos.DrawSphere(endTessaAmara.position, 0.5f);
        Gizmos.DrawSphere(endAmara.position, 0.5f);
        Gizmos.DrawLine(startTessa.position, endTessaAmara.position);
        Gizmos.DrawLine(amaraMove.transform.position, endAmara.position);

    }

    /// <summary>
    /// Sets up and starts the minigame
    /// </summary>
    public void StartGame()
    {
        setup = false;

        //Audio
        audioAmb.Play();

        //Setup objects
        float dayPerc = level.manager.GetDayPerc();
        int objectsToShow = (int)(((float)objects.Length) * dayPerc)-1;
        for(int i = 0; i < objects.Length; i++)
        {
            var o = objects[i];
            if (i < objectsToShow) o.SetHospital();
            else o.SetNormal();
        }

        //Setup characters
        mary.gameObject.SetActive(false);
        p3.gameObject.SetActive(false);
        tessa.gameObject.SetActive(false);
        bryce.gameObject.SetActive(false);
        flop.gameObject.SetActive(false);
        bryceWin.gameObject.SetActive(false);
        flopWin.gameObject.SetActive(false);
        animBedNormal.Play("idle");
        animBedHospital.Play("idle");

        //Setup UI
        level.manager.menuGame.Show();
        level.manager.menuGame.SetText("Click to wake up");
        setup = true;
    }

    /// <summary>
    /// Update function for player input
    /// </summary>
    void InputUpdate()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            if (level.manager.IsFinalDay() || end)
            {
                level.manager.menuEnding.text.SetAlpha(0f);
                level.manager.menuEnding.bg.SetAlpha(0f);
                level.manager.menuEnding.Show();

                var ending = GetEnding();
                if (ending == Ending.AMARA) StartCoroutine(EndingAmaraTransition());
                else if (ending == Ending.MARY || ending == Ending.P3 || ending == Ending.TESSA) StartCoroutine(EndingAwakeTransition(ending));
                else StartCoroutine(EndingAsleepTransition(ending));
            }
            else
            {
                level.manager.menuGame.SetText("Good morning, Tessa!");
                level.manager.fx.Play("points_normal");
                audioAmb.Stop(1f);
                level.StartEndTimer(2f);
            }

            setup = false;
        }
    }

    /// <summary>
    /// Update function for debug input
    /// </summary>
    void DebugUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            level.manager.counterMary.Set(0);
            level.manager.counterP3.Set(0);
            level.manager.counterLoss.Set(0);
            end = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            level.manager.counterMary.Set(10);
            level.manager.counterP3.Set(0);
            level.manager.counterLoss.Set(0);
            end = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            level.manager.counterMary.Set(0);
            level.manager.counterP3.Set(10);
            level.manager.counterLoss.Set(0);
            end = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            level.manager.counterMary.Set(0);
            level.manager.counterP3.Set(0);
            level.manager.counterLoss.Set(10);
            end = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            level.manager.counterMary.Set(1);
            level.manager.counterP3.Set(1);
            level.manager.counterLoss.Set(1);
            end = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            level.manager.counterAmara.Set(5);
            end = true;
        }
    }

    /// <summary>
    /// Main coroutine for the awake ending animation
    /// </summary>
    /// <param name="ending">The ending type</param>
    /// <returns></returns>
    IEnumerator EndingAwakeTransition(Ending ending)
    {
        yield return new WaitForSeconds(3f);

        animBedNormal.Play("empty");
        animBedHospital.Play("empty");
        tessa.SetAnimation("tessa_idle_down");
        tessa.gameObject.SetActive(true);
        audioStep.Play();

        yield return new WaitForSeconds(2f);

        //Zoom
        level.manager.SetMainCameraEnabled(false);
        cameraFocus.gameObject.SetActive(true);

        //Change Tessa
        if (ending == Ending.MARY) tessa.SetAnimation("mary_idle_down");
        else if (ending == Ending.P3) tessa.SetAnimation("p3_idle_down");
        else tessa.SetAnimation("tessa_idle_down");

        //Music
        if (ending == Ending.MARY) level.manager.music.Play("ending_mary");
        else if (ending == Ending.P3) level.manager.music.Play("ending_p3");
        else level.manager.music.Play("ending_tessa");

        yield return new WaitForSeconds(2f);

        lerpPosTessa = Lerp.Get(6f, startTessa.position, endTessa.position);

        //Walk left
        if (ending == Ending.MARY) tessa.SetAnimation("mary_walk_left");
        else if (ending == Ending.P3) tessa.SetAnimation("p3_walk_left");
        else tessa.SetAnimation("tessa_walk_left");

        yield return new WaitForSeconds(1f);

        cameraFocus.gameObject.SetActive(false);
        level.manager.SetMainCameraEnabled(true);

        //Enable friends
        if(ending == Ending.TESSA)
        {
            bryceWin.SetActive(true);
            flopWin.SetActive(true);
        }

        yield return new WaitForSeconds(2f);

        level.manager.menuEnding.bg.FadeAlpha(2f, 1f);
        audioAmb.Stop(2f);

        yield return new WaitForSeconds(2f);

        level.manager.menuEnding.text.SetText(GetEndingText(ending));
        level.manager.menuEnding.text.FadeAlpha(2f, 1f);
        level.manager.menuEnding.bg.FadeAlpha(2f, 1f);

        yield return new WaitForSeconds(4f);

        level.manager.menuEnding.text.FadeAlpha(2f, 0f);

        yield return new WaitForSeconds(3f);

        level.manager.ReturnToMainMenu();
    }

    /// <summary>
    /// Main coroutine for the asleep ending animation
    /// </summary>
    /// <param name="ending">The ending type</param>
    /// <returns></returns>
    IEnumerator EndingAsleepTransition(Ending ending)
    {
        yield return new WaitForSeconds(3f);

        level.manager.SetMainCameraEnabled(false);
        cameraFocus.gameObject.SetActive(true);

        if (ending == Ending.LOSS)
        {
            mary.gameObject.SetActive(true);
            p3.gameObject.SetActive(true);
        }
        else if(ending == Ending.NORMAL)
        {
            bryce.SetActive(true);
            flop.SetActive(true);
        }

        //Music
        if (ending == Ending.LOSS) level.manager.music.Play("ending_loss");
        else if (ending == Ending.NORMAL) level.manager.music.Play("ending_normal");

        yield return new WaitForSeconds(3f);

        cameraFocus.gameObject.SetActive(false);
        level.manager.SetMainCameraEnabled(true);

        yield return new WaitForSeconds(4f);

        level.manager.menuEnding.bg.FadeAlpha(2f, 1f);
        audioAmb.Stop(2f);

        yield return new WaitForSeconds(2f);

        level.manager.menuEnding.text.SetText(GetEndingText(ending));
        level.manager.menuEnding.text.FadeAlpha(2f, 1f);
        level.manager.menuEnding.bg.FadeAlpha(2f, 1f);

        yield return new WaitForSeconds(4f);

        level.manager.menuEnding.text.FadeAlpha(2f, 0f);

        yield return new WaitForSeconds(3f);

        level.manager.ReturnToMainMenu();
    }

    /// <summary>
    /// Main coroutine for the amara ending animation
    /// </summary>
    /// <returns></returns>
    IEnumerator EndingAmaraTransition()
    {
        yield return new WaitForSeconds(3f);

        glowAmara.FadeAlpha(1f, 1f);
        level.manager.fx.Play("flower_glow");

        yield return new WaitForSeconds(2f);

        lerpPosAmara = Lerp.Get(4f, amaraMove.transform.position, endAmara.position);
        amara.SetAnimation("idle_down");
        level.manager.music.Play("ending_amara");

        yield return new WaitForSeconds(5f);

        amara.SetAnimation("idle_right");
        glowAmara.FadeAlpha(1f, 0f);

        yield return new WaitForSeconds(3f);

        animBedNormal.Play("empty");
        animBedHospital.Play("empty");
        tessa.SetAnimation("tessa_idle_down");
        tessa.gameObject.SetActive(true);
        audioStep.Play();

        yield return new WaitForSeconds(2f);

        tessa.SetAnimation("tessa_idle_left");

        yield return new WaitForSeconds(3f);

        tessa.SetAnimation("tessa_walk_left");
        lerpPosTessa = Lerp.Get(4f, startTessa.position, endTessaAmara.position);

        yield return new WaitForSeconds(4f);

        tessa.SetAnimation("tessa_idle_left");

        yield return new WaitForSeconds(3f);

        tessaMove.SetActive(false);
        amara.SetAnimation("hug");

        yield return new WaitForSeconds(4f);

        level.manager.menuEnding.bg.FadeAlpha(2f, 1f);
        audioAmb.Stop(2f);

        yield return new WaitForSeconds(2f);

        level.manager.menuEnding.text.SetText(GetEndingText(Ending.AMARA));
        level.manager.menuEnding.text.FadeAlpha(2f, 1f);
        level.manager.menuEnding.bg.FadeAlpha(2f, 1f);

        yield return new WaitForSeconds(4f);

        level.manager.menuEnding.text.FadeAlpha(2f, 0f);

        yield return new WaitForSeconds(3f);

        level.manager.ReturnToMainMenu();
    }

    /// <summary>
    /// Returns the current ending, based on ending counters
    /// </summary>
    /// <returns>The ending type</returns>
    Ending GetEnding()
    {
        float mary = level.manager.counterMary.GetPerc();
        float p3 = level.manager.counterP3.GetPerc();
        float loss = level.manager.counterLoss.GetPerc();
        int amara = level.manager.counterAmara.GetValue();

        if (amara == LevelManager.DAY_MAX) return Ending.AMARA;
        else if (mary > endingBreakpoint && p3 > endingBreakpoint) return Ending.LOSS;
        else if (mary > endingBreakpoint) return Ending.MARY;
        else if (p3 > endingBreakpoint) return Ending.P3;
        else if (mary > 0f || p3 > 0f || loss > 0f) return Ending.NORMAL;
        else return Ending.TESSA;
    }

    /// <summary>
    /// Returns the ending text based on the ending type
    /// </summary>
    /// <param name="ending">The ending type</param>
    /// <returns>The ending text</returns>
    string GetEndingText(Ending ending)
    {
        switch (ending)
        {
            case Ending.LOSS: return "Ending 6: Inner demons";
            case Ending.MARY: return "Ending 5: A wolf in sheep's clothing";
            case Ending.P3: return "Ending 4: We all have a clock";
            case Ending.NORMAL: return "Ending 3: Still dreaming";
            case Ending.TESSA: return "Ending 2: Back to reality";
            case Ending.AMARA: return "Ending 1: Closure";
            default: return "";
        }
    }

    /// <summary>
    /// Sets the position of the moving tessa object
    /// </summary>
    /// <param name="v">The position</param>
    void SetPosTessa(Vector3 v)
    {
        tessaMove.transform.position = v;
    }

    /// <summary>
    /// Sets the position of the moving amara object
    /// </summary>
    /// <param name="v">The position</param>
    void SetPosAmara(Vector3 v)
    {
        amaraMove.transform.position = v;
    }
}
