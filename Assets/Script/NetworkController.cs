using System.Collections;
﻿using UnityEngine;


public class NetworkController : MonoBehaviour
{
	[SerializeField] string m_IPAddress = "127.0.0.1";
	[SerializeField] int m_PortNumber = 1435;
	[SerializeField] SocketObserver m_SocketObserver;

	void Start()
	{
		m_SocketObserver.Connect(m_IPAddress, m_PortNumber);
	}

	void TellGazedObject(int gazedObjectID)
	{
		if (!m_SocketObserver.Connected())
			return;

		m_SocketObserver.Write("gaze," + gazedObjectID);
	}

	#if UNITY_EDITOR

	void Reset()
	{
		m_SocketObserver = GetComponent<SocketObserver>();
	}

	#endif
}
