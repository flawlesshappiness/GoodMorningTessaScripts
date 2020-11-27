using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTime : MonoBehaviour {

	public static float time;
    public static float deltaTime;

	private static float mult = 1f;
	private static int idxMult = 1;
	private static float[] mults = new float[]{
		0.5f, 1f, 2f, 3f, 4f
	};

	private static bool paused;
	 
	// Use this for initialization
	void Start () {
		SetMultiplier(1f);
	}
	
	// Update is called once per frame
	void Update () {
		if(!paused)
		{
            deltaTime = UnityEngine.Time.deltaTime * mult;
            time += deltaTime;
		}
	}

    /// <summary>
    /// Returns the global time of the Application
    /// </summary>
    /// <returns>The global time</returns>
	public static float GetGlobalTime()
	{
		return UnityEngine.Time.time;
	}

	#region GAMETIME
    /// <summary>
    /// Returns the current game time
    /// </summary>
    /// <returns>The game time</returns>
	public static float Time()
	{
		return time;
	}

    /// <summary>
    /// Returns true if the game time is paused, else false
    /// </summary>
    /// <returns>if paused</returns>
	public static bool IsPaused()
	{
		return paused;
	}

    /// <summary>
    /// Pauses the game time
    /// </summary>
	public static void Pause()
	{
		if(paused) return;
		paused = true;
	}

    /// <summary>
    /// Unpauses the game time
    /// </summary>
	public static void Resume()
	{
		if(!paused) return;
		paused = false;
	}

	#region MULTIPLIER
    /// <summary>
    /// Returns the game time multiplier
    /// </summary>
    /// <returns>The multiplier</returns>
	public static float GetMultiplier()
	{
		return mult;
	}

    /// <summary>
    /// Set the game time multiplier
    /// </summary>
    /// <param name="mult">The multiplier</param>
	public static void SetMultiplier(float mult)
	{
        GameTime.mult = mult;
	}

    /// <summary>
    /// Set game time multiplier to the next in the array
    /// </summary>
	public static void NextMultiplier()
	{
		idxMult++;
		if(idxMult >= mults.Length) idxMult = mults.Length - 1;
		SetMultiplier(mults[idxMult]);
	}

    /// <summary>
    /// Set game time multiplier to the previous in the array
    /// </summary>
	public static void PrevMultiplier()
	{
		idxMult--;
		if(idxMult < 0) idxMult = 0;
		SetMultiplier(mults[idxMult]);
	}

    /// <summary>
    /// Returns the current multiplier index
    /// </summary>
    /// <returns>The multiplier index</returns>
	public static int GetMultiplierIndex()
	{
		return idxMult;
	}
	#endregion
	#endregion
}
