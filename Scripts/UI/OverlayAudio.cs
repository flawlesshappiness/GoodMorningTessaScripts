using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayAudio : MonoBehaviour
{
    private enum State { FADEIN, STAY, FADEOUT }
    private State state = State.FADEOUT;

    public ImageController imgFx;
    public TextController textFx;
    public ImageController imgMusic;
    public TextController textMusic;

    [Space]
    public FxManager fx;

    private Lerp<float> lerpFade;
    private float[] percs = { 0f, 0.01f, 0.025f, 0.05f, 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 1f };
    private string[] sPercs = { "0%", "1%", "2.5%", "5%", "10%", "20%", "30%", "40%", "50%", "60%", "70%", "80%", "90%", "100%"};
    private int idxFx = 5;
    private int idxMusic = 5;

    private readonly float timeFade = 0.5f;
    private readonly float timeStay = 1f;

    // Start is called before the first frame update
    void Start()
    {
        imgFx.SetAlpha(0f);
        textFx.SetAlpha(0f);
        imgMusic.SetAlpha(0f);
        textMusic.SetAlpha(0f);
        AudioManager.SetVolume(AudioManager.Type.FX, percs[idxFx]);
        AudioManager.SetVolume(AudioManager.Type.MUSIC, percs[idxMusic]);
        UpdateTexts(false);
    }

    // Update is called once per frame
    void Update()
    {
        InputUpdate();
        FadeUpdate();
    }

    /// <summary>
    /// Update function for Player input
    /// </summary>
    void InputUpdate()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            AdjustMusic(1);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            AdjustMusic(-1);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            AdjustFx(1);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            AdjustFx(-1);
        }
    }

    /// <summary>
    /// Update function for fading the overlay
    /// </summary>
    void FadeUpdate()
    {
        if(Lerp.Apply(lerpFade, null))
        {
            if(state == State.FADEIN)
            {
                lerpFade = Lerp.Get(timeStay, 0f, 1f);
                state = State.STAY;
            }
            else if(state == State.STAY)
            {
                lerpFade = Lerp.Get(timeFade, 0f, 1f);
                FadeGraphics(2f, 0f);
                state = State.FADEOUT;
            }
        }
    }

    /// <summary>
    /// Adjusts the Fx volume and displays the change
    /// </summary>
    /// <param name="amount">Amount to adjust</param>
    void AdjustFx(int amount)
    {
        idxFx += amount;
        if (idxFx > percs.Length - 1) idxFx = percs.Length - 1;
        else if (idxFx < 0) idxFx = 0;
        AudioManager.SetVolume(AudioManager.Type.FX, percs[idxFx]);
        UpdateTexts(true);
    }

    /// <summary>
    /// Adjusts the music volume and displays the change
    /// </summary>
    /// <param name="amount">Amount to adjust</param>
    void AdjustMusic(int amount)
    {
        idxMusic += amount;
        if (idxMusic > percs.Length - 1) idxMusic = percs.Length - 1;
        else if (idxMusic < 0) idxMusic = 0;
        AudioManager.SetVolume(AudioManager.Type.MUSIC, percs[idxMusic]);
        UpdateTexts(true);
    }

    /// <summary>
    /// Updates volume UI to new values
    /// </summary>
    /// <param name="show">True to display the change, else false</param>
    void UpdateTexts(bool show)
    {
        textFx.SetText(sPercs[idxFx]);
        textMusic.SetText(sPercs[idxMusic]);

        if (show)
        {
            state = State.FADEIN;
            lerpFade = Lerp.Get(timeFade, 0f, 1f);
            FadeGraphics(timeFade, 1f);
            fx.Play("blip");
        }
    }

    /// <summary>
    /// Fades music and fx UI to an alpha value
    /// </summary>
    /// <param name="time">Fade time</param>
    /// <param name="alpha">Alpha value</param>
    void FadeGraphics(float time, float alpha)
    {
        imgFx.FadeAlpha(time, alpha);
        textFx.FadeAlpha(time, alpha);
        imgMusic.FadeAlpha(time, alpha);
        textMusic.FadeAlpha(time, alpha);
    }
}
