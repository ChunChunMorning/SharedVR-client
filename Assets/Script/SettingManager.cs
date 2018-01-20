﻿using UnityEngine;
using UnityEngine.SceneManagement;
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

	public void OnStartClicked()
	{
		Config.VRMode = m_VRMode.isOn;
		Config.IPAddress = m_IPAddress.text;
		Config.Port = int.Parse(m_PortNumber.text);

		SceneManager.LoadScene(m_NextScene);
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
