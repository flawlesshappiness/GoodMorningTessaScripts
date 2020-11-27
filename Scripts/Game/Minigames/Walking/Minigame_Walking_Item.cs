using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame_Walking_Item : MonoBehaviour
{
    public enum Type { MARY, P3 }
    public Type type;

    public Minigame_Walking game;

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
        if(type == Type.MARY) game.CollectMary(this);
        else if(type == Type.P3) game.CollectP3(this);
    }
}
