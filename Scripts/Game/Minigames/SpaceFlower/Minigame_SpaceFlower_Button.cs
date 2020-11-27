using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame_SpaceFlower_Button : MonoBehaviour
{
    public Minigame_SpaceFlower game;
    public int idxAdjust;

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
        game.AdjustFlowerPosition(idxAdjust);
    }
}
