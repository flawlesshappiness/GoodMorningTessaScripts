using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GraphicController<T> : MonoBehaviour {

	private T renderer;

	private float cdbFadeDefault = 0.1f;
	private Lerp<Color> lerpColor;

	// Update is called once per frame
	void Update () {
		if(Lerp.Apply(lerpColor, SetColor)) lerpColor = null;
	}

    /// <summary>
    /// Returns the renderer
    /// </summary>
    /// <returns>The renderer</returns>
	public T GetRenderer()
	{
		if(renderer == null) renderer = GetComponent<T>();
		return renderer;
	}

	#region ACTIVE
    /// <summary>
    /// Sets the renderer active/inactive
    /// </summary>
    /// <param name="active">Active</param>
	public abstract void SetRendererActive(bool active);

    /// <summary>
    /// Sets this renderer, and all child renderers active/inactive
    /// </summary>
    /// <param name="active">Active</param>
	public void SetRendererActiveAll(bool active)
	{
		SetRendererActive(active);
		foreach(GraphicController<T> c in GetComponentsInChildren<GraphicController<T>>()) c.SetRendererActive(active);
	}
	#endregion
	#region COLOR
    /// <summary>
    /// Set renderer color
    /// </summary>
    /// <param name="c">Color</param>
	public abstract void SetColor(Color c);

    /// <summary>
    /// Returns renderer color
    /// </summary>
    /// <returns>The color</returns>
	public abstract Color GetColor();

    /// <summary>
    /// Set color of this renderer, and all child renderers
    /// </summary>
    /// <param name="color">The color</param>
	public void SetColorAll(Color color)
	{
		SetColor(color);
		foreach(GraphicController<T> c in GetComponentsInChildren<GraphicController<T>>()) c.SetColor(color);
	}

    /// <summary>
    /// Fades the color of this renderer, and all child renderers
    /// </summary>
    /// <param name="time">Fade time</param>
    /// <param name="start">Start color</param>
    /// <param name="end">End color</param>
	public void FadeColorAll(float time, Color start, Color end)
	{
		FadeColor(time, start, end);
		foreach(GraphicController<T> c in GetComponentsInChildren<GraphicController<T>>()) c.FadeColor(time, start, end);
	}

    /// <summary>
    /// Fades the color of this renderer, and all child renderers
    /// </summary>
    /// <param name="time">Fade time</param>
    /// <param name="end">End color</param>
	public void FadeColorAll(float time, Color end)
	{
		FadeColor(time, end);
		foreach(GraphicController<T> c in GetComponentsInChildren<GraphicController<T>>()) c.FadeColor(time, end);
	}

    /// <summary>
    /// Fades the color of this renderer
    /// </summary>
    /// <param name="time">Fade time</param>
    /// <param name="start">Start color</param>
    /// <param name="end">End color</param>
	public void FadeColor(float time, Color start, Color end)
	{
		lerpColor = Lerp.Get(time, start, end);
	}

    /// <summary>
    /// Fades the color of this renderer
    /// </summary>
    /// <param name="time">Fade time</param>
    /// <param name="end">End color</param>
	public void FadeColor(float time, Color end)
	{
		FadeColor(time, GetColor(), end);
	}

    /// <summary>
    /// Fades the color of this renderer, with a default fade time
    /// </summary>
    /// <param name="end">End color</param>
	public void FadeColor(Color end)
	{
		FadeColor(cdbFadeDefault, end);
	}

    /// <summary>
    /// True if currently fading, else false
    /// </summary>
    /// <returns>Is fading</returns>
	public bool IsFading()
	{
		return lerpColor != null;
	}
	#endregion
	#region ALPHA
    /// <summary>
    /// Set renderer alpha value
    /// </summary>
    /// <param name="alpha">The alpha value</param>
	public abstract void SetAlpha(float alpha);

    /// <summary>
    /// Get renderer alpha value
    /// </summary>
    /// <returns>The alpha value</returns>
	public abstract float GetAlpha();

    /// <summary>
    /// Set the alpha value of this renderer, and all child renderers
    /// </summary>
    /// <param name="alpha">The alpha value</param>
	public void SetAlphaAll(float alpha)
	{
		SetAlpha(alpha);
		foreach(GraphicController<T> c in GetComponentsInChildren<GraphicController<T>>()) c.SetAlpha(alpha);
	}

    /// <summary>
    /// Fades the alpha value of this renderer, and all child renderers
    /// </summary>
    /// <param name="time">Fade time</param>
    /// <param name="start">Start alpha</param>
    /// <param name="end">End alpha</param>
	public void FadeAlphaAll(float time, float start, float end)
	{
		FadeAlpha(time, start, end);
		foreach(GraphicController<T> c in GetComponentsInChildren<GraphicController<T>>()) c.FadeAlpha(time, start, end);
	}

    /// <summary>
    /// Fades the alpha value of this renderer, and all child renderers
    /// </summary>
    /// <param name="time">Fade time</param>
    /// <param name="end">End alpha</param>
	public void FadeAlphaAll(float time, float end)
	{
		FadeAlpha(time, end);
		foreach(GraphicController<T> c in GetComponentsInChildren<GraphicController<T>>()) c.FadeAlpha(time, end);
	}

    /// <summary>
    /// Fades the alpha value of this renderer
    /// </summary>
    /// <param name="time">Fade time</param>
    /// <param name="start">Start alpha</param>
    /// <param name="end">End alpha</param>
	public void FadeAlpha(float time, float start, float end)
	{
		lerpColor = Lerp.Get(time, GetColor().SetAlpha(start), GetColor().SetAlpha(end));
	}

    /// <summary>
    /// Fade the alpha value of this renderer
    /// </summary>
    /// <param name="time">Fade time</param>
    /// <param name="end">End alpha</param>
	public void FadeAlpha(float time, float end)
	{
		FadeAlpha(time, GetAlpha(), end);
	}

    /// <summary>
    /// Fade the alpha value of this renderer, with a default fade time
    /// </summary>
    /// <param name="end">End alpha</param>
	public void FadeAlpha(float end)
	{
		FadeAlpha(cdbFadeDefault, end);
	}
	#endregion
}
