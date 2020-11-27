using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame_WalkingPuzzle : MonoBehaviour
{
    public Level level;

    public Transform tTessa;
    public AnimatorController tessa;
    public Transform tEnd;
    public Transform tObjectEnd;
    public Transform tObjectStart;

    [Space]
    public Minigame_WalkingPuzzle_Scene sceneNormal;
    public Minigame_WalkingPuzzle_Scene sceneMary;
    public Minigame_WalkingPuzzle_Scene sceneP3;
    private Minigame_WalkingPuzzle_Scene scene;

    private bool setup = false;
    private int idxObject;

    private Lerp<Vector3> lerpPos;

    private List<int[]> listPuzzles = new List<int[]>
    {
        new int[] { 1, 2, 3, 2, 4, 5, 4, 6 },
        new int[] { 2, 1, 3, 2, 3, 3, 4, 5 },
    };

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Lerp.Apply(lerpPos, SetTessaPosition))
        {
            lerpPos = null;
        }
    }

    private void OnDrawGizmos()
    {
        if(tObjectStart != null)
        {
            Gizmos.color = Color.red.SetAlpha(0.5f);
            Gizmos.DrawSphere(tObjectStart.position, 0.5f);
        }

        if (tObjectEnd != null)
        {
            Gizmos.color = Color.yellow.SetAlpha(0.5f);
            Gizmos.DrawSphere(tObjectEnd.position, 0.5f);
        }

        if (tEnd != null)
        {
            Gizmos.color = Color.green.SetAlpha(0.5f);
            Gizmos.DrawSphere(tEnd.position, 0.5f);
        }
    }

    /// <summary>
    /// Sets up, and starts the minigame
    /// </summary>
    public void StartGame()
    {
        //Setup difficulty
        float diff = level.manager.GetDifficulty();

        //Setup scene
        sceneNormal.gameObject.SetActive(false);
        sceneMary.gameObject.SetActive(false);
        sceneP3.gameObject.SetActive(false);

        var type = level.GetLevelType();
        if(type == Level.Type.NORMAL)
        {
            level.manager.PlayMajorMusic();
            scene = sceneNormal;
        }
        else if(type == Level.Type.MARY)
        {
            level.manager.PlayMinorMusic();
            scene = sceneMary;
            diff = Mathf.Min(1f, diff + 0.5f);
        }
        else if(type == Level.Type.P3)
        {
            level.manager.PlayTickTockMusic();
            scene = sceneP3;
        }

        scene.gameObject.SetActive(true);

        //Flower
        scene.flower.gameObject.SetActive(level.IsAmaraActive());

        //Setup Tessa
        tessa.SetAnimation("tessa_walk_right");
        SetPositionIndex(-1);
        idxObject = -1;

        //Setup Objects
        int[] puzzle = listPuzzles[Random.Range(0, listPuzzles.Count)];
        int amountObjects = (int)Mathf.Lerp((float)0, (float)puzzle.Length, diff);
        for (int i = 0; i < puzzle.Length; i++)
        {
            scene.objects[i].Setup(puzzle[i]);
            scene.objects[i].SetActive(false);
        }

        //Set active objects
        List<int> idxObjects = new List<int>();
        for (int i = 0; i < scene.objects.Length; i++) idxObjects.Add(i);
        for(int i = 0; i < amountObjects; i++)
        {
            var idx = idxObjects[Random.Range(0, idxObjects.Count)];
            idxObjects.Remove(idx);
            scene.objects[idx].SetActive(true);
        }

        //Decorations
        level.SetDecorationsEnabled(scene.decorations, true, 0.5f);

        //UI
        level.manager.menuGame.Show();
        if (level.GetLevelType() == Level.Type.P3) level.manager.menuGame.SetTimerActive(12f, OnTimerEnd);

        //Start
        setup = true;
    }

    /// <summary>
    /// Increments the player position index
    /// </summary>
    /// <param name="amount">Amount to increment</param>
    public void IncrementPositionIndex(int amount)
    {
        if (!setup) return;
        idxObject += amount;
        StepObjects();

        if(idxObject > scene.objects.Length) //Game won
        {
            lerpPos = Lerp.Get(2f, tObjectEnd.position, tEnd.position);
            level.StartEndTimer(2f);
            level.manager.fx.Play("points_normal");
            level.manager.menuGame.SetText("Aw man!");
            setup = false;
            return;
        }

        SetPositionIndex(idxObject);
        level.manager.fx.Play("blip");
    }

    /// <summary>
    /// Decrements steps of all falling objects
    /// </summary>
    void StepObjects()
    {
        for (int i = 0; i < scene.objects.Length; i++)
        {
            int step = scene.objects[i].Step();
            if (step == 0)
            {
                if (idxObject == i) //Game lost
                {
                    setup = false;
                    level.AddCounter();
                    scene.objects[i].AnimateFall();
                }
                else //Object fell without hitting
                {
                    scene.objects[i].Fall();
                }
            }
        }
    }

    /// <summary>
    /// Sets the position index of the player
    /// </summary>
    /// <param name="idx">The index</param>
    void SetPositionIndex(int idx)
    {
        if (idx == -1) SetTessaPosition(tObjectStart.position);
        else if (idx == scene.objects.Length) SetTessaPosition(tObjectEnd.position);
        else SetTessaPosition(scene.objects[idx].transform.position);
    }

    /// <summary>
    /// Ends the level
    /// </summary>
    public void End()
    {
        level.EndLevel();
    }

    /// <summary>
    /// Sets the position of the player
    /// </summary>
    /// <param name="v">Position</param>
    void SetTessaPosition(Vector3 v)
    {
        tTessa.position = v;
    }

    /// <summary>
    /// Ends the level and adds to fail counter
    /// </summary>
    void OnTimerEnd()
    {
        if (!setup) return;
        setup = false;
        level.AddCounter();
        level.EndLevel();
    }
}
