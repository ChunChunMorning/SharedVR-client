﻿using UnityEngine;

public class User : GazedBehaviour
{
	[SerializeField]
	private float m_Smoothness;

	private Transform m_Target;

	public int ID
	{
		get { return id; }
		set { id = value; }
	}
	[SerializeField]
	private int id;

	public override int gazedObjectID
	{
		get { return id; }
	}

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

#if UNITY_EDITOR
	void Reset()
	{
		gameObject.layer = LayerMask.NameToLayer("GazedObject");
	}
#endif
}
