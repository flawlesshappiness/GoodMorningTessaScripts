using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.Rendering.Universal;

public class LevelManager : MonoBehaviour
{
    public const int MINIGAMES_PER_DAY = 5;
    public const int DAY_MAX = 5;

    [Space]
    [Header("MENUS")]
    public MenuMain menuMain;
    public MenuTransition menuTransition;
    public MenuEmpty menuGame;
    public MenuEnding menuEnding;

    [Space]
    [Header("MANAGERS")]
    public Camera cameraMain;
    public MusicManager music;
    public FxManager fx;
    public ParticleManager particles;

    [Space]
    [Header("SCENES")]
    public string[] minigames;
    private string minigameMorning = "GoodMorning";
    private string minigameAmara = "SpaceFlower";

    [Space]
    [Header("MUSIC")]
    public string[] levelMusics;

    [Space]
    [Header("COLORS")]
    public Light2D lightAmbient;
    public Color[] colorsDay;

    //Privates
    private int day;
    private readonly float[] difficulty = { 0.0f, 0.1f, 0.25f, 0.35f, 0.5f, 0.7f, 1f };
    private int[] dayAmountType = { 0, 0, 1, 2, 3, 4 };

    private int idxMinigame;
    private List<Minigame> dayMinigames = new List<Minigame>();

    private class Minigame
    {
        public Level.Type type;
        public bool hasSpecial;
        public string scene;

        public Minigame(Level.Type type, string scene)
        {
            this.type = type;
            this.scene = scene;
        }
    }

    private Minigame minigameCur;

    public EndingCounter counterMary = new EndingCounter(3);
    public EndingCounter counterP3 = new EndingCounter(3);
    public EndingCounter counterLoss = new EndingCounter(3);
    public EndingCounter counterAmara = new EndingCounter(5);

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Starts the game from day 0
    /// </summary>
    public void StartFirstDay()
    {
        day = 0;
        counterMary.Reset();
        counterP3.Reset();
        counterLoss.Reset();
        counterAmara.Reset();
        NextDay();
    }

    #region DAY
    /// <summary>
    /// Increments day index, and starts next minigame
    /// </summary>
    void NextDay()
    {
        day++;
        dayMinigames = GetMinigameList(MINIGAMES_PER_DAY);
        idxMinigame = -1;
        NextMinigame();
    }

    /// <summary>
    /// Increments minigame index, and loads the next minigame
    /// </summary>
    void NextMinigame()
    {
        if (idxMinigame >= dayMinigames.Count - 1)
        {
            NextDay();
            return;
        }

        idxMinigame++;
        UpdateLight(idxMinigame);
        LoadLevel(dayMinigames[idxMinigame]);
    }

    /// <summary>
    /// Returns the current difficulty [0.0 - 1.0]
    /// </summary>
    /// <returns>The difficulty</returns>
    public float GetDifficulty()
    {
        if (day >= difficulty.Length) return 1f;
        return difficulty[day];
    }

    /// <summary>
    /// Generates a list of minigames with given length
    /// </summary>
    /// <param name="length">The length of the list</param>
    /// <returns>The list of minigames</returns>
    List<Minigame> GetMinigameList(int length)
    {
        List<Minigame> list = new List<Minigame>();
        list.Add(new Minigame(Level.Type.NORMAL, minigameMorning));

        List<string> unused = new List<string>(minigames);
        for (int i = 0; i < length; i++)
        {
            if (unused.Count == 0) break;
            var game = unused[Random.Range(0, unused.Count)];
            list.Add(new Minigame(Level.Type.NORMAL, game));
            unused.Remove(game);
        }

        //Apply special types
        List<Level.Type> listTypes = new List<Level.Type>();
        int amountTypes = Mathf.Min(list.Count-1, dayAmountType[Mathf.Min(day, dayAmountType.Length - 1)]);
        bool mary = Random.Range(0, 2) == 0;
        for (int i = 0; i < amountTypes; i++)
        {
            listTypes.Add(mary ? Level.Type.MARY : Level.Type.P3);
            mary = !mary;
        }

        List<int> listIdx = new List<int>();
        for (int i = 1; i < list.Count; i++) listIdx.Add(i);
        foreach(Level.Type type in listTypes)
        {
            int idx = listIdx[Random.Range(0, listIdx.Count)];
            listIdx.Remove(idx);
            list[idx].type = type;
        }

        //Set special level
        list[Random.Range(1, list.Count)].hasSpecial = true;

        return list;
    }

    /// <summary>
    /// Returns the current day to DAY_MAX percentage
    /// </summary>
    /// <returns>The percentage</returns>
    public float GetDayPerc()
    {
        return (float)day / (float)DAY_MAX;
    }

    /// <summary>
    /// Returns true if current day is the final day, else false
    /// </summary>
    /// <returns>Is final day</returns>
    public bool IsFinalDay()
    {
        return day > DAY_MAX;
    }

    /// <summary>
    /// Adds the Amara minigame to the list of minigames
    /// </summary>
    public void AddAmaraLevel()
    {
        dayMinigames.Add(new Minigame(Level.Type.NORMAL, minigameAmara));
    }
    #endregion
    #region LEVEL
    /// <summary>
    /// Loads a level from a minigame
    /// </summary>
    /// <param name="minigame">The minigame</param>
    void LoadLevel(Minigame minigame)
    {
        menuTransition.ClearTexts();
        menuTransition.Show();

        minigameCur = minigame;
        StartCoroutine(LoadLevelTransition(minigame));
    }

    /// <summary>
    /// Coroutine to load and start a level from a minigame
    /// </summary>
    /// <param name="minigame">The minigame</param>
    /// <returns></returns>
    IEnumerator LoadLevelTransition(Minigame minigame)
    {
        menuTransition.SetText1(string.Concat("Day ", day, "/", DAY_MAX+1));
        fx.Play("heart_monitor");
        menuTransition.SetMaryEnabled(false);
        menuTransition.SetP3Enabled(false);

        SceneManager.LoadScene(minigame.scene, LoadSceneMode.Additive);
        float t = 2.0f;
        Level level = null;
        while (level == null)
        {
            yield return new WaitForSeconds(0.2f);
            t -= 0.2f;
            level = FindObjectOfType<Level>();
        }

        //Finish transition time
        if (t > 0f)
            yield return new WaitForSeconds(t);

        string name =
            minigame.type == Level.Type.NORMAL ? level.nameNormal :
            minigame.type == Level.Type.MARY ? level.nameMary :
            level.nameP3;

        string desc =
            minigame.type == Level.Type.NORMAL ? level.descNormal :
            minigame.type == Level.Type.MARY ? level.descMary :
            level.descP3;

        menuTransition.SetText2(name);
        menuTransition.SetText3(desc);

        menuTransition.SetMaryEnabled(minigame.type == Level.Type.MARY);
        menuTransition.SetP3Enabled(minigame.type == Level.Type.P3);

        fx.Play("heart_monitor");
        if (minigame.type == Level.Type.MARY) fx.Play("howl");
        else if (minigame.type == Level.Type.P3) fx.Play("bell");

        yield return new WaitForSeconds(3.0f);

        level.SetAmara(minigame.hasSpecial);
        level.SetLevelType(minigame.type);
        level.StartLevel(this);
    }

    /// <summary>
    /// Unloads the current level, and starts the next minigame
    /// </summary>
    public void CompleteLevel()
    {
        SceneManager.UnloadSceneAsync(minigameCur.scene);
        NextMinigame();
    }

    /// <summary>
    /// Unloads the current level, and displays the main menu
    /// </summary>
    public void ReturnToMainMenu()
    {
        SceneManager.UnloadSceneAsync(minigameCur.scene);
        menuMain.Show();
    }
    #endregion
    #region COUNTER
    public class EndingCounter
    {
        private int value;
        private int max = 999;
        private int percMax;

        public EndingCounter(int max)
        {
            percMax = max;
        }

        /// <summary>
        /// Resets the value to 0
        /// </summary>
        public void Reset()
        {
            value = 0;
        }

        /// <summary>
        /// Adds to the value
        /// </summary>
        /// <param name="amount">Amount to add</param>
        public void Add(int amount)
        {
            value += amount;
            if (value > max) value = max;
        }

        /// <summary>
        /// Sets the value
        /// </summary>
        /// <param name="value">New value</param>
        public void Set(int value)
        {
            this.value = value;
            if (value > max) value = max;
        }

        /// <summary>
        /// Returns the value
        /// </summary>
        /// <returns>The value</returns>
        public int GetValue()
        {
            return value;
        }

        /// <summary>
        /// Returns how close the counter is to max value, in percentage
        /// </summary>
        /// <returns>The percentage</returns>
        public float GetPerc()
        {
            return Mathf.Min(1f, ((float)value / (float)percMax));
        }
    }
    #endregion
    #region MUSIC
    /// <summary>
    /// Plays a random major music piece
    /// </summary>
    public void PlayMajorMusic()
    {
        string sMusic = levelMusics[Random.Range(0, levelMusics.Length)];
        sMusic += "_major";
        music.Play(sMusic);
    }

    /// <summary>
    /// Plays a random minor music piece
    /// </summary>
    public void PlayMinorMusic()
    {
        string sMusic = levelMusics[Random.Range(0, levelMusics.Length)];
        sMusic += "_minor";
        music.Play(sMusic);
    }

    /// <summary>
    /// Plays a random ticktock music piece
    /// </summary>
    public void PlayTickTockMusic()
    {
        string sMusic = levelMusics[Random.Range(0, levelMusics.Length)];
        sMusic += "_ticktock";
        music.Play(sMusic);
    }
    #endregion
    #region CAMERA
    /// <summary>
    /// Sets the main camera enabled/disabled
    /// </summary>
    /// <param name="enabled">Enabled</param>
    public void SetMainCameraEnabled(bool enabled)
    {
        cameraMain.gameObject.SetActive(enabled);
    }
    #endregion
    #region LIGHT
    /// <summary>
    /// Updates the ambient light color to a color from index
    /// </summary>
    /// <param name="idx">Index of color</param>
    void UpdateLight(int idx)
    {
        Color c = colorsDay.IndexOrMax(idx);
        lightAmbient.color = c;
    }
    #endregion
}
