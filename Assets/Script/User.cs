﻿using UnityEngine;

public class User : MonoBehaviour
{
	public int ID
	{
		get { return id; }
		set { id = value; }
	}
	[SerializeField]
	private int id;

	public void LookAt(Transform target)
	{
		transform.LookAt(target);
	}
}
