using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteTranslator : MonoBehaviour
{
    public Transform t;

    private Vector3 _pos;
    private Lerp<Vector3> lerpPos;

    private bool setup;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Lerp.Apply(lerpPos, _SetPosition)) lerpPos = null;
    }

    /// <summary>
    /// Sets up the translator
    /// </summary>
    void Setup()
    {
        if (setup) return;

        _pos = t.localPosition;

        setup = true;
    }

    /// <summary>
    /// Set the position of the transform
    /// </summary>
    /// <param name="pos">The position</param>
    public void SetPosition(Vector3 pos)
    {
        SetPosition(pos, 0.0f, Lerp.Type.LINEAR);
    }

    /// <summary>
    /// Lerp the transform from the start position to an end position
    /// </summary>
    /// <param name="pos">The end position</param>
    /// <param name="time">Lerp time</param>
    /// <param name="type">Lerp type</param>
    public void SetPosition(Vector3 pos, float time, Lerp.Type type)
    {
        if (!setup) Setup();

        Vector3 start = GetPos();
        Vector3 end = _pos + pos;

        if (time == 0.0f)
        {
            _SetPosition(end);
            return;
        }

        lerpPos = Lerp.Get(time, start, end, type, GameTime.GetGlobalTime);
    }

    /// <summary>
    /// Returns the local position of the transform
    /// </summary>
    /// <returns>The local position</returns>
    Vector3 GetPos()
    {
        return t.localPosition;
    }

    /// <summary>
    /// Sets the local position of the transform
    /// </summary>
    /// <param name="pos">The position</param>
    void _SetPosition(Vector3 pos)
    {
        if (!setup) Setup();
        t.localPosition = pos;
    }
}
