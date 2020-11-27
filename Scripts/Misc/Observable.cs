using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observable
{
    private List<IObserver> subscribers = new List<IObserver>();
    private List<IObserver> unsubscribers = new List<IObserver>();

    /// <summary>
    /// Subscribe an observer to this observable
    /// </summary>
    /// <param name="o">The observer</param>
    public void Subscribe(IObserver o)
    {
        if (!subscribers.Contains(o)) subscribers.Add(o);
    }

    /// <summary>
    /// Unsubscribe an observer to this observable
    /// </summary>
    /// <param name="o">The observer</param>
    public void UnSubscribe(IObserver o)
    {
        if (!unsubscribers.Contains(o)) unsubscribers.Add(o);
    }

    /// <summary>
    /// Notify all subscribed observers with a tag
    /// </summary>
    /// <param name="tag">The tag</param>
    public void NotifyAll(string tag)
    {
        //Unsubscribe subscribers
        foreach(IObserver o in unsubscribers)
        {
            subscribers.Remove(o);
        }
        unsubscribers.Clear();

        //Notify subscribers
        foreach(IObserver o in subscribers)
        {
            o.Notify(tag);
        }
    }
}
