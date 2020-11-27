using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMain : Menu
{
    public MenuCredits menuCredits;
    public LevelManager mgLevel;
    public GameObject gInstruction;

    private bool instructionActive;
    private float cdInstruction;
    private readonly float cdbInstruction = 1f;

    public override void OnDisabled()
    {
        mgLevel.music.Stop(); // Stop playing music
    }

    public override void OnEnabled()
    {
        mgLevel.PlayMajorMusic(); // Start playing music
    }

    public override void OnUpdate()
    {
        // Input for starting the game
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartGame();
        }

        // Lerp timer for toggling the instruction
        if(Time.time > cdInstruction)
        {
            instructionActive = !instructionActive;
            cdInstruction = Time.time + cdbInstruction;
            gInstruction.SetActive(instructionActive);
        }
    }

    /// <summary>
    /// Starts the first level
    /// </summary>
    void StartGame()
    {
        mgLevel.StartFirstDay();
    }

    /// <summary>
    /// Shows the credit menu
    /// </summary>
    public void Credits()
    {
        menuCredits.Show();
    }
}
