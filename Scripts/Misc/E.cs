using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Direction { NONE, UP, DOWN, LEFT, RIGHT } // General enum for direction

public static class E
{
    /// <summary>
    /// Returns a random enum value from a given enum
    /// </summary>
    /// <typeparam name="T">The enum type</typeparam>
    /// <returns>The enum value</returns>
    public static T GetRandomEnum<T>()
    {
        var values = Enum.GetValues(typeof(T));
        return GetRandomEnum<T>(values, 0, values.Length);
    }

    /// <summary>
    /// Returns a random enum value from a given enum
    /// </summary>
    /// <typeparam name="T">The enum type</typeparam>
    /// <param name="min">Min enum index</param>
    /// <param name="max">Max enum index</param>
    /// <returns>The enum value</returns>
    public static T GetRandomEnum<T>(int min, int max)
    {
        var values = Enum.GetValues(typeof(T));
        return GetRandomEnum<T>(values, min, max);
    }

    /// <summary>
    /// Returns a random enum value from an array of enum values
    /// </summary>
    /// <typeparam name="T">The enum type</typeparam>
    /// <param name="values">The enum array</param>
    /// <param name="min">Min enum index</param>
    /// <param name="max">Max enum index</param>
    /// <returns>The enum value</returns>
    static T GetRandomEnum<T>(Array values, int min, int max)
    {
        return (T)values.GetValue(UnityEngine.Random.Range(min, max));
    }
}