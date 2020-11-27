using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// UnityEvent for float parameters
/// </summary>
[System.Serializable]
public class UnityEventFloat : UnityEvent<float> {

}

/// <summary>
/// UnityEvent for gameobject parameters
/// </summary>
[System.Serializable]
public class UnityEventGameObject : UnityEvent<GameObject> {
	
}

/// <summary>
/// UnityEvent for Collider2D parameters
/// </summary>
[System.Serializable]
public class UnityEventCollider2D : UnityEvent<Collider2D> {

}