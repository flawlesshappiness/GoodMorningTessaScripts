using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame_Crosswalk : MonoBehaviour
{
    public enum MovementType { UP, LEFT, RIGHT, DOWN }

    public Level level;

    [Space]
    [Header("SCENES")]
    public Minigame_Crosswalk_Scene sceneNormal;
    public Minigame_Crosswalk_Scene sceneMary;
    public Minigame_Crosswalk_Scene sceneP3;

    private Minigame_Crosswalk_Scene scene;
    private Minigame_Crosswalk_Trail trail;

    private bool setup = false;

    private readonly float timeCrossMin = 6f;
    private readonly float timeCrossMax = 12f;
    private float timeCross;

    private readonly float timeP3Min = 15f;
    private readonly float timeP3Max = 25f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!setup) return;
        if (Level.DEBUG && Input.GetKeyDown(KeyCode.Space)) Win();
    }

    /// <summary>
    /// Sets up and starts the minigame
    /// </summary>
    public void StartGame()
    {
        //Type
        var type = level.GetLevelType();

        //Pick scene
        sceneNormal.gameObject.SetActive(false);
        sceneMary.gameObject.SetActive(false);
        sceneP3.gameObject.SetActive(false);

        scene = sceneNormal;
        if (type == Level.Type.MARY) scene = sceneMary;
        else if (type == Level.Type.P3) scene = sceneP3;
        scene.gameObject.SetActive(true);

        //Flower
        scene.flower.gameObject.SetActive(level.IsAmaraActive());

        //Trail
        if(type == Level.Type.MARY)
        {
            trail = scene.trails[Random.Range(0, scene.trails.Length)];
            trail.gameObject.SetActive(true);
            trail.FadeOut();
        }

        //Difficulty
        float diff = level.manager.GetDifficulty();
        timeCross = Mathf.Lerp(timeCrossMin, timeCrossMax, diff);

        //Decorations
        level.SetDecorationsEnabled(scene.decorations, true, 0.5f);

        //Music
        if (type == Level.Type.MARY) level.manager.PlayMinorMusic();
        else if (type == Level.Type.P3) level.manager.PlayTickTockMusic();
        else level.manager.PlayMajorMusic();

        //UI
        level.manager.menuGame.Show();
        if (type == Level.Type.P3)
        {
            float time = Mathf.Lerp(timeP3Max, timeP3Min, diff);
            level.manager.menuGame.SetTimerActive(time, Lose);
        }

        //Setup finished
        setup = true;

        //Stoplight
        if(scene.stoplight != null) StartCoroutine(StoplightTransition());
    }

    /// <summary>
    /// The looping coroutine for the stoplight states
    /// </summary>
    /// <returns></returns>
    IEnumerator StoplightTransition()
    {
        float min = 3f;
        float max = 5f;
        
        scene.stoplight.Play("red_1");
        yield return new WaitForSeconds(Random.Range(min, max));
        StartRandomCar();
        scene.stoplight.Play("red_2");
        yield return new WaitForSeconds(Random.Range(min, max));
        StartRandomCar();
        scene.stoplight.Play("red_3");
        yield return new WaitForSeconds(Random.Range(min, max));
        if(Random.Range(0f,1f) > level.manager.GetDifficulty()) StartRandomCar();
        scene.stoplight.Play("red_4");
        yield return new WaitForSeconds(Random.Range(min*1.5f, max*1.5f));
        scene.stoplight.Play("white");
        yield return new WaitForSeconds(timeCross); //Time to cross
        StartCoroutine(StoplightTransition()); //Reset
    }

    /// <summary>
    /// Start moving a random car
    /// </summary>
    void StartRandomCar()
    {
        var car = scene.cars[Random.Range(0, scene.cars.Length)];
        car.StartDriving();
    }

    /// <summary>
    /// Ends the game without adding a loss counter
    /// </summary>
    public void Win()
    {
        var type = level.GetLevelType();
        if (type == Level.Type.MARY) level.manager.menuGame.SetText("Just peachy!");
        else if (type == Level.Type.P3) level.manager.menuGame.SetText("Tick tock!");
        else level.manager.menuGame.SetText("Safety first!");

        if (type == Level.Type.MARY) level.manager.fx.Play("points_mary");
        else if (type == Level.Type.P3) level.manager.fx.Play("points_p3");
        else level.manager.fx.Play("points_normal");

        level.StartEndTimer(2f);
        setup = false;
    }

    /// <summary>
    /// Ends the game and add a loss counter
    /// </summary>
    public void Lose()
    {
        setup = false;
        level.AddCounter();
        level.EndLevel();
    }

    /// <summary>
    /// When the player moves´away from the trail, ends the game with a loss
    /// </summary>
    public void LoseTrail()
    {
        level.StartEndTimer(2f);
        level.AddCounter();
        level.manager.fx.Play("wrong");
        level.manager.menuGame.SetText("Oh no!");
        trail.FadeIn();
        setup = false;
    }

    /// <summary>
    /// Returns true if the game is setup, else false
    /// </summary>
    /// <returns>if is setup</returns>
    public bool IsSetup()
    {
        return setup;
    }
}
