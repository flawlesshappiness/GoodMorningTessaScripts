using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObserver
{
    /// <summary>
    /// Called by observables to notify this observer
    /// </summary>
    void Notify(string tag);
}
