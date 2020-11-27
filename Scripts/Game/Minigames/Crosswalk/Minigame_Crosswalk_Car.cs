using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame_Crosswalk_Car : MonoBehaviour
{
    public Minigame_Crosswalk game;
    public Transform start;
    public Transform end;
    public SpriteRenderer ren;
    public SpriteOrderController order;
    public AudioController audioDrive;
    public Collider2D target;
    public Sprite[] sprites;

    private bool driving;
    private Lerp<Vector3> lerpPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Lerp.Apply(lerpPos, SetPos))
        {
            lerpPos = null;
        }
    }

    private void OnDrawGizmos()
    {
        if (start == null || end == null) return;
        Gizmos.color = Color.green.SetAlpha(0.5f);
        Gizmos.DrawSphere(start.position, 0.5f);
        Gizmos.DrawSphere(end.position, 0.5f);
        Gizmos.DrawLine(start.position, end.position);
    }

    /// <summary>
    /// Starts lerping the car from start to end position, and playing sound
    /// </summary>
    public void StartDriving()
    {
        audioDrive.Play();
        SetRandomSprite();
        order.UpdateOrder();
        lerpPos = Lerp.Get(audioDrive.source.clip.length, start.position, end.position);
        driving = true;
    }

    /// <summary>
    /// Assigns a random sprite to the renderer
    /// </summary>
    void SetRandomSprite()
    {
        ren.sprite = sprites[Random.Range(0, sprites.Length)];
    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        if (!driving) return;
        if (c != target) return;

        audioDrive.Stop();
        game.Lose();
    }

    /// <summary>
    /// Sets the position of the object
    /// </summary>
    /// <param name="v">The position</param>
    void SetPos(Vector3 v)
    {
        transform.position = v;
    }
}
