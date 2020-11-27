using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame_Parking : MonoBehaviour
{
    public Level level;

    [Space]
    [Header("PROPERTIES")]
    public Minigame_Parking_Scene sceneNormal;
    public Minigame_Parking_Scene sceneMary;
    public Minigame_Parking_Scene sceneP3;

    //Privates
    private bool setup;

    private float parkedSpacesMin = 0.2f;
    private float parkedSpacesMax = 1f;
    private float illegalSpacesMin = 0.2f;
    private float illegalSpacesMax = 0.9f;

    private float timeP3Min = 4f;
    private float timeP3Max = 8f;

    private int amountToClick;

    private bool firstCorrect = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Sets up and starts the minigame
    /// </summary>
    public void StartGame()
    {
        setup = false;
        firstCorrect = false;
        var type = level.GetLevelType();

        //Disable scenes
        sceneNormal.gameObject.SetActive(false);
        sceneMary.gameObject.SetActive(false);
        sceneP3.gameObject.SetActive(false);

        //Pick scene
        Minigame_Parking_Scene scene = null;
        if (type == Level.Type.MARY) scene = sceneMary;
        else if (type == Level.Type.P3) scene = sceneP3;
        else scene = sceneNormal;

        scene.gameObject.SetActive(true);
        var spaces = scene.spaces;

        //Flower
        scene.flower.gameObject.SetActive(level.IsAmaraActive());

        //Setup difficulty
        float diff = level.manager.GetDifficulty();
        int parkedSpaces = Mathf.Max(1, (int)(spaces.Length * Mathf.Lerp(parkedSpacesMin, parkedSpacesMax, diff)));
        int illegalSpaces = Mathf.Max(1, (int)(parkedSpaces * Mathf.Lerp(illegalSpacesMin, illegalSpacesMax, diff)));
        float timeP3 = Mathf.Lerp(timeP3Max, timeP3Min, diff);

        amountToClick = illegalSpaces;

        //Setup objects
        List<Minigame_Parking_Space> unused = new List<Minigame_Parking_Space>(spaces);
        List<Minigame_Parking_Space> parked = new List<Minigame_Parking_Space>();
        List<Minigame_Parking_Space> illegal = new List<Minigame_Parking_Space>();

        foreach(Minigame_Parking_Space space in spaces) //Set spaces disabled
            space.Setup(this);

        for (int i = 0; i < parkedSpaces; i++) //Select parked spaces
        {
            var space = unused[Random.Range(0, unused.Count)];
            parked.Add(space);
            unused.Remove(space);
            space.SetLegal();
        }

        for (int i = 0; i < illegalSpaces; i++) //Select illegal spaces
        {
            var space = parked[Random.Range(0, parked.Count)];
            illegal.Add(space);
            parked.Remove(space);
            space.SetIllegal();
        }

        //Decorations
        level.SetDecorationsEnabled(scene.decorations, true, 0.5f);

        //Types
        if(type == Level.Type.MARY)
        {
            level.manager.PlayMinorMusic();
        }
        else if (type == Level.Type.P3)
        {
            level.manager.PlayTickTockMusic();
        }
        else
        {
            level.manager.PlayMajorMusic();
        }

        //UI
        level.manager.menuGame.Show();
        if (type == Level.Type.P3) level.manager.menuGame.SetTimerActive(timeP3, OnTimerEnd);

        //Start
        setup = true;
    }

    /// <summary>
    /// Checks if a clicked parking space was correct
    /// </summary>
    /// <param name="space">The parking space</param>
    public void ClickSpace(Minigame_Parking_Space space)
    {
        if (!setup) return;
        if (space.IsLegal()) //Wrong
        {
            space.SetWrong();
            level.manager.fx.Play("wrong");
            level.manager.menuGame.SetText("Oh no!");
            level.StartEndTimer(2f);
            level.AddCounter();
            setup = false;
        }
        else //Correct
        {
            space.SetCorrect();

            var ps = level.manager.particles.Instantiate("poof");
            ps.transform.position = space.illegal.transform.position;

            var type = level.GetLevelType();
            string soundPoints =
                (type == Level.Type.MARY) ? "points_mary" :
                (type == Level.Type.P3) ? "points_p3" :
                "points_normal";

            if(firstCorrect) level.manager.fx.AdjustPitch(soundPoints, 0.1f);
            level.manager.fx.Play(soundPoints);
            firstCorrect = true;
            amountToClick--;

            if(amountToClick <= 0) //Finished
            {
                setup = false;
                level.manager.menuGame.SetText("Have a Dugong day!");
                level.StartEndTimer(2f);
            }
        }
    }

    /// <summary>
    /// Ends the minigame and adds a fail counter
    /// </summary>
    void OnTimerEnd()
    {
        if (!setup) return;
        setup = false;
        level.AddCounter();
        level.EndLevel();
    }
}
