﻿using UnityEngine;

public class DummyUser : User
{
	[SerializeField]
	private float m_Smoothness;

	private Transform m_Target;

	void Update()
	{
		if (m_Target == null)
			return;

		var dir = m_Target.position - transform.position;
		dir.y = 0f;
		transform.rotation = Quaternion.Slerp(
			transform.rotation,
			Quaternion.LookRotation(dir),
			m_Smoothness * Time.deltaTime
		);
	}

	public void LookAt(Transform target)
	{
		m_Target = target;
	}

	public void Remove()
	{
		GazedObjectManager.Instance.Remove(gazedObjectID);
		Destroy(gameObject);
	}

#if UNITY_EDITOR
	void Reset()
	{
		gameObject.layer = LayerMask.NameToLayer("GazedObject");
	}
#endif
}
