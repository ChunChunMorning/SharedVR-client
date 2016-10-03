using System.Collections.Generic;
﻿using UnityEngine;

public class UserManager : MonoBehaviour
{
	[SerializeField]
	private MainUser m_MainUser;

	[SerializeField]
	private GameObject m_DummyUser;

	private Dictionary<int, DummyUser> m_DummyUsers;

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
	private static UserManager instance = null;

	public static bool IsReady
	{
		get { return instance != null; }
	}

	public MainUser MainUser
	{
		get { return m_MainUser; }
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

		m_DummyUsers = new Dictionary<int, DummyUser>();
	}

	public void Add(int id)
	{
		var dummyUser = (GameObject)Instantiate(m_DummyUser, Vector3.zero, Quaternion.identity);
		dummyUser.transform.SetParent(transform);
		var user = dummyUser.GetComponent<DummyUser>();
		user.ID = id;
		m_DummyUsers[id] = user;
	}

	public void Erase(int id)
	{
		m_DummyUsers.Remove(id);
		m_DummyUsers[id].Remove();
	}

	public DummyUser Get(int id)
	{
		return m_DummyUsers[id];
	}

#if UNITY_EDITOR
	void Reset()
	{
		m_MainUser = transform.GetChild(0).GetComponent<MainUser>();
	}
#endif
}
