using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Menu : MonoBehaviour {

	public static Menu menuCur;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		OnUpdate();
	}

    /// <summary>
    /// Called every time Update is called in the Menu
    /// </summary>
	public abstract void OnUpdate();

    /// <summary>
    /// Called after menu is enabled
    /// </summary>
	public abstract void OnEnabled();

    /// <summary>
    /// Called after menu is disabled
    /// </summary>
	public abstract void OnDisabled();

    /// <summary>
    /// Hide current menu, and show this menu
    /// </summary>
	public void Show()
	{
		if(menuCur != null)
		{
			menuCur.gameObject.SetActive(false); //Disable previous menu
			menuCur.OnDisabled(); //Call OnDisabled on previous menu
		}
		menuCur = this; //Set new menu to this menu
		menuCur.gameObject.SetActive(true); //Enable new menu
        menuCur.OnEnabled(); //Call OnEnabled on  new menu
    }

    /// <summary>
    /// Hides all menus parented to a panel
    /// </summary>
    /// <param name="mainPanel">The panel</param>
    public static void DisableAllMenus(Transform mainPanel)
    {
        foreach(Menu m in mainPanel.GetComponentsInChildren<Menu>())
        {
            m.gameObject.SetActive(false);
        }
    }
}
