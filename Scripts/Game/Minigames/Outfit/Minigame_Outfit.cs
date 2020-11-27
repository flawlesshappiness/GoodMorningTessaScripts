using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame_Outfit : MonoBehaviour
{
    public Level level;

    [Space]
    public Minigame_Outfit_Scene sceneNormal;
    public Minigame_Outfit_Scene sceneMary;
    public Minigame_Outfit_Scene sceneP3;

    [Space]
    public Transform tBoxStart;
    public Transform tBoxEnd;

    private Minigame_Outfit_Scene scene;

    private readonly int boxMin = 2;
    private readonly int boxMax = 5;

    private int switches;
    private readonly int switchesMin = 4;
    private readonly int switchesMax = 6;

    private float timeSwitch;
    private readonly float timeSwitchMin = 1f;
    private readonly float timeSwitchMax = 1f;

    private float timeWait;
    private readonly float timeWaitMin = 0.5f;
    private readonly float timeWaitMax = 0.5f;

    private Lerp<float> lerpSwitch;
    private Minigame_Outfit_Box boxSwitch1;
    private Minigame_Outfit_Box boxSwitch2;
    private Vector3 posSwitch1;
    private Vector3 posSwitch2;

    private bool canPick = false;
    private int toPick = 0;

    private List<Minigame_Outfit_Box> boxes = new List<Minigame_Outfit_Box>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Lerp.Apply(lerpSwitch, SetSwitchPos))
        {
            lerpSwitch = null;
        }
    }

    private void OnDrawGizmos()
    {
        if(tBoxStart != null && tBoxEnd != null)
        {
            Gizmos.color = Color.green.SetAlpha(0.5f);
            Gizmos.DrawSphere(tBoxStart.position, 0.5f);
            Gizmos.DrawSphere(tBoxEnd.position, 0.5f);
            Gizmos.DrawLine(tBoxStart.position, tBoxEnd.position);
        }
    }

    /// <summary>
    /// Sets up and starts the minigame
    /// </summary>
    public void StartGame()
    {
        var type = level.GetLevelType();

        //Scene
        sceneNormal.gameObject.SetActive(false);
        sceneMary.gameObject.SetActive(false);
        sceneP3.gameObject.SetActive(false);

        if (type == Level.Type.MARY) scene = sceneMary;
        else if (type == Level.Type.P3) scene = sceneP3;
        else scene = sceneNormal;
        scene.gameObject.SetActive(true);

        //Flower
        scene.flower.gameObject.SetActive(level.IsAmaraActive());

        //Setup difficulty
        float diff = level.manager.GetDifficulty();
        int amountCorrect = Mathf.Min(scene.boxes.Length, Mathf.Max(1, (int)(Mathf.Lerp((float)boxMin, (float)boxMax, diff))));

        toPick = 1;
        if (type == Level.Type.P3) toPick = amountCorrect;

        switches = (int)(Mathf.Lerp((float)switchesMin, (float)switchesMax, diff));
        if (type == Level.Type.P3) switches += 3;
        else if (type == Level.Type.MARY) switches += 2;

        timeSwitch = Mathf.Lerp(timeSwitchMin, timeSwitchMax, diff);
        if (type == Level.Type.P3) timeSwitch *= 0.9f;
        else if (type == Level.Type.MARY) timeSwitch *= 0.8f;

        //Select boxes
        boxes = new List<Minigame_Outfit_Box>();
        var boxesUnchosen = new List<Minigame_Outfit_Box>(scene.boxes);

        boxes.AddRange(scene.boxesWrong);
        for(int i = 0; i < amountCorrect; i++)
        {
            var box = boxesUnchosen[Random.Range(0, boxesUnchosen.Count)];
            boxes.Add(box);
            boxesUnchosen.Remove(box);
        }

        boxes.Shuffle();

        //Position boxes
        for(int i = 0; i < boxes.Count; i++)
        {
            var box = boxes[i];
            float t = ((float)i) / ((float)boxes.Count-1);
            box.transform.position = Vector3.Lerp(tBoxStart.position, tBoxEnd.position, t);
            box.SetClosed(true);
        }

        //Music
        if (type == Level.Type.MARY) level.manager.PlayMinorMusic();
        else if (type == Level.Type.P3) level.manager.PlayTickTockMusic();
        else level.manager.PlayMajorMusic();

        //UI
        level.manager.menuGame.Show();

        //Start
        StartCoroutine(TransitionSwitch());
    }

    /// <summary>
    /// The main coroutine for the boxes switching place
    /// </summary>
    /// <returns></returns>
    IEnumerator TransitionSwitch()
    {
        yield return new WaitForSeconds(1f);

        level.manager.menuGame.SetText("Memorize...");
        foreach(Minigame_Outfit_Box box in boxes) box.Open(0.5f);

        yield return new WaitForSeconds(4f);

        level.manager.menuGame.SetText("Wait...");
        foreach (Minigame_Outfit_Box box in boxes) box.Close(0.5f);

        yield return new WaitForSeconds(1f);

        for(int i = 0; i < switches; i++)
        {
            var list = new List<Minigame_Outfit_Box>(boxes);
            boxSwitch1 = list[Random.Range(0, list.Count)];
            list.Remove(boxSwitch1);
            boxSwitch2 = list[Random.Range(0, list.Count)];

            lerpSwitch = Lerp.Get(timeSwitch, 0f, 1f);
            posSwitch1 = boxSwitch1.transform.position;
            posSwitch2 = boxSwitch2.transform.position;

            yield return new WaitForSeconds(timeSwitch + timeWait);
        }

        level.manager.menuGame.SetText("Pick!");

        canPick = true;
    }

    /// <summary>
    /// Called when picking a box. Checks if the box was correct.
    /// </summary>
    /// <param name="box">The box clicked</param>
    public void PickBox(Minigame_Outfit_Box box)
    {
        bool end = false;
        box.Open(0.5f);

        if (box.isWrong)
        {
            level.manager.menuGame.SetText("Oh no!");
            level.manager.fx.Play("wrong");
            level.AddCounter();
            canPick = false;
            end = true;
        }
        else
        {
            toPick--;
            var type = level.GetLevelType();
            if (type == Level.Type.MARY) level.manager.fx.Play("points_mary");
            else if (type == Level.Type.P3) level.manager.fx.Play("points_p3");
            else level.manager.fx.Play("points_normal");

            if (toPick > 0) //More to pick
            {
                level.manager.menuGame.SetText(string.Concat(toPick, " left"));
                return;
            }

            if(type == Level.Type.P3) level.manager.menuGame.SetText("Tick tock!");
            else level.manager.menuGame.SetText("Good choice!");
            canPick = false;
            end = true;
        }

        if (end)
        {
            level.StartEndTimer(2f);
        }
    }

    /// <summary>
    /// Returns true if can pick boxes, else false.
    /// </summary>
    /// <returns>If can pick</returns>
    public bool CanPickBox()
    {
        return canPick;
    }

    /// <summary>
    /// Sets the position of boxSwitch1 and boxSwitch2, based on a value from 0 to 1
    /// </summary>
    /// <param name="f">The value</param>
    void SetSwitchPos(float f)
    {
        float y1 = -10f * Mathf.Pow(f, 2f) + 10f*f;
        boxSwitch1.transform.position = Vector3.Lerp(posSwitch1, posSwitch2, f).AddY(y1);

        float y2 = -20f * Mathf.Pow(f, 2f) + 20f*f;
        boxSwitch2.transform.position = Vector3.Lerp(posSwitch2, posSwitch1, f).AddY(y2);
    }
}
