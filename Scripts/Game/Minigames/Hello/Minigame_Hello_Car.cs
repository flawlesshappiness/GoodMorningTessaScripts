using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame_Hello_Car : MonoBehaviour
{
    public Minigame_Hello game;
    public AudioController audioDrive;
    public bool isLoss;

    private Lerp<float> lerpPos;
    private float offset;
    private bool hasPlayedAudio;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Lerp.Apply(lerpPos, SetPos))
        {
            lerpPos = null;
            if (isLoss) game.Lose();
        }

        if (!hasPlayedAudio && (GetPerc() + offset) > 0.3f)
        {
            hasPlayedAudio = true;
            audioDrive.Play();
        }
    }

    /// <summary>
    /// Starts lerping the position of the car
    /// </summary>
    /// <param name="time">Lerp time</param>
    /// <param name="offset">Lerp offset</param>
    public void StartDriving(float time, float offset)
    {
        this.offset = offset;
        lerpPos = Lerp.Get(time, 0f, 1f);
    }

    /// <summary>
    /// Sets the position of the car from a lerp value
    /// </summary>
    /// <param name="f">The lerp value</param>
    void SetPos(float f)
    {
        transform.position = Vector3.Lerp(game.tCarStart.position, game.tCarEnd.position, f + offset);
    }

    /// <summary>
    /// Gets progress to finish, as a percentage
    /// </summary>
    /// <returns>The percentage</returns>
    public float GetPerc()
    {
        if (lerpPos == null) return 0f;
        return lerpPos.GetLerp() + offset;
    }
}
