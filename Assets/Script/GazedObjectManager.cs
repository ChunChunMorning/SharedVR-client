using System.Collections.Generic;
﻿using UnityEngine;

public class GazedObjectManager : MonoBehaviour
{
	private Dictionary<int, GazedBehaviour> m_GazedObjects;

	public static GazedObjectManager Instance
	{
		get
		{
#if UNITY_EDITOR
			if (instance == null && !Application.isPlaying)
			{
				instance = FindObjectOfType<GazedObjectManager>();
			}
#endif
			if (instance == null)
			{
				Debug.LogError("Instance is not found!");
			}
			return instance;
		}
	}
	private static GazedObjectManager instance = null;

	void Awake()
	{
		if (instance == null)
			instance = this;

		if (instance != this)
		{
			Debug.LogError("There are two instance!");
			DestroyImmediate(this);
		}

		m_GazedObjects = new Dictionary<int, GazedBehaviour>();
	}

	public void Add(int id, GazedBehaviour gazedObject)
	{
		m_GazedObjects[id] = gazedObject;
	}

	public void Remove(int id)
	{
		m_GazedObjects.Remove(id);
	}

	public GazedBehaviour Get(int id)
	{
		return m_GazedObjects[id];
	}
}
