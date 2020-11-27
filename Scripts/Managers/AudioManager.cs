using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Range(0f, 1f)]
    public float _volumeMaster;
    [Range(0f, 1f)]
    public float _volumeFx;
    [Range(0f, 1f)]
    public float _volumeMusic;

    public enum Type { MASTER, FX, MUSIC }
    public static Dictionary<Type, float> dicVolumes = new Dictionary<Type, float>
    {
        { Type.MASTER, 1f },
        { Type.FX, 1f },
        { Type.MUSIC, 1f },
    };

    private static Dictionary<Type, List<AudioController>> dicAudio = new Dictionary<Type, List<AudioController>>()
    {
        { Type.MASTER, new List<AudioController>() },
        { Type.FX, new List<AudioController>() },
        { Type.MUSIC, new List<AudioController>() },
    };

    // Awake
    private void Awake()
    {
        dicVolumes[Type.MASTER] = _volumeMaster;
        dicVolumes[Type.FX] = _volumeFx;
        dicVolumes[Type.MUSIC] = _volumeMusic;
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
    /// Sets the volume of an audio type
    /// </summary>
    /// <param name="type">The audio type</param>
    /// <param name="volume">The volume</param>
    public static void SetVolume(Type type, float volume)
    {
        dicVolumes[type] = volume;
        if(type == Type.MASTER)
        {
            UpdateAudioVolumes(Type.FX);
            UpdateAudioVolumes(Type.MUSIC);
        }
        else
        {
            UpdateAudioVolumes(type);
        }
    }

    /// <summary>
    /// Updates volume of all audio controllers of a given type
    /// </summary>
    /// <param name="type">The audio type</param>
    static void UpdateAudioVolumes(Type type)
    {
        var list = dicAudio[type];
        foreach (AudioController audio in list)
            audio.UpdateVolumeType();
    }

    /// <summary>
    /// Returns the volume of an audio type
    /// </summary>
    /// <param name="type">The audio type</param>
    /// <returns>The volume</returns>
    public static float GetVolume(Type type)
    {
        return dicVolumes[type];
    }

    /// <summary>
    /// Called when an AudioController is played
    /// </summary>
    /// <param name="audio">The AudioController</param>
    public static void OnAudioPlay(AudioController audio)
    {
        var list = dicAudio[audio.type];
        if (!list.Contains(audio))
            list.Add(audio);
    }

    /// <summary>
    /// Called when an AudioController stops playing
    /// </summary>
    /// <param name="audio">The AudioController</param>
    public static void OnAudioStop(AudioController audio)
    {
        var list = dicAudio[audio.type];
        if (list.Contains(audio))
            list.Remove(audio);
    }
}
