using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxManager : MonoBehaviour
{
    public AudioFx[] fxs;
    private Dictionary<string, AudioController> dicFx = new Dictionary<string, AudioController>();
    private List<AudioController> adjustments = new List<AudioController>();

    private bool setup;

    [System.Serializable]
    public class AudioFx
    {
        public string name;
        public AudioController audio;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Setse up the Fx manager
    /// </summary>
    void Setup()
    {
        foreach(AudioFx fx in fxs)
        {
            dicFx.Add(fx.name, fx.audio);
        }

        setup = true;
    }

    /// <summary>
    /// Plays an fx from the dictionary, by a name key
    /// </summary>
    /// <param name="name">The name key</param>
    public void Play(string name)
    {
        GetFx(name).Play();
    }

    /// <summary>
    /// Gets an fx from a name key
    /// </summary>
    /// <param name="name">The name key</param>
    /// <returns>The fx AudioController</returns>
    public AudioController GetFx(string name)
    {
        if (!setup) Setup();
        return dicFx[name];
    }

    #region PITCH
    /// <summary>
    /// Adjusts the pitch of an fx
    /// </summary>
    /// <param name="name">The fx name key</param>
    /// <param name="amount">The amount to adjust pitch</param>
    public void AdjustPitch(string name, float amount)
    {
        var audio = GetFx(name);
        audio.source.pitch += amount;
        AddAdjustment(audio);
    }

    /// <summary>
    /// Sets the pitch of an fx
    /// </summary>
    /// <param name="name">The fx name key</param>
    /// <param name="pitch">The amount to adjust pitch</param>
    public void SetPitch(string name, float pitch)
    {
        var audio = GetFx(name);
        audio.source.pitch = pitch;
        AddAdjustment(audio);
    }
    #endregion
    #region ADJUSTMENTS
    /// <summary>
    /// Logs that an AudioController has been adjusted
    /// </summary>
    /// <param name="audio">The AudioController</param>
    void AddAdjustment(AudioController audio)
    {
        if (!adjustments.Contains(audio)) adjustments.Add(audio);
    }

    /// <summary>
    /// Resets all adjustments to all adjusted AudioControllers
    /// </summary>
    public void ResetAdjustment()
    {
        foreach(AudioController audio in adjustments)
        {
            var source = audio.source;
            source.pitch = 1f;
        }

        adjustments.Clear();
    }
    #endregion
}
