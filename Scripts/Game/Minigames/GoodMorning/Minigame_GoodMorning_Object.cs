using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame_GoodMorning_Object : MonoBehaviour
{
    public GameObject normal;
    public GameObject hospital;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Sets the normal version of the object active, and disables the hospital version
    /// </summary>
    public void SetNormal()
    {
        normal.SetActive(true);
        hospital.SetActive(false);
    }

    /// <summary>
    /// Sets the hospital version of the object active, and disables the normal version
    /// </summary>
    public void SetHospital()
    {
        normal.SetActive(false);
        hospital.SetActive(true);
    }
}
