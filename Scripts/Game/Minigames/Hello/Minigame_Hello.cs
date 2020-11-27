using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame_Hello : MonoBehaviour
{
    public Level level;

    [Space]
    public Minigame_Hello_Scene sceneNormal;
    public Minigame_Hello_Scene sceneMary;
    public Minigame_Hello_Scene sceneP3;

    [Space]
    public AnimatorController tessa;
    public Minigame_Hello_Car carMatt;
    public Minigame_Hello_Car carVivi;
    public Minigame_Hello_Clock clock;
    public Minigame_Hello_Car[] carsOther;

    [Space]
    public SpriteRenderer renButton;
    public Sprite spBtnHello;
    public Sprite spBtnCount;

    [Space]
    public Transform tCarStart;
    public Transform tCarEnd;

    private readonly float distCarBase = 100f;
    private readonly float distCarMultMin = 1f;
    private readonly float distCarMultMax = 4f;

    private float timeCar;
    private readonly float timeCarMin = 8f;
    private readonly float timeCarMax = 6f;

    private int amountCars;
    private readonly int amountCarsMin = 2;
    private readonly int amountCarsMax = 6;

    private int amountClockHits;
    private readonly int amountClockHitsMin = 3;
    private readonly int amountClockHitsMax = 8;

    private bool setup;
    private bool carActive;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        if(tCarStart != null && tCarEnd != null)
        {
            Gizmos.color = Color.green.SetAlpha(0.5f);
            Gizmos.DrawSphere(tCarStart.position, 0.5f);
            Gizmos.DrawSphere(tCarEnd.position, 0.5f);
            Gizmos.DrawLine(tCarStart.position, tCarEnd.position);
        }
    }

    /// <summary>
    /// Sets up and starts the minigame
    /// </summary>
    public void StartGame()
    {
        var type = level.GetLevelType();

        //Setup scene
        sceneNormal.gameObject.SetActive(false);
        sceneMary.gameObject.SetActive(false);
        sceneP3.gameObject.SetActive(false);

        var scene = sceneNormal;
        if (type == Level.Type.MARY) scene = sceneMary;
        else if (type == Level.Type.P3) scene = sceneP3;
        scene.gameObject.SetActive(true);
        level.SetDecorationsEnabled(scene.decorations, true, 0.5f);

        //Button
        renButton.sprite = type == Level.Type.P3 ? spBtnCount : spBtnHello;

        //Flower
        scene.flower.gameObject.SetActive(level.IsAmaraActive());

        //Setup difficulty
        float diff = level.manager.GetDifficulty();
        float distCarMult = Mathf.Lerp(distCarMultMin, distCarMultMin, diff);
        timeCar = Mathf.Lerp(timeCarMin, timeCarMax, diff);
        amountCars = (int)(Mathf.Lerp((float)amountCarsMin, (float)amountCarsMax, diff));
        amountClockHits = (int)(Mathf.Lerp((float)amountClockHitsMin, (float)amountClockHitsMax, diff));
        
        tCarStart.position = tCarStart.position.SetX(distCarBase * distCarMult);
        tCarEnd.position = tCarEnd.position.SetX(-distCarBase * distCarMult);

        //Music
        if (type == Level.Type.MARY) level.manager.PlayMinorMusic();
        else if (type == Level.Type.P3) level.manager.PlayTickTockMusic();
        else level.manager.PlayMajorMusic();

        //UI
        level.manager.menuGame.Show();

        //Start
        setup = true;

        if (type == Level.Type.MARY) StartCoroutine(TransitionMary());
        else if (type == Level.Type.P3) StartCoroutine(TransitionClock(amountClockHits));
        else StartCoroutine(TransitionNormal());
    }

    /// <summary>
    /// The main coroutine for the minigame
    /// </summary>
    /// <returns></returns>
    IEnumerator TransitionNormal()
    {
        carActive = true;
        yield return new WaitForSeconds(Random.Range(1f, 2f));

        //Setup cars
        int amount = Mathf.Min(carsOther.Length, amountCars);
        int idxMatt = Random.Range((int)(amount * 0.75f), amount);
        List<Minigame_Hello_Car> list = new List<Minigame_Hello_Car>();
        for (int i = 0; i < amount; i++)
        {
            if (i == idxMatt) list.Add(carMatt);
            list.Add(carsOther[i]);
        }
        
        for (int i = 0; i < list.Count; i++)
        {
            var car = list[i];
            car.StartDriving(timeCar, 0f);
            yield return new WaitForSeconds(Random.Range(2f, 4f));
        }
    }

    /// <summary>
    /// The coroutine for the mary type of the minigame
    /// </summary>
    /// <returns></returns>
    IEnumerator TransitionMary()
    {
        carActive = true;
        yield return new WaitForSeconds(Random.Range(2f, 5f));

        carVivi.StartDriving(timeCar * 0.75f, 0f);
    }

    /// <summary>
    /// The coroutine for the ticktock type of the minigame
    /// </summary>
    /// <param name="hits"></param>
    /// <returns></returns>
    IEnumerator TransitionClock(int hits)
    {
        carActive = true;
        yield return new WaitForSeconds(Random.Range(2f, 3f));

        float timeMin = 1f;
        float timeMax = 3.0f;
        float timeDiff = 0.4f;
        float time = timeMax;

        for(int i = 0; i < hits; i++)
        {
            if (setup)
            {
                tessa.SetAnimation("tessa_idle_down");

                carActive = true;
                clock.Rotate(time);
                time = Mathf.Max(timeMin, time - timeDiff);

                yield return new WaitForSeconds(time * 1.3f);
            }
            else break;
        }
    }

    /// <summary>
    /// Sets Tessa waving and checks if the target position was correctly timed
    /// </summary>
    public void SayHello()
    {
        if (!setup) return;
        tessa.SetAnimation("tessa_wave");
        CheckPosition();
    }

    /// <summary>
    /// Checks if the target position was correctly timed, and ends the game
    /// </summary>
    void CheckPosition()
    {
        var matt = carMatt.GetPerc();
        var mary = carVivi.GetPerc();
        var p3 = clock.GetPerc();
        float rangeMin = 0.47f;
        float rangeMax = 0.53f;

        if(matt >= rangeMin && matt <= rangeMax)
        {
            level.manager.fx.Play("points_normal");
            level.manager.menuGame.SetText("Gotta go!");
            carActive = false;
            End();
        }
        else if (mary >= rangeMin && mary <= rangeMax)
        {
            level.manager.fx.Play("points_mary");
            level.manager.menuGame.SetText("Zoom!");
            carActive = false;
            End();
        }
        else if(p3 >= 0.3f && p3 <= 0.7)
        {
            level.manager.fx.Play("points_p3");
            carActive = false;

            amountClockHits--;
            if(amountClockHits <= 0)
            {
                level.manager.menuGame.SetText("Tick tock!");
                End();
            }
        }
        else
        {
            Lose();
        }
    }

    /// <summary>
    /// Ends the ticktock minigame with a loss
    /// </summary>
    public void ClockEnd()
    {
        if (!setup) return;
        if (carActive) Lose();
    }

    /// <summary>
    /// Ends the game with a loss and adds a fail counter
    /// </summary>
    public void Lose()
    {
        if (!carActive) return;
        level.manager.fx.Play("wrong");
        level.AddCounter();
        level.manager.menuGame.SetText("Oh no!");
        End();
    }

    /// <summary>
    /// Ends the game
    /// </summary>
    void End()
    {
        level.StartEndTimer(2f);
        setup = false;
    }
}
