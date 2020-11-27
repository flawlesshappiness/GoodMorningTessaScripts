using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextController : GraphicController<Text> {
	public override void SetColor (Color c)
	{
		GetRenderer().color = c;
	}

	public override Color GetColor ()
	{
		return GetRenderer().color;
	}

	public override void SetAlpha (float alpha)
	{
		SetColor(GetRenderer().color.SetAlpha(alpha));
	}

	public override float GetAlpha ()
	{
		return GetColor().a;
	}

	public override void SetRendererActive (bool active)
	{
		GetRenderer().enabled = active;
	}

	public void SetText(string text)
	{
		GetRenderer().text = text;
	}
}
