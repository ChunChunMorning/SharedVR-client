using System;
using System.Collections.Generic;
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
					UserManager.Instance.MainUser.transform.position = new Vector3(
						float.Parse(args[2]), float.Parse(args[3]), float.Parse(args[4])
					);
					break;

				case "add":
					UserManager.Instance.Add(int.Parse(args[0]));
					break;

				case "erase":
					UserManager.Instance.Erase(int.Parse(args[0]));
					break;

				case "gaze":
					try
					{
						var user = UserManager.Instance.Get(int.Parse(args[0]));
						user.LookAt(GazedObjectManager.Instance.Get(int.Parse(args[2])).transform);
					}
					catch (KeyNotFoundException)
					{
#if UNITY_EDITOR
						Debug.LogWarning(int.Parse(args[0]) + " see " + int.Parse(args[2]) + ".");
#endif
					}
					break;

				case "mode":
					FindObjectOfType<GazePointer>().Mode = (args[2] == "rot") ? SynchronizeMode.Rotation : SynchronizeMode.Destination;
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

	public void TellRotation(Quaternion rotation)
	{
		if (!m_SocketObserver.isConnected)
			return;

		m_SocketObserver.Write(
			"rot," +
			rotation.x + ',' +
			rotation.y + ',' +
			rotation.z + ',' +
			rotation.w
		);
	}

#if UNITY_EDITOR
	void Reset()
	{
		m_SocketObserver = GetComponent<SocketObserver>();
	}
#endif
}
