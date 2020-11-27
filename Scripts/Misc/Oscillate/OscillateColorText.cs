using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class OscillateColorText : Oscillator<Color> {

	private Text ren;

	public override void Setup ()
	{
		ren = GetComponent<Text>();
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
