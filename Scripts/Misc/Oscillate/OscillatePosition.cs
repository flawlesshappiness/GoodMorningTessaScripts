using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscillatePosition : Oscillator<Vector3> {

	private Transform t;

	public override void Setup ()
	{
		t = transform;
	}

    public override Lerp<Vector3> GetLerp (float time, Vector3 start, Vector3 end, Lerp.Type type)
	{
		return Lerp.Get(time, start, end, type, GetTime);
	}

	public override void ApplyLerp (Vector3 start, Vector3 value)
	{
		t.localPosition = start + value;
	}

	public override Vector3 GetValue ()
	{
		return t.localPosition;
	}
}
