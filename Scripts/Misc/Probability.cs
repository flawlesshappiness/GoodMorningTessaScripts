using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Class to pick a random value from values with different weights
/// </summary>
/// <typeparam name="T">The type of the values</typeparam>
public class Probability<T>
{
    private float max;
    private Dictionary<T, float> dicEntries;

    public Probability()
    {
        max = 0f;
        dicEntries = new Dictionary<T, float>();
    }

    public Probability(Dictionary<T, float> dic)
    {
        dicEntries = dic;

        max = 0f;
        foreach (KeyValuePair<T, float> kvp in dic)
            max += kvp.Value;
    }

    /// <summary>
    /// Add value with a weight to the collection
    /// </summary>
    /// <param name="value">The value to add</param>
    /// <param name="weight">The weight of the value</param>
    public void Add(T value, float weight)
    {
        dicEntries.Add(value, weight);
        max += weight;
    }

    /// <summary>
    /// Get semi-random value, from collection of weighted values
    /// </summary>
    /// <returns>Semi-random value</returns>
    public T Get()
    {
        float v = Random.Range(0f, max);
        float maxTemp = 0f;

        foreach(KeyValuePair<T, float> kvp in dicEntries)
        {
            maxTemp += kvp.Value;
            if (v <= maxTemp) return kvp.Key;
        }

        return dicEntries.ElementAt(0).Key;
    }

    /// <summary>
    /// Return the weight of a given value
    /// </summary>
    /// <param name="value">The given value</param>
    /// <returns>The weight of the value</returns>
    public float GetWeight(T value)
    {
        return dicEntries[value];
    }
}
