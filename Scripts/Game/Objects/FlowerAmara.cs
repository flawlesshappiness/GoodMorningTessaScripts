using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerAmara : MonoBehaviour
{
    public Level level;
    public Transform flower;
    public ParticleSystem ps;
    public Transform flowerEnd;

    private int clicksLeft = 3;
    private bool activated = false;

    private Lerp<Vector3> lerpFlower;

    // Start is called before the first frame update
    void Start()
    {
        ps.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Lerp.Apply(lerpFlower, SetFlowerPos))
        {
            lerpFlower = null;
            flower.gameObject.SetActive(false);
        }
    }

    private void OnDrawGizmos()
    {
        if (flowerEnd == null) return;
        Gizmos.color = new Color(1f, 0.5f, 1f, 0.5f);
        Gizmos.DrawSphere(flowerEnd.position, 0.5f);
        Gizmos.DrawLine(flower.position, flowerEnd.position);
    }

    private void OnMouseDown()
    {
        if (activated) return;
        clicksLeft--;
        if(clicksLeft <= 0)
        {
            Amara();
        }
    }

    /// <summary>
    /// Adds the Amara level to the minigame list
    /// </summary>
    void Amara()
    {
        level.manager.fx.Play("flower_glow");
        lerpFlower = Lerp.Get(1f, flower.position, flowerEnd.position);
        ps.gameObject.SetActive(true);
        level.manager.AddAmaraLevel();
        activated = true;
    }

    /// <summary>
    /// Sets the position of this object
    /// </summary>
    /// <param name="v">Position</param>
    void SetFlowerPos(Vector3 v)
    {
        flower.position = v;
    }
}
