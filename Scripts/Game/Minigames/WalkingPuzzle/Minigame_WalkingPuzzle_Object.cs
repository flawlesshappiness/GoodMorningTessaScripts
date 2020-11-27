using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame_WalkingPuzzle_Object : MonoBehaviour
{
    public Minigame_WalkingPuzzle game;

    [Space]
    public Transform g;
    public Transform shadow;
    public Transform fallEnd;

    [Space]
    public AudioController audioFall;

    private int steps;
    private float[] sizes = { 0f, 24f, 16f, 12f, 8f };

    private Lerp<Vector3> lerpFall;
    private Lerp<float> lerpPitch;

    private bool isActive;

    private readonly float audioPitchStart = 1.2f;
    private readonly float audioPitchEnd = 0.8f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Lerp.Apply(lerpFall, SetPosition))
        {
            lerpFall = null;
            game.End();
        }

        if (Lerp.Apply(lerpPitch, SetPitch))
        {
            lerpPitch = null;
        }
    }

    /// <summary>
    /// sets up the WalkingPuzzle object
    /// </summary>
    /// <param name="steps">The steps to fall</param>
    public void Setup(int steps)
    {
        this.steps = steps;
        UpdateStep();
    }

    /// <summary>
    /// Sets the object active/inactive
    /// </summary>
    /// <param name="active">Active</param>
    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
        isActive = active;
    }

    /// <summary>
    /// Decrements the object steps
    /// </summary>
    /// <returns>Remaining steps</returns>
    public int Step()
    {
        if (!isActive) return -1;
        if (steps <= 0) return -1;
        steps--;
        UpdateStep();
        return steps;
    }

    /// <summary>
    /// Updates the size of the shadow
    /// </summary>
    void UpdateStep()
    {
        float size = (steps >= sizes.Length) ? sizes[sizes.Length - 1] : sizes[steps];
        shadow.localScale = new Vector3(size, size, size);
    }

    /// <summary>
    /// Animates the object falling, and plays a sound
    /// </summary>
    public void AnimateFall()
    {
        if (!isActive) return;
        lerpFall = Lerp.Get(1f, g.position, fallEnd.position);

        audioFall.source.pitch = audioPitchStart;
        lerpPitch = Lerp.Get(1f, audioPitchStart, audioPitchEnd);
        audioFall.Play();
        
    }

    /// <summary>
    /// Instantly shows the object and plays the impact sound
    /// </summary>
    public void Fall()
    {
        g.position = shadow.position;
        game.level.manager.fx.Play("break");

        var ps = game.level.manager.particles.Instantiate("impact_fall");
        ps.transform.position = g.position;
    }

    /// <summary>
    /// Sets the position of the object
    /// </summary>
    /// <param name="pos">Position</param>
    void SetPosition(Vector3 pos)
    {
        g.position = pos;
    }

    /// <summary>
    /// Sets the pitch of the falling sound
    /// </summary>
    /// <param name="f">Pitch</param>
    void SetPitch(float f)
    {
        audioFall.source.pitch = f;
    }
}
