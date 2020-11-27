using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpriteRotator : MonoBehaviour
{
    public Transform t;

    private float _z;
    private Lerp<Quaternion> lerpRotation;

    private bool setup;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Lerp.Apply(lerpRotation, _SetRotation)) lerpRotation = null;
    }

    /// <summary>
    /// Sets up the sprite rotator
    /// </summary>
    void Setup()
    {
        if (setup) return;

        _z = t.localRotation.eulerAngles.z;

        setup = true;
    }

    /// <summary>
    /// Sets z angle of the transform
    /// </summary>
    /// <param name="z">The z angle</param>
    public void SetAngle(float z)
    {
        SetAngle(z, 0.0f, Lerp.Type.LINEAR);
    }

    /// <summary>
    /// Lerps z angle of the transform from current to a new angle
    /// </summary>
    /// <param name="z">The new z angle</param>
    /// <param name="time">Lerp time</param>
    /// <param name="type">Lerp type</param>
    public void SetAngle(float z, float time, Lerp.Type type)
    {
        if (!setup) Setup();

        Quaternion qStart = GetCurrentRotation();
        Quaternion qEnd = GetQuaternion(_z + z);

        if (time == 0f)
        {
            _SetRotation(qEnd);
            return;
        }

        lerpRotation = Lerp.Get(time, qStart, qEnd, type, GameTime.GetGlobalTime);
    }

    /// <summary>
    /// Returns the local rotation of the transform
    /// </summary>
    /// <returns>The rotation</returns>
    Quaternion GetCurrentRotation()
    {
        if (lerpRotation != null) return lerpRotation.GetLerp();
        return t.localRotation;
    }

    /// <summary>
    /// Returns a z angle as a quaternion rotation
    /// </summary>
    /// <param name="z">The z angle</param>
    /// <returns>The rotation</returns>
    Quaternion GetQuaternion(float z)
    {
        return Quaternion.Euler(Vector3.zero.SetZ(z));
    }

    /// <summary>
    /// Sets the local rotation of the transform
    /// </summary>
    /// <param name="q">The rotation</param>
    void _SetRotation(Quaternion q)
    {
        if (!setup) Setup();
        t.localRotation = q;
    }
}
