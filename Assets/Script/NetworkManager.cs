using System;
using System.Collections;
﻿using UnityEngine;


public class NetworkManager : MonoBehaviour
{
	[SerializeField]
	private SocketObserver m_SocketObserver;

	private static NetworkManager instance = null;

	public static NetworkManager Instance
	{
		get
		{
#if UNITY_EDITOR
			if (instance == null && !Application.isPlaying)
			{
				instance = FindObjectOfType<NetworkManager>();
			}
#endif
			if (instance == null)
			{
				Debug.LogError("Instance is not found!");
			}
			return instance;
		}
	}

	void Awake()
	{
		if (instance == null)
			instance = this;

		if (instance != this)
		{
			Debug.LogError("There are two instance!");
			DestroyImmediate(this);
		}
	}

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
