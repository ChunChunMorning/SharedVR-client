using System;
using System.Collections;
﻿using UnityEngine;


public class NetworkManager : MonoBehaviour
{
	[SerializeField]
	private SocketObserver m_SocketObserver;

	[SerializeField]
	private User m_User;

	[SerializeField]
	private GameObject m_DummyUser;

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
		if (!m_SocketObserver.isConnected)
			return;

		while (m_SocketObserver.count > 0)
		{
			var args = m_SocketObserver.ReadLine().Split(',');

			switch (args[1])
			{
				case "you":
					m_User.id = int.Parse(args[0]);
					break;

				case "add":
					var dummyUser = (GameObject)Instantiate(
										m_DummyUser,
										new Vector3(float.Parse(args[2]), float.Parse(args[3]), float.Parse(args[4])),
										Quaternion.identity
									);
					dummyUser.GetComponent<User>().id = int.Parse(args[0]);
					break;

				case "erase":
					// Erase User.
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
