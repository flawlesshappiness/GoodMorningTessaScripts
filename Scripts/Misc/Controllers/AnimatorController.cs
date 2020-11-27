using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    public Animator anim;
    public SpriteRenderer ren;
    [Space]
    public string initAnim;
    [Space]
    public Anim[] anims;

    [System.Serializable]
    public class Anim
    {
        public string name;
        public string animation;
        public float speed = 1.0f;
        public bool flipX;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (initAnim != null && initAnim != "")
        {
            SetAnimation(initAnim);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Play animation with a specific name
    /// </summary>
    /// <param name="name">Name of animation</param>
    public void SetAnimation(string name)
    {
        var a = GetAnim(name);
        anim.Play(a.animation);
        anim.speed = a.speed;
        ren.flipX = a.flipX;
    }

    /// <summary>
    /// Returns animation with a specific name
    /// </summary>
    /// <param name="name">Name of animation</param>
    /// <returns>The animation</returns>
    Anim GetAnim(string name)
    {
        foreach (Anim a in anims)
        {
            if (a.name == name) return a;
        }

        return null;
    }
}
