using System.Collections;
﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
	[SerializeField]
	private string m_NextScene;

	[SerializeField]
	private Toggle m_VRMode;

	[SerializeField]
	private InputField m_IPAddress;

	[SerializeField]
	private InputField m_PortNumber;

	[SerializeField]
	private Text m_Message;

	private Coroutine m_DisplayFailureCoroutine;

	public void OnStartClicked()
	{
		var address = m_IPAddress.text;
		var port = int.Parse(m_PortNumber.text);

		NetworkManager.Instance.TryConnect(address, port, OnSuccess, OnFailure);
	}

	private void OnSuccess()
	{
		GvrViewer.Instance.VRModeEnabled = m_VRMode.isOn;

		FindObjectOfType<GazeInputModule>().enabled = true;
		FindObjectOfType<StandaloneInputModule>().enabled = false;
		FindObjectOfType<GameController>().SwitchScene("Setting", m_NextScene, 0.5f);
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

		yield return new WaitForSeconds(1f);

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
