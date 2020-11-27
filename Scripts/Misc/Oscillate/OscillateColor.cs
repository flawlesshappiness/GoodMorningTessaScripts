using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class OscillateColor : Oscillator<Color> {

	private SpriteRenderer ren;

	public override void Setup ()
	{
		ren = GetComponent<SpriteRenderer>();
	}

	public override Lerp<Color> GetLerp (float time, Color start, Color end, Lerp.Type type)
	{
		return Lerp.Get(time, start, end, type, GetTime);
	}

	public override void ApplyLerp (Color start, Color value)
	{
		ren.color = value;
	}

	public override Color GetValue ()
	{
		return ren.color;
	}
}
