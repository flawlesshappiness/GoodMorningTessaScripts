using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Level : MonoBehaviour
{
    public const bool DEBUG = false;

    [HideInInspector]
    public LevelManager manager;
    public UnityEvent onStartLevel;
    public string nameNormal;
    public string descNormal;
    public string nameMary;
    public string descMary;
    public string nameP3;
    public string descP3;

    private bool started;
    private bool amara;

    // Start is called before the first frame update
    void Start()
    {

    }
    
    // Update is called once per frame
    void Update()
    {
        if (DEBUG && started && Input.GetKeyDown(KeyCode.Escape)) EndLevel();
    }

    /// <summary>
    /// Starts the level
    /// </summary>
    /// <param name="mgLevel">The level manager</param>
    public void StartLevel(LevelManager mgLevel)
    {
        manager = mgLevel;
        onStartLevel.Invoke();
        started = true;
    }

    /// <summary>
    /// Starts the timer that ends the level
    /// </summary>
    /// <param name="time">The timer time</param>
    public void StartEndTimer(float time)
    {
        manager.menuGame.StopTimer();
        StartCoroutine(EndTimer(time));
    }

    /// <summary>
    /// The coroutine for the end timer
    /// </summary>
    /// <param name="time">The timer time</param>
    /// <returns></returns>
    IEnumerator EndTimer(float time)
    {
        yield return new WaitForSeconds(time);

        EndLevel();
    }

    /// <summary>
    /// Ends the level, and starts the next minigame
    /// </summary>
    public void EndLevel()
    {
        manager.menuGame.StopTimer();
        manager.fx.ResetAdjustment();
        manager.music.Stop();
        manager.CompleteLevel();
    }

    /// <summary>
    /// Returns true if mary is active, else false
    /// </summary>
    /// <returns>if mary is active</returns>
    public bool IsMaryActive()
    {
        return type == Type.MARY;
    }

    /// <summary>
    /// Returns true if P3 is active, else false
    /// </summary>
    /// <returns>if P3 is active</returns>
    public bool IsP3Active()
    {
        return type == Type.P3;
    }

    /// <summary>
    /// Adds a counter to the active type, Mary/P3/None
    /// </summary>
    public void AddCounter()
    {
        if (type == Level.Type.MARY) manager.counterMary.Add(1);
        else if (type == Level.Type.P3) manager.counterP3.Add(1);
        else manager.counterLoss.Add(1);
    }

    #region DECORATIONS
    /// <summary>
    /// Enables/disables a percentage of decorations from an array. If disable, then all is disabled.
    /// </summary>
    /// <param name="decorations">Array of decorations</param>
    /// <param name="enabled">Enabled</param>
    /// <param name="perc">Percentage of decorations to enable/disable</param>
    public void SetDecorationsEnabled(GameObject[] decorations, bool enabled, float perc)
    {
        if (enabled)
        {
            if (decorations.Length > 0)
                UpdateDecorations(decorations, perc);
        }
        else
        {
            foreach (GameObject g in decorations)
                g.SetActive(false);
        }
    }

    /// <summary>
    /// Enables an array of decorations, and disables the rest
    /// </summary>
    /// <param name="decorations">The array of decorations</param>
    /// <param name="perc">The percentage to enable</param>
    void UpdateDecorations(GameObject[] decorations, float perc)
    {
        List<GameObject> unused = new List<GameObject>(decorations);
        int amount = (int)(Mathf.Max(1, decorations.Length * perc));
        for (int i = 0; i < amount; i++)
        {
            GameObject chosen = unused[Random.Range(0, unused.Count)];
            unused.Remove(chosen);
            chosen.SetActive(true);
        }

        foreach (GameObject g in unused) g.SetActive(false);
    }
    #endregion
    #region TYPE
    public enum Type { NORMAL, MARY, P3 }
    private Type type;

    /// <summary>
    /// Sets the level type
    /// </summary>
    /// <param name="type">The level type</param>
    public void SetLevelType(Type type)
    {
        this.type = type;
    }

    /// <summary>
    /// Returns the level type
    /// </summary>
    /// <returns>The level type</returns>
    public Type GetLevelType()
    {
        return type;
    }
    #endregion
    #region AMARA
    /// <summary>
    /// Sets the Amara object active
    /// </summary>
    /// <param name="active">Active</param>
    public void SetAmara(bool active)
    {
        amara = active;
    }

    /// <summary>
    /// Returns true if Amara is active, false if not
    /// </summary>
    /// <returns>If Amara is active</returns>
    public bool IsAmaraActive()
    {
        return amara;
    }
    #endregion
}
