using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame_SpaceFlower : MonoBehaviour
{
    public Level level;
    public SpriteController overlay;

    [Space]
    public GameObject sceneMain;
    public GameObject sceneAmara;

    [Space]
    public GameObject panelButtons;
    public GameObject panelSpots;

    [Space]
    public Transform flower;
    public Transform amara;

    [Space]
    public Transform spotsStart;
    public Transform spotsEnd;
    public Transform amaraEnd;
    public Minigame_SpaceFlower_Spot[] spots;

    [Space]
    public Minigame_SpaceFlower_Glow[] glows;

    private readonly int[] spotsAmounts = { 2, 3, 4, 5, 6 };
    private List<Minigame_SpaceFlower_Spot> spotsActive = new List<Minigame_SpaceFlower_Spot>();

    private int idxFlower;
    private Lerp<Vector3> lerpFlower;
    private Lerp<Vector3> lerpAmara;

    private bool setup;
    private bool skip;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Lerp.Apply(lerpFlower, SetFlowerPosition))
        {
            lerpFlower = null;
        }

        if (Lerp.Apply(lerpAmara, SetAmaraPosition))
        {
            lerpAmara = null;
        }

        if (Level.DEBUG) DebugUpdate();
    }

    private void OnDrawGizmos()
    {
        if (spotsStart != null && spotsEnd != null)
        {
            Gizmos.color = Color.green.SetAlpha(0.5f);
            Gizmos.DrawSphere(spotsStart.position, 0.5f);
            Gizmos.DrawSphere(spotsEnd.position, 0.5f);
            Gizmos.DrawLine(spotsStart.position, spotsEnd.position);
        }
    }

    /// <summary>
    /// The update function for debug input
    /// </summary>
    void DebugUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            skip = true;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            level.manager.counterAmara.Set(LevelManager.DAY_MAX - 1);
        }
    }

    /// <summary>
    /// Sets up and starts the minigame
    /// </summary>
    public void StartGame()
    {
        int amara = level.manager.counterAmara.GetValue();

        //Scene
        sceneMain.SetActive(true);
        sceneAmara.SetActive(false);

        //Spots
        int spotsAmount = spotsAmounts.IndexOrMax(amara);
        for (int i = 0; i < spotsAmount; i++)
        {
            var spot = spots[i];
            spotsActive.Add(spots[i]);
            float t = ((float)i) / ((float)spotsAmount - 1);
            spot.transform.position = Vector3.Lerp(spotsStart.position, spotsEnd.position, t);
            spot.SetInactive();
            spot.SetIndex(i);
        }

        //Flower
        int idxMid = spotsAmount / 2;
        idxFlower = idxMid;
        SetFlowerPosition(idxFlower);

        //Overlay
        overlay.SetAlpha(0f);

        //Music
        level.manager.music.Play("ships");

        //UI
        level.manager.menuGame.Show();

        //Setup
        setup = true;
        StartCoroutine(TransitionGame());
    }

    /// <summary>
    /// Adjusts the flower position from an index
    /// </summary>
    /// <param name="idx">Position index adjustment</param>
    public void AdjustFlowerPosition(int idx)
    {
        if (!setup) return;
        idxFlower += idx;
        if (idxFlower >= spotsActive.Count) idxFlower = spotsActive.Count - 1;
        else if (idxFlower < 0) idxFlower = 0;
        else level.manager.fx.Play("blip");

        SetFlowerPosition(idxFlower);
    }

    /// <summary>
    /// Sets the flower position from an index
    /// </summary>
    /// <param name="idx">Position index</param>
    void SetFlowerPosition(int idx)
    {
        var spot = spotsActive[idx];
        flower.position = spot.bottom.position;
    }

    /// <summary>
    /// The main minigame coroutine
    /// </summary>
    /// <returns></returns>
    IEnumerator TransitionGame()
    {
        float pauseMin = 1f;
        float pauseMax = 5f;
        float pauseDiff = 0.5f;
        float timePause = pauseMax;

        float timeFall = 0.5f;

        yield return new WaitForSeconds(3f);

        for(int i = 0; i < 10; i++)
        {
            if (skip) continue;
            List<Minigame_SpaceFlower_Spot> spots = new List<Minigame_SpaceFlower_Spot>(spotsActive);
            int toRemove = spots.Count == 2 ? 1 : Random.Range(1, 3);
            for (int j = 0; j < toRemove; j++) spots.Remove(spots[Random.Range(0, spots.Count)]);
            foreach (Minigame_SpaceFlower_Spot spot in spots) spot.SetActive(true);
                yield return new WaitForSeconds(1f);
            foreach (Minigame_SpaceFlower_Spot spot in spots) spot.FallStar(timeFall);
            level.manager.fx.Play("star_fall");
                yield return new WaitForSeconds(timeFall);
            level.manager.fx.Play("break");
                yield return new WaitForSeconds(Mathf.Max(0f, timePause - timeFall));

            timePause = Mathf.Max(pauseMin, timePause - pauseDiff);
        }

        setup = false;
        var spotFlower = spotsActive[idxFlower];
        lerpFlower = Lerp.Get(1f, flower.position, spotFlower.top.position);

        yield return new WaitForSeconds(0.5f);

        overlay.FadeAlpha(1f, 1f);

        yield return new WaitForSeconds(1f);

        int amara = level.manager.counterAmara.GetValue();
        flower.gameObject.SetActive(false);
        panelButtons.SetActive(false);
        panelSpots.SetActive(false);
        sceneMain.SetActive(false);
        sceneAmara.SetActive(true);

        for (int i = 0; i < glows.Length; i++)
        {
            var glow = glows[i];
            glow.SetGlowing(i <= amara-1);
        }

        overlay.FadeAlpha(2f, 0f);

        yield return new WaitForSeconds(3f);

        level.manager.counterAmara.Add(1);
        amara = level.manager.counterAmara.GetValue();
        glows[amara-1].StartFlower(0.5f);

        yield return new WaitForSeconds(0.5f);

        level.manager.fx.Play("flower_glow");

        if(amara >= LevelManager.DAY_MAX)
        {
            yield return new WaitForSeconds(2f);

            lerpAmara = Lerp.Get(1f, this.amara.position, amaraEnd.position);
            level.manager.fx.Play("star_fall");
        }

        yield return new WaitForSeconds(5f);

        level.EndLevel();
    }

    /// <summary>
    /// Hit a position index
    /// </summary>
    /// <param name="idx">Index to hit</param>
    public void Hit(int idx)
    {
        if (idxFlower == idx)
        {
            level.EndLevel();
            level.AddCounter();
        }
    }

    /// <summary>
    /// Sets the position of the flower
    /// </summary>
    /// <param name="v">Position</param>
    void SetFlowerPosition(Vector3 v)
    {
        flower.position = v;
    }

    /// <summary>
    /// Sets the position of the Amara
    /// </summary>
    /// <param name="v">Position</param>
    void SetAmaraPosition(Vector3 v)
    {
        amara.position = v;
    }
}
