using UnityEngine;
using System.Collections;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class SocketObserver : MonoBehaviour
{
	[SerializeField] int m_PortNumber = 1435;
	[SerializeField] ConsoleController m_ConsoleController;
	TcpClient m_Client;
	NetworkStream m_NetworkStream;
	Coroutine m_ReadingCoroutine;

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
	}

	public void Write (string text)
	{
		var data = Encoding.UTF8.GetBytes (text + '\n');
		m_NetworkStream.Write (data, 0, data.Length);
		m_NetworkStream.Flush ();
	}

	public void Disconnect ()
	{
		Write ("quit");
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
				m_ConsoleController.AddMessage (Encoding.UTF8.GetString (data));

				#if UNITY_EDITOR

				Debug.Log (Encoding.UTF8.GetString (data) + "is Coming!");

				#endif
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
