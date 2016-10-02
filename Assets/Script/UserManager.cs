using System.Collections.Generic;
﻿using UnityEngine;

public class UserManager : MonoBehaviour
{
	[SerializeField]
	private MainUser m_MainUser;

	[SerializeField]
	private GameObject m_DummyUser;

	private Dictionary<int, User> m_Users;

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

		m_Users = new Dictionary<int, User>();
	}

	public void Add(int id)
	{
		var dummyUser = (GameObject)Instantiate(m_DummyUser, Vector3.zero, Quaternion.identity);
		dummyUser.transform.SetParent(transform);
		var user = dummyUser.GetComponent<User>();
		user.ID = id;
		m_Users[id] = user;
	}

	public void Erase(int id)
	{
		Destroy(m_Users[id].gameObject);
		m_Users.Remove(id);
	}

	public User Get(int id)
	{
		return m_Users[id];
	}

#if UNITY_EDITOR
	void Reset()
	{
		m_MainUser = transform.GetChild(0).GetComponent<MainUser>();
	}
#endif
}
