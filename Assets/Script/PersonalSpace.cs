using System;
﻿using UnityEngine;

public class PersonalSpace : MonoBehaviour
{
	[SerializeField]
	private User m_User;

	void OnTriggerEnter(Collider other)
	{
		Debug.Log(m_User.id + " is triggered!");
		var otherUser = other.GetComponent<User>();

		if (otherUser == null) return;

		// Young ID User invade.
		if (m_User.id > otherUser.id)
			OnInvaded(otherUser);
	}

	private void OnInvaded(User user)
	{
		var direction = transform.root.position - user.transform.root.position;
		direction = direction == Vector3.zero ? -transform.root.forward : direction.normalized;
		Debug.Log(m_User.id + " 's direction is " + direction);
		transform.root.position = user.transform.root.position + direction * (GetComponent<SphereCollider>().radius + float.Epsilon);
	}

#if UNITY_EDITOR
	void Reset()
	{
		m_User = transform.parent.GetComponent<User>();
	}
#endif
}
