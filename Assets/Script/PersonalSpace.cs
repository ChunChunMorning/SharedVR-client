﻿using UnityEngine;

public class PersonalSpace : MonoBehaviour
{
	[SerializeField]
	private User m_User;

	void OnTriggerEnter(Collider other)
	{
		var otherUser = other.GetComponent<User>();

		if (otherUser == null) return;

		// Young ID User invade.
		if (m_User.id > otherUser.id)
			OnInvaded(otherUser);
	}

	private void OnInvaded(User user)
	{
		var direction = m_User.transform.position - user.transform.position;
		direction = direction == Vector3.zero ? -m_User.transform.forward : direction.normalized;
		m_User.transform.position =
			user.transform.position +
			direction * (GetComponent<SphereCollider>().radius + float.Epsilon);
	}

#if UNITY_EDITOR
	void Reset()
	{
		m_User = transform.parent.GetComponent<User>();
	}
#endif
}
