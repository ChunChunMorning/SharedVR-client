using UnityEngine;
using System.Collections;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class SocketObserver : MonoBehaviour
{
	[SerializeField] int m_PortNumber = 1435;
	[SerializeField] float m_DetectTime = 55.0f;
	[SerializeField] ConsoleController m_ConsoleController;
	TcpClient m_Client;
	NetworkStream m_NetworkStream;
	Coroutine m_ReadingCoroutine;
	float m_SocketBlankTime;

	void OnDestroy ()
	{
		if (m_Client != null && m_Client.Connected)
			Disconnect ();
	}

	public void Connect (string address)
	{
		m_Client = new TcpClient ();
		m_Client.Connect (IPAddress.Parse (address), m_PortNumber);

		#if UNITY_EDITOR

		Debug.Log ("Connection is started on " + m_PortNumber + '.');

		#endif

		m_NetworkStream = m_Client.GetStream ();
		m_ReadingCoroutine = StartCoroutine (Read());
		StartCoroutine (DetectDisconnection ());
	}

	public void Write (string text)
	{
		var data = Encoding.UTF8.GetBytes (text + '\n');
		m_NetworkStream.Write (data, 0, data.Length);
		m_NetworkStream.Flush ();
		m_SocketBlankTime = 0.0f;
	}

	public void Disconnect ()
	{
		m_Client.Close ();
		StopCoroutine (m_ReadingCoroutine);
		m_Client = null;
		m_NetworkStream = null;

		#if UNITY_EDITOR

		Debug.Log ("Disconnect.");

		#endif
	}

	IEnumerator Read ()
	{
		while (m_Client.Connected)
		{
			if (m_Client.Available > 0)
			{
				var data = new byte [256];
				m_NetworkStream.Read (data, 0, data.Length);
				var text = Encoding.UTF8.GetString (data);
				var message = text.Split (',');
				m_ConsoleController.AddMessage (message[0], message[1]);

				#if UNITY_EDITOR

				Debug.Log (Encoding.UTF8.GetString (data) + "is Coming!");

				#endif
			}

			yield return null;
		}
	}

	IEnumerator DetectDisconnection ()
	{
		m_SocketBlankTime = 0.0f;

		while (m_Client.Connected)
		{
			m_SocketBlankTime += Time.deltaTime;

			if (m_SocketBlankTime > m_DetectTime)
			{
				Write ("");
			}

			yield return null;
		}

		Debug.Log ("Detect End");
	}

	#if UNITY_EDITOR

	void Reset ()
	{
		m_ConsoleController = FindObjectOfType<ConsoleController> ();
	}

	#endif
}
