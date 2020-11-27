using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame_Crosswalk_Trail : MonoBehaviour
{
    public SpriteController[] sprites;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Fade out all sprites in the sprites array
    /// </summary>
    public void FadeOut()
    {
        foreach(SpriteController spc in sprites)
        {
            spc.FadeAlpha(3f, 0f);
        }
    }

    /// <summary>
    /// Fade in all the sprites in the sprites array
    /// </summary>
    public void FadeIn()
    {
        foreach (SpriteController spc in sprites)
        {
            spc.FadeAlpha(3f, 1f);
        }
    }
}
