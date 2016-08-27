﻿using System.Collections;
﻿using UnityEngine;
using UnityEngine.UI;


public class SettingManager : MonoBehaviour
{
	[SerializeField] Toggle m_VRMode;
	[SerializeField] InputField m_IPAddress;
	[SerializeField] InputField m_PortNumber;
	[SerializeField] Text m_Message;

	NetworkController m_NetworkController;
	Coroutine m_DisplayFailureCoroutine;

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
		if (m_DisplayFailureCoroutine != null)
			StopCoroutine(m_DisplayFailureCoroutine);
		
		m_DisplayFailureCoroutine = StartCoroutine(DisplayFailure());
	}

	private IEnumerator DisplayFailure()
	{
		m_Message.text = "Sorry, I can't connect.";

		yield return new WaitForSeconds(1.0f);

		m_Message.text = "Please retry.";
	}

	#if UNITY_EDITOR

	void Reset()
	{
		m_VRMode = GameObject.Find("VRMode").GetComponent<Toggle>();
		m_IPAddress = GameObject.Find("IPAddress/InputField").GetComponent<InputField>();
		m_PortNumber = GameObject.Find("Port/InputField").GetComponent<InputField>();
		m_Message = GameObject.Find("Message").GetComponent<Text>();
	}

	#endif
}
