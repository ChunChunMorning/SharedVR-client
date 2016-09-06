using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;


public class SocketObserver : MonoBehaviour
{
	[SerializeField] float m_DetectTime = 55.0f;

	float m_NoWritingTime;
	TcpClient m_Client;
	NetworkStream m_NetworkStream;

	public event Action OnDisconnect;

	public bool isConnected
	{
		get { return m_Client != null; }
	}

	public bool Connect(string address, int port)
	{
		try
		{
			m_Client = new TcpClient();
			m_Client.Connect(IPAddress.Parse(address), port);
			m_NetworkStream = m_Client.GetStream();
			StartCoroutine(Read());
			StartCoroutine(DetectDisconnection());
		}
		catch (SocketException)
		{
			m_Client = null;
			m_NetworkStream = null;

			return false;
		}

		#if UNITY_EDITOR

		Debug.Log("Connect on " + port + '.');

		#endif

		return true;
	}

	public void Disconnect()
	{
		m_Client.Close();
		m_Client = null;
		m_NetworkStream = null;
		OnDisconnect();

		#if UNITY_EDITOR

		Debug.Log("Disconnect.");

		#endif
	}

	public void Write(string text)
	{
		try
		{
			var data = Encoding.UTF8.GetBytes(text + '\n');
			m_NetworkStream.Write(data, 0, data.Length);
			m_NetworkStream.Flush();
			m_NoWritingTime = 0.0f;

			#if UNITY_EDITOR

			Debug.Log("Send: " + text);

			#endif
		}
		catch (IOException)
		{
			Disconnect();
		}
	}

	IEnumerator Read()
	{
		while (m_Client != null)
		{
			if (m_Client.Available > 0)
			{
				var data = new byte[256];
				m_NetworkStream.Read(data, 0, data.Length);
				var text = Encoding.UTF8.GetString(data);

				#if UNITY_EDITOR

				Debug.Log("Receive: " + text);

				#endif
			}

			yield return null;
		}
	}

	IEnumerator DetectDisconnection()
	{
		m_NoWritingTime = 0.0f;

		while (m_Client != null)
		{
			m_NoWritingTime += Time.deltaTime;

			if (m_NoWritingTime > m_DetectTime)
			{
				Write("");
			}

			yield return null;
		}
	}
}
