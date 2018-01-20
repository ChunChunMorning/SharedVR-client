using System.Collections.Generic;
using UnityEngine;

public class UserManager : MonoBehaviour
{
	[SerializeField]
	private MainUser m_MainUser;

	[SerializeField]
	private DummyUser[] m_DummyUsers;

	private Vector3[] m_Positions;

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

	public MainUser mainUser
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
	}

	void Start()
	{
		m_Positions = new Vector3[m_DummyUsers.Length];

		for (int i = 0; i < m_DummyUsers.Length; ++i)
		{
			m_Positions[i] = m_DummyUsers[i].transform.position;
		}
	}

	public void SetMainUser(int id)
	{
		Debug.Assert(0 <= id && id <= 2);

		m_MainUser.gazedObjectID = id;
		m_MainUser.transform.position = m_DummyUsers[id].transform.position;
	}

	public void Add(int id)
	{
		Debug.Assert(0 <= id && id <= 2);

		m_DummyUsers[id].gameObject.SetActive(true);
	}

	public void Erase(int id)
	{
		Debug.Assert(0 <= id && id <= 2);

		m_DummyUsers[id].gameObject.SetActive(false);
	}

	public DummyUser Get(int id)
	{
		Debug.Assert(0 <= id && id <= 2);

		return m_DummyUsers[id];
	}

	public void SetPosition(int index)
	{
		Debug.Assert(0 <= index && index <= 2);

		m_MainUser.transform.position = m_Positions[index];

		for (int i = 0; i < m_Positions.Length; ++i)
		{
			var userIndex = (m_MainUser.gazedObjectID + i) % m_Positions.Length;
			var posIndex = (index + i) % m_Positions.Length;

			m_DummyUsers[userIndex].transform.position = m_Positions[posIndex];
		}
	}

#if UNITY_EDITOR
	void Reset()
	{
		m_MainUser = FindObjectOfType<MainUser>();
		m_DummyUsers = FindObjectsOfType<DummyUser>();
	}
#endif
}
