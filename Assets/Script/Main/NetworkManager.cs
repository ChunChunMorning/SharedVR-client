using System;
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

	void Update()
	{
		if (!m_SocketObserver.isConnected || !UserManager.IsReady)
			return;

		while (m_SocketObserver.count > 0)
		{
			var args = m_SocketObserver.ReadLine().Split(',');

			switch (args[1])
			{
				case "you":
					UserManager.Instance.MainUser.ID = int.Parse(args[0]);
					break;

				case "add":
					UserManager.Instance.Add(int.Parse(args[0]));
					break;

				case "erase":
					UserManager.Instance.Erase(int.Parse(args[0]));
					break;

				case "gaze":
					UserManager.Instance.Get(
						int.Parse(args[0])
					).LookAt(GazedObjectManager.Instance.Get(int.Parse(args[2])).transform);
					break;

				default:
					break;
			}
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
		if (!m_SocketObserver.isConnected)
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
