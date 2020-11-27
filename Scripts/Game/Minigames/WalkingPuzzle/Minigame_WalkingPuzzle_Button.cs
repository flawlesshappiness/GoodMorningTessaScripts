using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame_WalkingPuzzle_Button : MonoBehaviour
{
    public int incAmount;
    public Minigame_WalkingPuzzle game;

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
        game.IncrementPositionIndex(incAmount);
    }
}
