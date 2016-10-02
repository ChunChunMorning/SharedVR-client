﻿using UnityEngine;

public class User : GazedBehaviour
{
	public int ID
	{
		get { return id; }
		set
		{
			id = value;
			GazedObjectManager.Instance.Add(id, this);
		}
	}
	[SerializeField]
	private int id;

	public override int gazedObjectID
	{
		get { return id; }
	}
}
