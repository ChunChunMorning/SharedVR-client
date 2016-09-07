using System;
﻿using UnityEngine;

public class PersonalSpace : MonoBehaviour
{
	public event Action<User> onInvade;

	void Awake()
	{
		// Test.
		onInvade += (User other) => { Debug.Log("Move from " + other.id + "."); };
	}

	void OnTriggerEnter(Collider other)
	{
		var otherUser = other.GetComponent<User>();

		if (otherUser == null) return;

		// Old ID User only move.
		if (transform.parent.GetComponent<User>().id > otherUser.id)
			onInvade(otherUser);
	}
}
