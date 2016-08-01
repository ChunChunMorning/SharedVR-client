using System.Collections;
﻿using UnityEngine;


public class NetworkController : MonoBehaviour
{
	[SerializeField] SocketObserver m_SocketObserver;

	#if UNITY_EDITOR

	void Reset()
	{
		m_SocketObserver = GetComponent<SocketObserver>();
	}

	#endif
}
