using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame_Walking_Button : MonoBehaviour
{
    public enum Type { FAST, SLOW }
    public Type type;
    public Minigame_Walking_Character character;

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
        if (type == Type.SLOW) character.SetSlow();
        else if (type == Type.FAST) character.SetFast();
    }
}
