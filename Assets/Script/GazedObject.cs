using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;


[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(EventTrigger))]
public class GazedObject : MonoBehaviour, IGvrGazeResponder
{
	[SerializeField] int m_GazedObjectID;

	void Awake()
	{
		#if UNITY_EDITOR

		if (m_GazedObjectID < 0)
			Debug.Log("I don't have GazedObjectID.");

		#endif
	}

	#region Implement IGvrGazeResponder.

	public void OnGazeEnter()
	{
		Debug.Log("Gaze ID: " + m_GazedObjectID);
	}

	public void OnGazeExit() {}

	public void OnGazeTrigger() {}

	#endregion

	#if UNITY_EDITOR

	void Reset()
	{
		gameObject.layer = LayerMask.NameToLayer("GazedObject");
		m_GazedObjectID = -1;
	}

	[ContextMenu("AssignAllGameObjectID")]
	void AssignAllGameObjectID()
	{
		var gazedObjects = FindObjectsOfType<GazedObject>();

		for (int i = 0; i < gazedObjects.Length; ++i)
		{
			gazedObjects[i].m_GazedObjectID = i;
		}
	}

	#endif
}
