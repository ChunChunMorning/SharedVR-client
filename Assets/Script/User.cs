﻿using UnityEngine;

public class User : MonoBehaviour
{
	[SerializeField]
	private int id;

	public int ID
	{
		get { return id; }
		set { id = value; }
	}
}
