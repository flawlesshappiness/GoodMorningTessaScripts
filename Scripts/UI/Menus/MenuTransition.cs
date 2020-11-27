using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuTransition : Menu
{
    public Text text1;
    public Text text2;
    public Text text3;
    public RectTransform panelMary;
    public RectTransform panelP3;

    public override void OnDisabled()
    {
        
    }

    public override void OnEnabled()
    {
        
    }

    public override void OnUpdate()
    {
        
    }

    /// <summary>
    /// Clears all texts
    /// </summary>
    public void ClearTexts()
    {
        SetText1("");
        SetText2("");
        SetText3("");
    }

    /// <summary>
    /// Sets the text of upper most text UI
    /// </summary>
    /// <param name="text">The text to set</param>
    public void SetText1(string text)
    {
        this.text1.text = text;
    }

    /// <summary>
    /// Sets the text of the middle text UI
    /// </summary>
    /// <param name="text">The text to set</param>
    public void SetText2(string text)
    {
        this.text2.text = text;
    }

    /// <summary>
    /// Sets the text of the lower most text UI
    /// </summary>
    /// <param name="text">The text to set</param>
    public void SetText3(string text)
    {
        this.text3.text = text;
    }

    /// <summary>
    /// Sets mary background enabled
    /// </summary>
    /// <param name="enabled">Enabled</param>
    public void SetMaryEnabled(bool enabled)
    {
        panelMary.gameObject.SetActive(enabled);
    }

    /// <summary>
    /// Sets P3 background enabled
    /// </summary>
    /// <param name="enabled">enabled</param>
    public void SetP3Enabled(bool enabled)
    {
        panelP3.gameObject.SetActive(enabled);
    }
}
