using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCredits : Menu
{
    public LevelManager manager;

    private RectTransform panelCur;

    public override void OnDisabled()
    {
        manager.music.Stop();
    }

    public override void OnEnabled()
    {
        
    }

    public override void OnUpdate()
    {
        
    }

    /// <summary>
    /// Sets current panel inactive, and new panel active
    /// </summary>
    /// <param name="panel">The new panel</param>
    public void SetPanel(RectTransform panel)
    {
        if (panelCur != null) panelCur.gameObject.SetActive(false);
        panelCur = panel;
        panelCur.gameObject.SetActive(true);
    }

    /// <summary>
    /// Hides this menu and shows the main menu
    /// </summary>
    public void Back()
    {
        manager.menuMain.Show();
    }
}
