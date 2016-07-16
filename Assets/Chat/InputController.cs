using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InputController : MonoBehaviour
{
	[SerializeField] InputField m_InputField;

	void Awake ()
	{

	}

	public void OnClick ()
	{
		m_InputField.text = "";
	}

	#if UNITY_EDITOR

	void Reset ()
	{
		m_InputField = FindObjectOfType<InputField> ();
	}

	#endif
}
