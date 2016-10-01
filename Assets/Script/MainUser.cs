﻿using UnityEngine;

public class MainUser : User
{
	private Transform m_CameraTransform;
	private Vector3 m_Offset;

	void Awake()
	{
		m_CameraTransform = Camera.main.transform;
		m_Offset = m_CameraTransform.position - transform.position;
	}

	void LateUpdate()
	{
		m_CameraTransform.position = transform.position + m_Offset;
		transform.rotation = m_CameraTransform.rotation;
	}
}
