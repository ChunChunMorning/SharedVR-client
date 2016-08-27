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
		var address = m_IPAddress.text;
		var port = int.Parse(m_PortNumber.text);

		m_NetworkController.TryConnect(address, port, OnSuccess, OnFailure);
	}

	private void OnSuccess()
	{
		Debug.Log("Success");
	}

	private void OnFailure()
	{
		Debug.Log("Failure");
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
