using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class TextLink : MonoBehaviour
{
    public string link;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Opens a new window with the link
    /// </summary>
    public void ClickLink()
    {
        OpenWindow(link);
    }

    [DllImport("__Internal")]
    private static extern void OpenWindow(string url);
}
