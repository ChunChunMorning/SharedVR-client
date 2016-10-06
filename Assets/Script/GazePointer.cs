using System;
using System.Collections;
using UnityEngine;

public enum SynchronizeMode
{
	Destination, Rotation
}

public class GazePointer : MonoBehaviour, IGvrGazePointer
{
	[SerializeField]
	private SynchronizeMode m_Mode = SynchronizeMode.Destination;
	public SynchronizeMode Mode
	{
		get { return m_Mode; }
		set { m_Mode = value; }
	}

	private NetworkManager m_NetworkController;

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

	void Update()
	{
		if (m_Mode == SynchronizeMode.Rotation)
		{
			NetworkManager.Instance.TellRotation(transform.rotation);
		}
	}

#region Implement IGvrGazePointer.
	public void OnGazeEnabled() {}

	public void OnGazeDisabled() {}

	public void OnGazeStart(Camera camera, GameObject targetObject, Vector3 intersectionPosition, bool isInteractive)
	{
		if (m_Mode == SynchronizeMode.Destination)
		{
			var gazedObjectID = targetObject.GetComponent<GazedBehaviour>().gazedObjectID;

			m_NetworkController.TellGazedObjectID(gazedObjectID);
		}
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
}
