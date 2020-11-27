using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Lerp<T> {
	private Lerp.Type _type;

	private float _time { get; set; }
	private float _startTime { get; set; }
	private float _endTime { get; set; }
	private T _startValue { get; set; }
	private T _endValue { get; set; }
	private Func<T, T, float, T> _funcLerp;
	private Func<float> _funcTime;

	public Lerp(float time, T start, T end, Lerp.Type type, Func<T, T, float, T> lerpFunc, Func<float> timeFunc)
	{
		_time = time;
		_startTime = timeFunc();
		_endTime = _startTime + time;
		_startValue = start;
		_endValue = end;
		_funcLerp = lerpFunc;
		_funcTime = timeFunc;
		_type = type;
	}

	/// <summary>
	/// Gets the lerp percentage.
	/// Calculations taken from https://chicounity3d.wordpress.com/2014/05/23/how-to-lerp-like-a-pro/ by Robert Utter
	/// </summary>
	/// <returns>The percentage.</returns>
	public float GetPerc()
	{
        float t = GetPercTime();
        return Lerp.GetPerc(t, _type);
	}

    /// <summary>
    /// Gets how close the lerp is to the end time, in percentage
    /// </summary>
    /// <returns>The percentage</returns>
    public float GetPercTime()
    {
        return TimePerc(_startTime, _endTime);
    }

    /// <summary>
    /// Percentage of current time between start and end
    /// </summary>
    /// <param name="start">The star time</param>
    /// <param name="end">The end time</param>
    /// <returns>The percentage</returns>
	float TimePerc(float start, float end)
	{
        float time = _funcTime();
        if (time > end) time = end;

		return (time - start) / (end - start);
	}

    /// <summary>
    /// Returns true if the lerp has finished
    /// </summary>
    /// <returns>true if finished, else false</returns>
	public bool IsFinished()
	{
		return GetPerc() >= 0.999f;
	}

    /// <summary>
    /// Gets the lerp value and the current time
    /// </summary>
    /// <returns>Value of type T</returns>
	public T GetLerp()
	{
		return _funcLerp(_startValue, _endValue, GetPerc());
	}

    /// <summary>
    /// Resets start and end time
    /// </summary>
	public void Reset()
	{
		_startTime = _funcTime();
		_endTime = _startTime + _time;
	}

    /// <summary>
    /// Gets the final value in the lerp
    /// </summary>
    /// <returns>Final value of type T</returns>
	public T GetEndValue()
	{
		return _endValue;
	}

    /// <summary>
    /// Sets the final value in the lerp
    /// </summary>
    /// <param name="endvalue">The new final value</param>
	public void SetEndValue(T endvalue)
	{
		_endValue = endvalue;
	}
}

public class Lerp
{
	public enum Type { LINEAR, EASE_OUT, EASE_IN, EXPONENTIAL, SMOOTHSTEP, SMOOTHERSTEP }

	#region FLOAT
	public static Lerp<float> Get(float time, float start, float end)
	{
		return new Lerp<float>(time, start, end, Lerp.Type.LINEAR, Mathf.Lerp, GameTime.GetGlobalTime);
	}

    public static Lerp<float> Get(float time, float start, float end, Lerp.Type type)
    {
        return new Lerp<float>(time, start, end, type, Mathf.Lerp, GameTime.GetGlobalTime);
    }

    public static Lerp<float> Get(float time, float start, float end, Lerp.Type type, Func<float> funcTime)
	{
		return new Lerp<float>(time, start, end, type, Mathf.Lerp, funcTime);
	}
	#endregion
	#region COLOR
	public static Lerp<Color> Get(float time, Color start, Color end)
	{
		return new Lerp<Color>(time, start, end, Lerp.Type.LINEAR, Color.Lerp, GameTime.GetGlobalTime);
	}

    public static Lerp<Color> Get(float time, Color start, Color end, Lerp.Type type)
    {
        return new Lerp<Color>(time, start, end, type, Color.Lerp, GameTime.GetGlobalTime);
    }

    public static Lerp<Color> Get(float time, Color start, Color end, Lerp.Type type, Func<float> funcTime)
	{
		return new Lerp<Color>(time, start, end, type, Color.Lerp, funcTime);
	}
	#endregion
	#region VECTOR3
	public static Lerp<Vector3> Get(float time, Vector3 start, Vector3 end)
	{
		return new Lerp<Vector3>(time, start, end, Lerp.Type.LINEAR, Vector3.Lerp, GameTime.GetGlobalTime);
	}

    public static Lerp<Vector3> Get(float time, Vector3 start, Vector3 end, Lerp.Type type)
    {
        return new Lerp<Vector3>(time, start, end, type, Vector3.Lerp, GameTime.GetGlobalTime);
    }

    public static Lerp<Vector3> Get(float time, Vector3 start, Vector3 end, Lerp.Type type, Func<float> funcTime)
	{
		return new Lerp<Vector3>(time, start, end, type, Vector3.Lerp, funcTime);
	}
	#endregion
	#region QUATERNION
	public static Lerp<Quaternion> Get(float time, Quaternion start, Quaternion end)
	{
		return new Lerp<Quaternion>(time, start, end, Lerp.Type.LINEAR, Quaternion.Lerp, GameTime.GetGlobalTime);
	}

    public static Lerp<Quaternion> Get(float time, Quaternion start, Quaternion end, Lerp.Type type)
    {
        return new Lerp<Quaternion>(time, start, end, type, Quaternion.Lerp, GameTime.GetGlobalTime);
    }

    public static Lerp<Quaternion> Get(float time, Quaternion start, Quaternion end, Lerp.Type type, Func<float> funcTime)
	{
		return new Lerp<Quaternion>(time, start, end, type, Quaternion.Lerp, funcTime);
	}
	#endregion

    /// <summary>
    /// Calls action with the current value of lerp.
    /// </summary>
    /// <typeparam name="T">The type used in this lerp</typeparam>
    /// <param name="lerp">The lerp to get a value from</param>
    /// <param name="action">The action to call with a value as argument</param>
    /// <param name="actionEnd">The final action to call, when the lerp is finished</param>
    /// <returns>Returns true if the lerp is finished, else false</returns>
	public static bool Apply<T>(Lerp<T> lerp, Action<T> action, Action actionEnd)
	{
		if(lerp == null) return false;
		if(lerp.IsFinished())
		{
			actionEnd();
			return true;
		}
		else
		{
            action?.Invoke(lerp.GetLerp());
			return false;
		}
	}

    /// <summary>
    /// Calls action with the current value of lerp. Calls the action a final time, when the lerp is finished.
    /// </summary>
    /// <typeparam name="T">The type used in this lerp</typeparam>
    /// <param name="lerp">The lerp to get a value from</param>
    /// <param name="action">The action to call with a value as argument</param>
    /// <returns>Returns true if the lerp is finished, else false</returns>
	public static bool Apply<T>(Lerp<T> lerp, Action<T> action)
	{
		return Apply(lerp, action, delegate {
			action?.Invoke(lerp.GetEndValue());
		});
	}

    /// <summary>
    /// Get lerp percentage value from lerp type
    /// </summary>
    /// <param name="t">Lerp value</param>
    /// <param name="type">Lerp type</param>
    /// <returns>The lerp percentage value</returns>
    public static float GetPerc(float t, Lerp.Type type)
    {
        switch (type)
        {
            case Lerp.Type.LINEAR: return t;
            case Lerp.Type.EASE_OUT: return Mathf.Sin(t * Mathf.PI * 0.5f);
            case Lerp.Type.EASE_IN: return 1f - Mathf.Cos(t * Mathf.PI * 0.5f);
            case Lerp.Type.EXPONENTIAL: return t * t;
            case Lerp.Type.SMOOTHSTEP: return t * t * (3f - 2f * t);
            case Lerp.Type.SMOOTHERSTEP: return t * t * t * (t * (6f * t - 15f) + 10f);
            default: return t;
        }
    }
}
