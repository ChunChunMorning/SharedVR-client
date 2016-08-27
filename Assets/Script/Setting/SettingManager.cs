using System.Collections;
﻿using UnityEngine;
using UnityEngine.UI;


public class SettingManager : MonoBehaviour
{
	[SerializeField] Toggle m_VRMode;
	[SerializeField] InputField m_IPAddress;
	[SerializeField] InputField m_PortNumber;

	NetworkController m_NetworkController;

	void Start()
	{
		m_NetworkController = FindObjectOfType<NetworkController>();
	}

	public void OnStartClicked()
	{
		m_NetworkController.TryConnect();
	}

	#if UNITY_EDITOR

	void Reset()
	{
		m_VRMode = GameObject.Find("VRMode").GetComponent<Toggle>();
		m_IPAddress = GameObject.Find("IPAddress/InputField").GetComponent<InputField>();
		m_PortNumber = GameObject.Find("Port/InputField").GetComponent<InputField>();
	}

	#endif
}
