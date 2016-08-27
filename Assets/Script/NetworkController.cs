using System;
using System.Collections;
﻿using UnityEngine;


public class NetworkController : MonoBehaviour
{
	[SerializeField] SocketObserver m_SocketObserver;

	public void TryConnect(string ipAddress, int portNumber, Action onSuccess, Action onFailure)
	{
		var success = m_SocketObserver.Connect(ipAddress, portNumber);

		if (success)
		{
			onSuccess();
		}
		else
		{
			onFailure();
		}
	}

	public void TellGazedObjectID(int gazedObjectID)
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
