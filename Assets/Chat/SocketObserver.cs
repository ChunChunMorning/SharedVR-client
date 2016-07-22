using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class SocketObserver : MonoBehaviour
{
	[SerializeField] int m_PortNumber = 1435;
	[SerializeField] float m_DetectTime = 55.0f;
	[SerializeField] ConsoleController m_ConsoleController;
	public event Action OnDisconnect;
	float m_NoWritingTime;
	TcpClient m_Client;
	NetworkStream m_NetworkStream;

	public void Connect (string address)
	{
		m_Client = new TcpClient ();
		m_Client.Connect (IPAddress.Parse (address), m_PortNumber);
		m_NetworkStream = m_Client.GetStream ();
		StartCoroutine (Read());
		StartCoroutine (DetectDisconnection ());

		#if UNITY_EDITOR

		Debug.Log ("Connect on " + m_PortNumber + '.');

		#endif
	}

	public void Write (string text)
	{
		try
		{
			var data = Encoding.UTF8.GetBytes (text + '\n');
			m_NetworkStream.Write (data, 0, data.Length);
			m_NetworkStream.Flush ();
			m_NoWritingTime = 0.0f;

			#if UNITY_EDITOR

			Debug.Log ("Send: " + text);

			#endif
		}
		catch (IOException)
		{
			Disconnect ();
		}
	}

	public void Disconnect ()
	{
		m_Client.Close ();
		m_Client = null;
		m_NetworkStream = null;
		m_ConsoleController.AddMessage ("Disconnect.");
		OnDisconnect ();

		#if UNITY_EDITOR

		Debug.Log ("Disconnect.");

		#endif
	}

	IEnumerator Read ()
	{
		while (m_Client != null)
		{
			if (m_Client.Available > 0)
			{
				var data = new byte [256];
				m_NetworkStream.Read (data, 0, data.Length);
				var text = Encoding.UTF8.GetString (data);
				var message = text.Split (',');
				m_ConsoleController.AddMessage (message[0], message[1]);

				#if UNITY_EDITOR

				Debug.Log ("Receive: " + text);

				#endif
			}

			yield return null;
		}
	}

	IEnumerator DetectDisconnection ()
	{
		m_NoWritingTime = 0.0f;

		while (m_Client != null)
		{
			m_NoWritingTime += Time.deltaTime;

			if (m_NoWritingTime > m_DetectTime)
			{
				Write ("");
			}

			yield return null;
		}
	}

	#if UNITY_EDITOR

	void Reset ()
	{
		m_ConsoleController = FindObjectOfType<ConsoleController> ();
	}

	#endif
}
