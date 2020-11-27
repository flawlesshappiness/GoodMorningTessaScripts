using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionTrigger : MonoBehaviour {
	public UnityEventCollider2D onEnter;
	public UnityEventCollider2D onExit;
	public UnityEventCollider2D onStay;

	void OnTriggerEnter2D(Collider2D c)
	{
		if(onExit != null) onEnter.Invoke(c);
	}

	void OnTriggerExit2D(Collider2D c)
	{
		if(onExit != null) onExit.Invoke(c);
	}

	void OnTriggerStay2D(Collider2D c)
	{
		if(onStay != null) onStay.Invoke(c);
	}
}
