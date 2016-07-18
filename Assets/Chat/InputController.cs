using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InputController : MonoBehaviour
{
	[SerializeField] InputField m_InputField;
	[SerializeField] Text m_Placeholder;
	[SerializeField] ConsoleController m_ConsoleController;

	void Awake ()
	{

	}

	public void OnClick ()
	{
		if (m_InputField.text == "")
		{
			m_ConsoleController.AddMessage ("Write your message!");
			return;
		}

		m_ConsoleController.AddMessage ("You", m_InputField.text);
		m_InputField.text = "";
	}

	#if UNITY_EDITOR

	void Reset ()
	{
		m_InputField = FindObjectOfType<InputField> ();
		m_Placeholder = GameObject.Find ("Placeholder").GetComponent<Text> ();
		m_ConsoleController = FindObjectOfType<ConsoleController> ();
	}

	#endif
}
