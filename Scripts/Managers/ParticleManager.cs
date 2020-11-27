using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public ParticleMap[] particles;

    private bool setup = false;
    private Dictionary<string, ParticleSystem> dic = new Dictionary<string, ParticleSystem>();

    [System.Serializable]
    public class ParticleMap
    {
        public string name;
        public ParticleSystem ps;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Sets up the particle manager
    /// </summary>
    public void Setup()
    {
        foreach(ParticleMap map in particles)
        {
            dic.Add(map.name, map.ps);
        }

        setup = true;
    }

    /// <summary>
    /// Instantiates a particle system from a dictionary, with a name key
    /// </summary>
    /// <param name="name">The name key</param>
    /// <returns>The particle system</returns>
    public ParticleSystem Instantiate(string name)
    {
        if (!setup) Setup();
        return Instantiate(dic[name]);
    }
}
