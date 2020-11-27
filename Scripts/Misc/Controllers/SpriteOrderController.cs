using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteOrderController : MonoBehaviour
{
    public SpriteRenderer r;

    public bool x;
    public bool y;
    public bool z;

    public float mult;

    // Start is called before the first frame update
    void Start()
    {
        UpdateOrder();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Updates the sorting order of the renderer, based on its position
    /// </summary>
    public void UpdateOrder()
    {
        var p = transform.position;
        float _x = x ? p.x : 0f;
        float _y = y ? p.y : 0f;
        float _z = z ? p.z : 0f;
        int order = (int)((0f - _x - _y - _z) * mult);
        r.sortingOrder = order;
    }
}
