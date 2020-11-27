using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteController : GraphicController<SpriteRenderer> {

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
		GetRenderer().color = GetRenderer().color.SetAlpha(alpha);
	}

	public override float GetAlpha ()
	{
		return GetRenderer().color.a;
	}

	public override void SetRendererActive (bool active)
	{
		GetRenderer().enabled = active;
	}
}
