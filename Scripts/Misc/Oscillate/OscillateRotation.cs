using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscillateRotation : Oscillator<float> {

	private Transform t;

	public override void Setup ()
	{
		t = transform;
	}

	public override Lerp<float> GetLerp (float time, float start, float end, Lerp.Type type)
	{
		return Lerp.Get(time, start, end, type, GetTime);
	}

	public override void ApplyLerp (float start, float value)
	{
		t.eulerAngles = new Vector3(0f, 0f, start + value);
	}

	public override float GetValue ()
	{
		return t.eulerAngles.z;
	}
}
