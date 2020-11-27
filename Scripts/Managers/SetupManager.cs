using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupManager : MonoBehaviour
{
    public Menu menuInit;
    public MenuException menuException;
    public RectTransform panelMain;

    [Space]
    [Header("MANAGERS")]
    public LevelManager mgLevel;

    private void Awake()
    {
        ExceptionHandler.SetupExceptionHandling(menuException);

        DataObject.save = DataObject.New();
    }

    // Start is called before the first frame update
    void Start()
    {
        DisableAllMenus();
        menuInit.Show();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Disables all menues in the main panel
    /// </summary>
    void DisableAllMenus()
    {
        var menus = panelMain.GetComponentsInChildren<Menu>();
        foreach(Menu m in menus)
        {
            m.gameObject.SetActive(false);
        }
    }
}
