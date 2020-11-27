using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame_Parking_Space : MonoBehaviour
{
    public GameObject main;
    public GameObject legal;
    public GameObject illegal;

    [Space]
    public SpriteController spWrong;

    private Minigame_Parking game;
    private bool isLegal;

    private bool clicked;

    private bool setup = false;

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
        OnClick();
    }

    /// <summary>
    /// Checks the parking space when clicked
    /// </summary>
    public void OnClick()
    {
        if (!setup) return;
        if (clicked) return;
        clicked = true;
        game.ClickSpace(this);
    }

    /// <summary>
    /// Sets up the parking space
    /// </summary>
    /// <param name="game"></param>
    public void Setup(Minigame_Parking game)
    {
        this.game = game;
        SetEnabled(false);
        spWrong.SetAlpha(0f);
        clicked = false;
        setup = true;
    }

    /// <summary>
    /// Sets the space enabled/disabled
    /// </summary>
    /// <param name="enabled">Enabled</param>
    void SetEnabled(bool enabled)
    {
        main.SetActive(enabled);
    }

    /// <summary>
    /// Sets the space enabled, and illegal
    /// </summary>
    public void SetIllegal()
    {
        isLegal = false;
        legal.SetActive(isLegal);
        illegal.SetActive(!isLegal);
        SetEnabled(true);
    }

    /// <summary>
    /// Sets the space enabled, and legal
    /// </summary>
    public void SetLegal()
    {
        isLegal = true;
        legal.SetActive(isLegal);
        illegal.SetActive(!isLegal);
        SetEnabled(true);
    }

    /// <summary>
    /// Returns true if legal, else false
    /// </summary>
    /// <returns>If legal</returns>
    public bool IsLegal()
    {
        return isLegal;
    }

    /// <summary>
    /// Diplays the 'wrong' graphic
    /// </summary>
    public void SetWrong()
    {
        spWrong.SetAlpha(1f);
    }

    /// <summary>
    /// Disables the parking space
    /// </summary>
    public void SetCorrect()
    {
        SetEnabled(false);
        //Show correct
    }
}
