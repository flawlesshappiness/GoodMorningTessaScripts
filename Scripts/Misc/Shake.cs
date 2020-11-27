using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    public Transform target;

    private Vector3 posStart;

    public float size; //Radius of the shake
    public float time; //Time between shakes

    public bool startActive;
    private bool active;
    private bool setup;

    private Cooldown cdShake;
    private Lerp<float> lerpSize;
    private Lerp<float> lerpTime;

    //Awake
    private void Awake()
    {
        Setup();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!setup) return;
        if (!active) return;
        if (Lerp.Apply(lerpSize, SetSize)) lerpSize = null;
        if (Lerp.Apply(lerpTime, SetTime)) lerpTime = null;
        DoShake();
    }

    /// <summary>
    /// Does a single displacement of the gameobject
    /// </summary>
    void DoShake()
    {
        if (!cdShake.IsFinished()) return;
        cdShake.Start();

        target.localPosition = posStart.ToVector2() + Random.insideUnitCircle * size;
    }

    /// <summary>
    /// Sets up the shake
    /// </summary>
    public void Setup()
    {
        posStart = target.localPosition;
        cdShake = new Cooldown(time);
        setup = true;
        SetActive(startActive);
    }

    /// <summary>
    /// Enables/disables the shake
    /// </summary>
    /// <param name="active">Active</param>
    public void SetActive(bool active)
    {
        this.active = active;

        if (!active)
        {
            target.localPosition = posStart;
        }
    }

    #region TIME
    /// <summary>
    /// Lerp time between shakes
    /// </summary>
    /// <param name="value">New time value</param>
    /// <param name="time">Lerp time</param>
    public void FadeTime(float value, float time)
    {
        lerpTime = Lerp.Get(time, this.time, value);
    }

    /// <summary>
    /// Set the time between shakes
    /// </summary>
    /// <param name="time">The time</param>
    public void SetTime(float time)
    {
        this.time = time;
    }
    #endregion
    #region SIZE
    /// <summary>
    /// Lerp the size of the shakes
    /// </summary>
    /// <param name="size">New size value</param>
    /// <param name="time">Lerp time</param>
    public void FadeSize(float size, float time)
    {
        lerpTime = Lerp.Get(time, this.size, size);
    }

    /// <summary>
    /// Set the size of the shakes
    /// </summary>
    /// <param name="size">The size</param>
    public void SetSize(float size)
    {
        this.size = size;
    }
    #endregion
}
