using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame_Outfit_Box : MonoBehaviour
{
    public Minigame_Outfit game;
    public bool isWrong;

    [Space]
    public SpriteRenderer renContent;
    public SpriteRenderer renBox;

    [Space]
    public Sprite spOpen;
    public Sprite spClosed;

    [Space]
    public Transform tContentUp;
    public Transform tContentDown;

    private Lerp<float> lerpContentAlpha;
    private Lerp<Vector3> lerpContentPos;
    private bool closing;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Lerp.Apply(lerpContentAlpha, SetContentAlpha))
        {
            lerpContentAlpha = null;
        }

        if(Lerp.Apply(lerpContentPos, SetContentPos))
        {
            lerpContentPos = null;
            if(closing) renBox.sprite = spClosed;
        }
    }

    private void OnMouseDown()
    {
        if (!game.CanPickBox()) return;
        game.PickBox(this);
    }

    /// <summary>
    /// Animates opening the box
    /// </summary>
    /// <param name="time">Animation time</param>
    public void Open(float time)
    {
        closing = false;
        lerpContentAlpha = Lerp.Get(time, 0f, 1f);
        lerpContentPos = Lerp.Get(time, tContentDown.position, tContentUp.position);
        renBox.sprite = spOpen;
    }

    /// <summary>
    /// Animates closing the box
    /// </summary>
    /// <param name="time">Animation time</param>
    public void Close(float time)
    {
        closing = true;
        lerpContentAlpha = Lerp.Get(time, 1f, 0f);
        lerpContentPos = Lerp.Get(time, tContentUp.position, tContentDown.position);
    }

    /// <summary>
    /// Instantly opens or closes the box
    /// </summary>
    /// <param name="closed">Closed</param>
    public void SetClosed(bool closed)
    {
        renContent.color = renContent.color.SetAlpha(closed ? 0f : 1f);
        renContent.transform.position = closed ? tContentDown.position : tContentUp.position;
        renBox.sprite = closed ? spClosed : spOpen;
    }

    /// <summary>
    /// Set the alpha value of the content renderer
    /// </summary>
    /// <param name="a">The alpha value</param>
    void SetContentAlpha(float a)
    {
        renContent.color = renContent.color.SetAlpha(a);
    }

    /// <summary>
    /// Set the position of the content
    /// </summary>
    /// <param name="v">The position</param>
    void SetContentPos(Vector3 v)
    {
        renContent.transform.position = v;
    }
}
