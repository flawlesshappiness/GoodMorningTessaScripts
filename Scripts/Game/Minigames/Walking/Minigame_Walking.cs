using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame_Walking : MonoBehaviour
{
    public Level level;
    [Space]
    public Minigame_Walking_Character character;
    [Space]
    public Transform posObjectStart;
    public Transform posObjectEnd;
    public Minigame_Walking_Object[] objects;
    [Space]
    public GameObject[] decorationsNormal;
    public GameObject[] decorationsMary;
    public GameObject[] decorationsP3;
    [Space]
    public GameObject[] itemsMary;
    public GameObject[] itemsP3;

    private bool setup;

    private int amountObjects;
    private readonly int objectsMin = 1;
    private readonly int objectsMax = 3;

    private int amountScenes;
    private readonly int scenesMin = 2;
    private readonly int scenesMax = 5;

    private bool maryActive;
    private int sceneMary;
    private bool p3Active;
    private int sceneP3;

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
        if (posObjectStart == null || posObjectEnd == null) return;
        Gizmos.color = Color.green.SetAlpha(0.5f);
        Gizmos.DrawSphere(posObjectStart.position, 0.5f);
        Gizmos.DrawSphere(posObjectEnd.position, 0.5f);
        Gizmos.DrawLine(posObjectStart.position, posObjectEnd.position);
    }

    /// <summary>
    /// Sets up and starts the minigame
    /// </summary>
    public void StartGame()
    {
        //UI
        level.manager.menuGame.Show();

        //Setup difficulty
        float diff = level.manager.GetDifficulty();
        amountObjects = (int)Mathf.Lerp((float)objectsMin, (float)objectsMax, diff);
        amountScenes = (int)Mathf.Lerp((float)scenesMin, (float)scenesMax, diff);

        //Scene
        NextScene();
        character.StartMoving();

        //Setup items
        foreach (GameObject g in itemsMary) g.SetActive(false);
        foreach (GameObject g in itemsP3) g.SetActive(false);

        if (level.IsMaryActive())
        {
            maryActive = true;
            sceneMary = Random.Range(1, amountScenes);
        }

        if (level.IsP3Active())
        {
            p3Active = true;
            sceneP3 = Random.Range(1, amountScenes);
        }


        //Setup Mary & P3 decorations
        float pMary = level.manager.counterMary.GetPerc();
        float pP3 = pMary + level.manager.counterP3.GetPerc();
        float pChance = Random.Range(0f, 2f);

        if (pChance < pMary) //Mary scene
        {
            level.SetDecorationsEnabled(decorationsMary, true, level.manager.counterMary.GetPerc());
            level.SetDecorationsEnabled(decorationsP3, false, level.manager.counterP3.GetPerc());
            level.manager.PlayMinorMusic();
        }
        else if (pChance < pP3) //P3 scene
        {
            level.SetDecorationsEnabled(decorationsMary, false, level.manager.counterMary.GetPerc());
            level.SetDecorationsEnabled(decorationsP3, true, level.manager.counterP3.GetPerc());
            level.manager.PlayTickTockMusic();
        }
        else //Normal scene
        {
            level.SetDecorationsEnabled(decorationsMary, false, level.manager.counterMary.GetPerc());
            level.SetDecorationsEnabled(decorationsP3, false, level.manager.counterP3.GetPerc());
            level.manager.PlayMajorMusic();
        }

        setup = true;
    }

    /// <summary>
    /// Shows next scene, and ends minigame if no more scenes
    /// </summary>
    public void NextScene()
    {
        amountScenes--;
        if(amountScenes <= 0)
        {
            level.StartEndTimer(2f);
            level.manager.menuGame.SetText("I'm fine!");
            level.manager.fx.Play("points_normal");
            return;
        }

        level.SetDecorationsEnabled(decorationsNormal, true, Random.Range(0.2f, 0.8f));
        character.ResetPosition();

        //Setup objects
        List<Minigame_Walking_Object> unchosen = new List<Minigame_Walking_Object>(objects);
        List<Minigame_Walking_Object> chosen = new List<Minigame_Walking_Object>();
        for(int i = 0; i < amountObjects; i++)
        {
            var o = unchosen[Random.Range(0, unchosen.Count)];
            unchosen.Remove(o);
            chosen.Add(o);
            o.StartFalling();
        }

        foreach (Minigame_Walking_Object o in unchosen)
            o.ResetObject();

        //Setup items
        if (maryActive)
        {
            if(amountScenes == sceneMary)
            {
                var item = itemsMary[Random.Range(0, itemsMary.Length)];
                item.SetActive(true);
            }
            else
            {
                foreach (GameObject g in itemsMary)
                    g.SetActive(false);
            }
        }

        if (p3Active && amountScenes == sceneP3)
        {
            if (amountScenes == sceneP3)
            {
                var item = itemsP3[Random.Range(0, itemsP3.Length)];
                item.SetActive(true);
            }
            else
            {
                foreach (GameObject g in itemsP3)
                    g.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Adds to the loss counter, and ends the level
    /// </summary>
    public void OnHit()
    {
        level.manager.counterLoss.Add(1);
        level.EndLevel();
    }

    /// <summary>
    /// Disables the item and plays a sound
    /// </summary>
    /// <param name="item">The item</param>
    public void CollectMary(Minigame_Walking_Item item)
    {
        item.gameObject.SetActive(false);
        level.manager.fx.Play("points_mary");
    }

    /// <summary>
    /// Disables the item and plays a sound
    /// </summary>
    /// <param name="item">The item</param>
    public void CollectP3(Minigame_Walking_Item item)
    {
        item.gameObject.SetActive(false);
        level.manager.fx.Play("points_p3");
    }
}
