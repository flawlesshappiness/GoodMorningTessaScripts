using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame_Crosswalk_Character : MonoBehaviour
{
    [Space]
    [Header("COMPONENTS")]
    public Minigame_Crosswalk game;
    public AnimatorController tessa;
    public SpriteOrderController order;

    [Space]
    [Header("PROPERTIES")]
    public float distMoveVertical;
    public float distMoveHorizontal;
    public float radius;

    // Start is called before the first frame update
    void Start()
    {
        tessa.SetAnimation("tessa_idle_up");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red.SetAlpha(0.3f);
        Gizmos.DrawSphere(transform.position, radius);
    }

    /// <summary>
    /// Move the player character by a movement type
    /// </summary>
    /// <param name="type">The movement type</param>
    public void Move(Minigame_Crosswalk.MovementType type)
    {
        if (!game.IsSetup()) return;

        order.UpdateOrder();

        var hits = GetRaycastHits();
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider == null) continue;
            if (hit.collider.tag == "wall_left" && type == Minigame_Crosswalk.MovementType.LEFT) return;
            else if (hit.collider.tag == "wall_right" && type == Minigame_Crosswalk.MovementType.RIGHT) return;
            else if (hit.collider.tag == "wall_up" && type == Minigame_Crosswalk.MovementType.UP) return;
            else if(hit.collider.tag == "win") game.Win();
            else if (hit.collider.tag == "lose")
            {
                game.LoseTrail();
                return;
            }
        }

        if (type == Minigame_Crosswalk.MovementType.UP)
        {
            Move(new Vector3(0f, distMoveVertical));
            tessa.SetAnimation("tessa_idle_up");
        }
        else if (type == Minigame_Crosswalk.MovementType.DOWN)
        {
            Move(new Vector3(0f, -distMoveVertical));
            tessa.SetAnimation("tessa_idle_down");
        }
        else if (type == Minigame_Crosswalk.MovementType.LEFT)
        {
            Move(new Vector3(-distMoveHorizontal, 0f));
            tessa.SetAnimation("tessa_idle_left");
        }
        else if (type == Minigame_Crosswalk.MovementType.RIGHT)
        {
            Move(new Vector3(distMoveHorizontal, 0f));
            tessa.SetAnimation("tessa_idle_right");
        }
    }

    /// <summary>
    /// Adjust the position of the player and play a sound
    /// </summary>
    /// <param name="v">The position adjustment</param>
    void Move(Vector3 v)
    {
        game.level.manager.fx.Play("blip");
        transform.position += v;
    }

    /// <summary>
    /// Gets all raycast hits around the player
    /// </summary>
    /// <returns>The array of raycast hits</returns>
    RaycastHit2D[] GetRaycastHits()
    {
        return Physics2D.CircleCastAll(transform.position, radius, Vector2.zero);
    }
}
