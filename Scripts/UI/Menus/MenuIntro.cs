using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuIntro : Menu
{
    public FxManager fx;
    public MenuMain menuMain;
    public RectTransform[] panels;
    private int idx;

    public override void OnDisabled()
    {
        
    }

    public override void OnEnabled()
    {
        foreach (RectTransform t in panels)
            t.gameObject.SetActive(false);

        idx = 0;
        panels[idx].gameObject.SetActive(true);
    }

    public override void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NextPanel();
        }
    }

    /// <summary>
    /// Displays the next panel from the panel array
    /// </summary>
    void NextPanel()
    {
        fx.Play("points_normal");
        panels[idx].gameObject.SetActive(false);
        idx++;
        if (idx >= panels.Length) menuMain.Show();
        else panels[idx].gameObject.SetActive(true);
    }
}
