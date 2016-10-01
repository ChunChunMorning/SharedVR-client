using System.Collections;
﻿using UnityEngine;

public class UserManager : MonoBehaviour
{
	[SerializeField]
	private User m_User;

	[SerializeField]
	private GameObject m_DummyUser;

	private static UserManager instance = null;

	public static UserManager Instance
	{
		get
		{
#if UNITY_EDITOR
			if (instance == null && !Application.isPlaying)
			{
				instance = FindObjectOfType<UserManager>();
			}
#endif
			if (instance == null)
			{
				Debug.LogError("Instance is not found!");
			}
			return instance;
		}
	}

	public static bool IsReady
	{
		get { return instance != null; }
	}

	public User MainUser
	{
		get { return m_User; }
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

	public void Add(int id, Vector3 position)
	{
		var dummyUser = (GameObject)Instantiate(m_DummyUser, position, Quaternion.identity);
		dummyUser.GetComponent<User>().id = id;
		dummyUser.transform.SetParent(transform);
	}

#if UNITY_EDITOR

	void Reset()
	{
		
	}

#endif
}
