using System.Collections.Generic;
﻿using UnityEngine;

public class GazedObjectManager : MonoBehaviour
{
	private Dictionary<int, GazedObject> m_GazedObjects;

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
	}

	public void Add(int id, GazedObject gazedObject)
	{
		m_GazedObjects[id] = gazedObject;
	}

	public void Remove(int id)
	{
		m_GazedObjects.Remove(id);
	}

	public GazedObject Get(int id)
	{
		return m_GazedObjects[id];
	}
}
