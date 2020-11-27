using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cooldown {

	private float _cdb; //Base cooldown
	private float _cds; //Start time of cooldown
	private float _cd; //End time of cooldown

	private System.Func<float> _funcTime;

	public Cooldown (float time)
	{
		_cdb = time;
		_funcTime = GetGlobalTime;
	}

	public Cooldown (float time, System.Func<float> funcTime)
	{
		_cdb = time;
		_funcTime = funcTime;
	}

    /// <summary>
    /// Sets new start and end times. Can restart the time.
    /// </summary>
	public void Start()
	{
		_cd = _funcTime() + _cdb;
		_cds = _funcTime();
	}

    /// <summary>
    /// Simply calls Start()
    /// </summary>
    public void Restart()
    {
        Start();
    }

    /// <summary>
    /// Sets the end time to the start time. Stops the timer.
    /// </summary>
	public void Stop()
	{
		_cd = _cds;
	}

    /// <summary>
    /// Returns true if the cooldown is finished, else false
    /// </summary>
    /// <returns>If finished</returns>
	public bool IsFinished()
	{
		return _funcTime() >= _cd;
	}

    /// <summary>
    /// Ends the cooldown
    /// </summary>
	public void SetFinished()
	{
		_cd = _funcTime();
	}

    /// <summary>
    /// Returns how close the cooldown is to end, in percentage
    /// </summary>
    /// <returns>The percentage</returns>
	public float GetPercent()
	{
		float perc = TimePerc(_cds, _cd);
		if(perc > 1f) return 1f;
		else return perc;
	}

    /// <summary>
    /// Returns how close the time is to end in percentage
    /// </summary>
    /// <param name="start">The start time</param>
    /// <param name="end">The end time</param>
    /// <returns>The percentage</returns>
	float TimePerc(float start, float end)
	{
		return (_funcTime() - start) / (end - start);
	}

    /// <summary>
    /// Returns the base cooldown value
    /// </summary>
    /// <returns>The base cooldown value</returns>
	public float GetBase()
	{
		return _cdb;
	}

    /// <summary>
    /// Returns the global time
    /// </summary>
    /// <returns>The global time</returns>
	float GetGlobalTime()
	{
		return Time.time;
	}
}
