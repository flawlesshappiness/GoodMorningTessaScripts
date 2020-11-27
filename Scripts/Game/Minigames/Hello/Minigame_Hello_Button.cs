using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame_Hello_Button : MonoBehaviour
{
    public Minigame_Hello game;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        game.SayHello();
    }
}
