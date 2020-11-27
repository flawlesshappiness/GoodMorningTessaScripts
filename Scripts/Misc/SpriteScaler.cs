using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteScaler : MonoBehaviour
{
    public Transform t;

    public bool x;
    public bool y;

    private Vector3 _scale;
    private Lerp<Vector3> lerpScale;

    private bool setup;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Lerp.Apply(lerpScale, _SetScale)) lerpScale = null;
    }

    /// <summary>
    /// Sets up the sprite scaler
    /// </summary>
    void Setup()
    {
        if (setup) return;

        _scale = t.localScale;

        setup = true;
    }

    /// <summary>
    /// Sets the scale percentage of the transform
    /// </summary>
    /// <param name="perc">The percentage</param>
    public void SetScale(float perc)
    {
        SetScale(perc, 0.0f, Lerp.Type.LINEAR);
    }

    /// <summary>
    /// Lerps the scale percentage from current to a new scale
    /// </summary>
    /// <param name="perc">Percentage</param>
    /// <param name="time">Lerp time</param>
    /// <param name="type">Lerp type</param>
    public void SetScale(float perc, float time, Lerp.Type type)
    {
        if (!setup) Setup();

        Vector3 start = GetScale();
        Vector3 end = _scale * perc;

        if(time == 0.0f)
        {
            _SetScale(end);
            return;
        }

        lerpScale = Lerp.Get(time, start, end, type, GameTime.GetGlobalTime);
    }

    /// <summary>
    /// Returns the local scale of the transform
    /// </summary>
    /// <returns>The local scale</returns>
    Vector3 GetScale()
    {
        return t.localScale;
    }

    /// <summary>
    /// Sets the local scale of the transform
    /// </summary>
    /// <param name="s">The scale</param>
    void _SetScale(Vector3 s)
    {
        if (!setup) Setup();

        Vector3 scale = new Vector3(
            x ? s.x : _scale.x,
            y ? s.y : _scale.y,
            _scale.z);

        t.localScale = scale;
    }
}
