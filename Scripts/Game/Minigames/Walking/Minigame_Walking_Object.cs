using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame_Walking_Object : MonoBehaviour
{
    public Minigame_Walking game;
    public Transform tObject;
    public Transform tShadow;
    public Transform start;
    public Collider2D target;

    private Lerp<Vector3> lerpPos;
    private Lerp<Vector3> lerpShadow;

    private float timeFallMin = 1f;
    private float timeFallMax = 3f;

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
            ResetObject();
            game.level.manager.fx.Play("break");
        }

        if(Lerp.Apply(lerpShadow, SetShadowSize))
        {
            lerpShadow = null;
        }
    }

    /// <summary>
    /// Starts the object fall animation
    /// </summary>
    public void StartFalling()
    {
        tObject.gameObject.SetActive(true);
        tShadow.gameObject.SetActive(true);

        float time = Random.Range(timeFallMin, timeFallMax);
        lerpPos = Lerp.Get(time, start.position, tShadow.position.AddY(1f));
        lerpShadow = Lerp.Get(time, Vector3.zero, new Vector3(26f, 26f));
    }

    /// <summary>
    /// Disables the object
    /// </summary>
    public void ResetObject()
    {
        tObject.gameObject.SetActive(false);
        tShadow.gameObject.SetActive(false);
    }

    /// <summary>
    /// Sets the object position
    /// </summary>
    /// <param name="v">The position</param>
    void SetPos(Vector3 v)
    {
        tObject.position = v;
    }

    /// <summary>
    /// Sets the size of the shadow
    /// </summary>
    /// <param name="v">The size</param>
    void SetShadowSize(Vector3 v)
    {
        tShadow.localScale = v;
    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        if(c == target)
        {
            ResetObject();
            game.OnHit();
        }
    }
}
