using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame_SpaceFlower_Glow : MonoBehaviour
{
    public ParticleSystem ps_petals;
    public Transform flower;
    public SpriteController spGlow;

    private Lerp<Vector3> lerpFlower;

    // Start is called before the first frame update
    void Start()
    {
        spGlow.SetAlpha(0f);
    }

    // Update is called once per frame
    void Update()
    {
        if(Lerp.Apply(lerpFlower, SetFlowerPosition))
        {
            lerpFlower = null;
            SetGlowing(true);
            spGlow.FadeAlpha(1f, 1f, 0f);
        }
    }

    /// <summary>
    /// Starts lerping the position of the flower
    /// </summary>
    /// <param name="time">Lerp time</param>
    public void StartFlower(float time)
    {
        lerpFlower = Lerp.Get(time, flower.position, transform.position, Lerp.Type.EXPONENTIAL);
    }

    /// <summary>
    /// Sets the petal particle system active/inactive
    /// </summary>
    /// <param name="active"></param>
    public void SetGlowing(bool active)
    {
        ps_petals.gameObject.SetActive(active);
    }

    /// <summary>
    /// Sets the flower position
    /// </summary>
    /// <param name="v">Position</param>
    void SetFlowerPosition(Vector3 v)
    {
        flower.position = v;
    }
}
