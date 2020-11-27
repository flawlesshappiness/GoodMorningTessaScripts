using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageController : GraphicController<Image> {

    private bool _preserveAspectRatio;
	private Color _initColor;
    private Sprite _initSprite;

	private bool setup;

	void Setup()
	{
		var r = GetRenderer();
		r.preserveAspect = _preserveAspectRatio;

		_initColor = r.color;
		_initSprite = r.sprite;

		setup = true;
	}

	void CheckSetup()
	{
		if(!setup) Setup();
	}

	public override void SetColor (Color c)
	{
		CheckSetup();
		GetRenderer().color = c;
	}

	public override Color GetColor ()
	{
		CheckSetup();
		return GetRenderer().color;
	}

	public override void SetAlpha (float alpha)
	{
		CheckSetup();
		GetRenderer().color = GetRenderer().color.SetAlpha(alpha);
	}

	public override float GetAlpha ()
	{
		CheckSetup();
		return GetRenderer().color.a;
	}

	public override void SetRendererActive (bool active)
	{
		CheckSetup();
		GetRenderer().enabled = active;
	}
}
