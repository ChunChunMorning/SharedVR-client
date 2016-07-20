using UnityEngine;
using UnityEngine.UI;
using System.Collections;

enum State
{
	ExpectedAddress,
	ExpectedMessage
}

public class InputController : MonoBehaviour
{
	[SerializeField] InputField m_InputField;
	[SerializeField] Text m_Placeholder;
	[SerializeField] Text m_Button;
	[SerializeField] ConsoleController m_ConsoleController;
	[SerializeField] SocketObserver m_SocketObserver;

	State m_State;

	void Awake ()
	{
		m_State = State.ExpectedAddress;
	}

	public void OnClick ()
	{
		switch (m_State)
		{
		case State.ExpectedAddress:
			m_SocketObserver.Connect (m_InputField.text);
			m_Placeholder.text = "Write message.";
			m_Button.text = "Send";
			m_State = State.ExpectedMessage;
			break;

		case State.ExpectedMessage:
			switch (m_InputField.text)
			{
			case "":
				m_ConsoleController.AddMessage ("Write your message!");
				return;

			case "logout":
				m_SocketObserver.Disconnect ();
				m_ConsoleController.AddMessage ("Logout.");
				m_Placeholder.text = "Write address.";
				m_Button.text = "Connect";
				m_State = State.ExpectedAddress;
				break;

			default:
				m_ConsoleController.AddMessage ("You", m_InputField.text);
				m_SocketObserver.Write (m_InputField.text);
				break;
			}

			break;
		}

		m_InputField.text = "";
	}

	#if UNITY_EDITOR

	void Reset ()
	{
		m_InputField = FindObjectOfType<InputField> ();
		m_Placeholder = GameObject.Find ("Placeholder").GetComponent<Text> ();
		m_Button = GameObject.Find ("Button/Text").GetComponent<Text> ();
		m_ConsoleController = FindObjectOfType<ConsoleController> ();
		m_SocketObserver = FindObjectOfType<SocketObserver> ();
	}

	#endif
}
