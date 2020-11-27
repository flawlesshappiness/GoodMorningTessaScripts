using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioController : MonoBehaviour
{
    private enum FadeState { IN, OUT }
    private FadeState stateFade;

    public AudioManager.Type type;

    [Space]
    [Range(0f, 1f)]
    public float volume;

    [Space]
    public AudioSource source;
    public AudioClip[] clips;

    private Lerp<float> lerpEnd;
    private Lerp<float> lerpVolume;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayUpdate();
    }

    #region PLAY
    /// <summary>
    /// Update function for when the audio is playing
    /// </summary>
    void PlayUpdate()
    {
        if(Lerp.Apply(lerpEnd, null))
        {
            Stop();
            lerpEnd = null;
        }

        if (Lerp.Apply(lerpVolume, SetVolumeMultiplier))
        {
            if (stateFade == FadeState.OUT) Stop();
            lerpVolume = null;
        }
    }

    /// <summary>
    /// Finds a random clip, updates the volume and plays the audio
    /// </summary>
    public void Play()
    {
        UpdateVolumeType();
        source.clip = GetRandomClip();
        source.Play();
        if(!source.loop) lerpEnd = Lerp.Get(source.clip.length, 0f, 1f);
        AudioManager.OnAudioPlay(this);
    }

    /// <summary>
    /// Plays the audio, and fades in the volume
    /// </summary>
    /// <param name="fadeIn"></param>
    public void Play(float fadeIn)
    {
        Play();

        float volumeCur = 0f;
        if (lerpVolume != null) volumeCur = lerpVolume.GetLerp();
        else if (source.isPlaying) volumeCur = 1f;
        lerpVolume = Lerp.Get(fadeIn, volumeCur, 1f);

        stateFade = FadeState.IN;
    }

    /// <summary>
    /// Stops the audio
    /// </summary>
    public void Stop()
    {
        source.Stop();
        AudioManager.OnAudioStop(this);
    }

    /// <summary>
    /// Fades out and stops the audio
    /// </summary>
    /// <param name="fadeOut"></param>
    public void Stop(float fadeOut)
    {
        float volumeCur = 0f;
        if (lerpVolume != null) volumeCur = lerpVolume.GetLerp();
        else if (source.isPlaying) volumeCur = 1f;
        lerpVolume = Lerp.Get(fadeOut, volumeCur, 0f);

        stateFade = FadeState.OUT;
    }
    #endregion
    #region VOLUME
    /// <summary>
    /// Updates the volume, if not currently fading
    /// </summary>
    public void UpdateVolumeType()
    {
        if(lerpVolume == null) SetVolumeMultiplier(1f);
    }

    /// <summary>
    /// Sets the volume multiplier
    /// </summary>
    /// <param name="mult">The new multiplier</param>
    void SetVolumeMultiplier(float mult)
    {
        if (source == null) return;
        source.volume = volume * AudioManager.GetVolume(type) * AudioManager.GetVolume(AudioManager.Type.MASTER) * mult;
    }
    #endregion

    /// <summary>
    /// Returns a random audio clip
    /// </summary>
    /// <returns>The audio clip</returns>
    AudioClip GetRandomClip()
    {
        return clips[Random.Range(0, clips.Length)];
    }
}
