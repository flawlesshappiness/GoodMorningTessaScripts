using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioMusic[] musics;
    private AudioMusic musicCur;

    [System.Serializable]
    public class AudioMusic
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
    /// Stops the current music, and plays music from name
    /// </summary>
    /// <param name="name">Name of music to play</param>
    public void Play(string name)
    {
        var music = GetMusic(name);
        if (music == null) return;
        if (musicCur != null) musicCur.audio.Stop();
        music.audio.Play();
        musicCur = music;
    }

    /// <summary>
    /// Fades out the current music, and fades in the next music from name
    /// </summary>
    /// <param name="name">Name of music to fade in</param>
    /// <param name="fadeIn">Fade time</param>
    public void Play(string name, float fadeIn)
    {
        var music = GetMusic(name);
        if (music == null) return;
        music.audio.Play(fadeIn);

        if (musicCur != null) musicCur.audio.Stop(fadeIn);
        musicCur = music;
    }

    /// <summary>
    /// Stops the current music
    /// </summary>
    public void Stop()
    {
        if (musicCur == null) return;
        musicCur.audio.Stop();
        musicCur = null;
    }

    /// <summary>
    /// Fades out the current music
    /// </summary>
    /// <param name="fadeOut">Fade time</param>
    public void Stop(float fadeOut)
    {
        if (musicCur == null) return;
        musicCur.audio.Stop(fadeOut);
        musicCur = null;
    }

    /// <summary>
    /// Gets an AudioMusic object from name
    /// </summary>
    /// <param name="name">Music name</param>
    /// <returns>The AudioMusic</returns>
    AudioMusic GetMusic(string name)
    {
        foreach (AudioMusic music in musics)
            if (music.name == name)
                return music;

        return null;
    }
}
