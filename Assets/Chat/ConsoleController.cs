using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ConsoleController : MonoBehaviour
{
	[SerializeField] GameObject m_MessagePrefab;
	[SerializeField] GameObject m_Content;

	public void AddMessage (string text)
	{
		CreateMessage ("system: " + text);
	}

	public void AddMessage (string name, string text)
	{
		CreateMessage (name + ": " + text);
	}

	void CreateMessage (string text)
	{
		var message = Instantiate (m_MessagePrefab);
		message.transform.SetParent (m_Content.transform, false);
		message.transform.FindChild ("Text").GetComponent<Text> ().text = text;
	}

	#if UNITY_EDITOR

	void Reset ()
	{
		m_Content = GameObject.Find ("Content");
	}

	#endif
}
