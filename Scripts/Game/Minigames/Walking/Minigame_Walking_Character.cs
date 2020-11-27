using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame_Walking_Character : MonoBehaviour
{
    public Minigame_Walking game;
    public AnimatorController tessa;
    public Transform start;
    public Collider2D colEnd;

    [Space]
    private float speed;
    public float speedSlow;
    public float speedFast;
    private bool moving;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveUpdate();
    }

    /// <summary>
    /// Starts the player animation and sets move speed to slow
    /// </summary>
    public void StartMoving()
    {
        moving = true;
        tessa.SetAnimation("tessa_walk_right");
        SetSlow();
    }

    /// <summary>
    /// The move update function
    /// </summary>
    void MoveUpdate()
    {
        if (!moving) return;
        var move = Vector3.right * speed * GameTime.deltaTime;
        transform.position += move;
    }

    /// <summary>
    /// Resets the position of the player
    /// </summary>
    public void ResetPosition()
    {
        transform.position = start.position;
    }

    /// <summary>
    /// Sets the move speed to slow
    /// </summary>
    public void SetSlow()
    {
        speed = speedSlow;
    }

    /// <summary>
    /// Sets the move speed to fast
    /// </summary>
    public void SetFast()
    {
        speed = speedFast;
    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        if(c == colEnd)
        {
            game.NextScene();
        }
    }
}
