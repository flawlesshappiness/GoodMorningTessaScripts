using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Oscillator<T> : MonoBehaviour {

	[Space]
	public Lerp.Type type;
	public T valueMin;
	public T valueMax;

	[Space]
	public float timeMin;
	public float timeMax;

    [Space]
    public bool oscillating = true;
    public bool randomStart = true;

	private bool start;
	private Lerp<T> lerp;
	private bool lerpingToMax;
	private T valueStart;

	// Use this for initialization
	void Start () {
		Setup();

		valueStart = GetValue();

		if(randomStart) lerpingToMax = Random.Range(0, 2) == 0;
		NextLerp();
		start = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(!start) return;
		if(Lerp.Apply(lerp, ApplyLerp)) NextLerp();
	}

    /// <summary>
    /// Restarts the oscillating lerp
    /// </summary>
	void NextLerp()
	{
		float time = Random.Range(timeMin, timeMax);

		if(oscillating) lerpingToMax = !lerpingToMax;
		ApplyLerp(lerpingToMax ? valueMin : valueMax);
		if(lerpingToMax) lerp = GetLerp(time, valueMin, valueMax, type);
		else lerp = GetLerp(time, valueMax, valueMin, type);
	}

    /// <summary>
    /// Applies the lerp to the object
    /// </summary>
    /// <param name="value"></param>
	void ApplyLerp(T value)
	{
		ApplyLerp(valueStart, value);
	}

    /// <summary>
    /// returns the current global time
    /// </summary>
    /// <returns>The global time</returns>
	public float GetTime()
	{
		return Time.time;
	}

	public abstract void Setup();
	public abstract Lerp<T> GetLerp(float time, T start, T end, Lerp.Type type);
	public abstract void ApplyLerp(T start, T value);
	public abstract T GetValue();
}
