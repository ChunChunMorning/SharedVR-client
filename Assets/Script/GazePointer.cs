using System;
using System.Collections;
using UnityEngine;


public class GazePointer : MonoBehaviour, IGvrGazePointer
{
	NetworkManager m_NetworkController;

	void Awake()
	{
		m_NetworkController = GameObject.Find("NetworkController").GetComponent<NetworkManager>();
	}

	void OnEnable()
	{
		GazeInputModule.gazePointer = this;
	}

	void OnDisable()
	{
		if (GazeInputModule.gazePointer == (IGvrGazePointer)this)
		{
			GazeInputModule.gazePointer = null;
		}
	}

	#region Implement IGvrGazePointer.

	public void OnGazeEnabled() {}

	public void OnGazeDisabled() {}

	public void OnGazeStart(Camera camera, GameObject targetObject, Vector3 intersectionPosition, bool isInteractive)
	{
		var gazedObjectID = targetObject.GetComponent<GazedObject>().gazedObjectID;

		m_NetworkController.TellGazedObjectID(gazedObjectID);
	}

	public void OnGazeStay(Camera camera, GameObject targetObject, Vector3 intersectionPosition, bool isInteractive) {}

	public void OnGazeExit(Camera camera, GameObject targetObject) {}

	public void OnGazeTriggerStart(Camera camera) {}

	public void OnGazeTriggerEnd(Camera camera) {}

	public void GetPointerRadius(out float innerRadius, out float outerRadius)
	{
		innerRadius = 0.0f;
		outerRadius = 0.1f;
	}

	#endregion

	#if UNITY_EDITOR

	void Reset()
	{

	}

	#endif
}
