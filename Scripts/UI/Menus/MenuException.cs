using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuException : Menu
{
    public Text textTitle;
    public Text textText;

    public override void OnDisabled()
    {
        
    }

    public override void OnEnabled()
    {
        GameTime.SetMultiplier(0f); // Pause the game
    }

    public override void OnUpdate()
    {
        
    }

    /// <summary>
    /// Set the title and description text
    /// </summary>
    /// <param name="title">The title text</param>
    /// <param name="text">The description text</param>
    public void SetText(string title, string text)
    {
        textTitle.text = title;
        textText.text = text;
    }

    /// <summary>
    /// Quits the game
    /// </summary>
    public void ClickQuit()
    {
        Application.Quit();
    }
}
