using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame_SpaceFlower_Spot : MonoBehaviour
{
    public Minigame_SpaceFlower game;

    public Transform bottom;
    public Transform top;
    public Transform star;
    public SpriteController spBeam;

    private readonly float alphaActive = 0.5f;
    private readonly float alphaInactive = 0.1f;

    private Lerp<Vector3> lerpStar;

    private int idx;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Lerp.Apply(lerpStar, SetStarPosition))
        {
            lerpStar = null;
            SetActive(false);
            var ps = game.level.manager.particles.Instantiate("impact_star");
            ps.transform.position = star.position;
            star.position = top.position;
            game.Hit(idx);
        }
    }

    /// <summary>
    /// Sets the index of the soit
    /// </summary>
    /// <param name="idx">The index</param>
    public void SetIndex(int idx)
    {
        this.idx = idx;
    }

    /// <summary>
    /// Sets the spot active/inactive
    /// </summary>
    /// <param name="active">Active</param>
    public void SetActive(bool active)
    {
        float a = active ? alphaActive : alphaInactive;
        spBeam.FadeAlpha(1f, a);
    }

    /// <summary>
    /// Hides the beam
    /// </summary>
    public void SetInactive()
    {
        spBeam.SetAlpha(alphaInactive);
    }

    /// <summary>
    /// Starts lerping the falling star position
    /// </summary>
    /// <param name="time">Lerp time</param>
    public void FallStar(float time)
    {
        lerpStar = Lerp.Get(time, top.position, bottom.position);
    }

    /// <summary>
    /// Sets the position of the falling star
    /// </summary>
    /// <param name="v">The position</param>
    void SetStarPosition(Vector3 v)
    {
        star.position = v;
    }
}
