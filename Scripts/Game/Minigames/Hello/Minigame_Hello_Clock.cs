using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame_Hello_Clock : MonoBehaviour
{
    public Minigame_Hello game;

    public float angleStart;
    public float angleEnd;

    private bool toEnd;

    private Lerp<Quaternion> lerpRot;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Lerp.Apply(lerpRot, SetRotation))
        {
            lerpRot = null;
            game.ClockEnd();
        }
    }

    /// <summary>
    /// Starts a lerp to rotate the clock arm
    /// </summary>
    /// <param name="time">Lerp time</param>
    public void Rotate(float time)
    {
        float start = toEnd ? angleStart : angleEnd;
        float end = toEnd ? angleEnd : angleStart;
        lerpRot = Lerp.Get(time, Quaternion.Euler(0f, 0f, start), Quaternion.Euler(0f, 0f, end));

        toEnd = !toEnd;
    }

    /// <summary>
    /// Gets progress to finish as percentage
    /// </summary>
    /// <returns>The percentage</returns>
    public float GetPerc()
    {
        if (lerpRot == null) return 0f;
        return lerpRot.GetPerc();
    }

    /// <summary>
    /// Sets rotation of the object
    /// </summary>
    /// <param name="q">The rotation</param>
    void SetRotation(Quaternion q)
    {
        transform.rotation = q;
    }
}
