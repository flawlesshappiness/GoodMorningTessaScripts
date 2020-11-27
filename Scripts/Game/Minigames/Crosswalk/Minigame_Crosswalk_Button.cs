using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame_Crosswalk_Button : MonoBehaviour
{
    public Level level;
    public Minigame_Crosswalk.MovementType type;
    public Minigame_Crosswalk_Character character;

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
        character.Move(type);
    }
}
