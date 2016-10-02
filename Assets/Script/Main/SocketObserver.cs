using System;
using System.Collections;
using System.Collections.Generic;
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
	Queue<string> m_ReceivingData;

	public event Action OnDisconnect;

	public bool isConnected
	{
		get { return m_Client != null; }
	}

	public int count
	{
		get { return m_ReceivingData.Count; }
	}

	public bool Connect(string address, int port)
	{
		try
		{
			m_Client = new TcpClient();
			m_Client.Connect(IPAddress.Parse(address), port);
			m_NetworkStream = m_Client.GetStream();
			m_ReceivingData = new Queue<string>();
			StartCoroutine(Read());
			StartCoroutine(DetectDisconnection());
		}
		catch (SocketException)
		{
			m_Client = null;
			m_NetworkStream = null;

			return false;
		}

		return true;
	}

	public void Disconnect()
	{
		m_Client.Close();
		m_Client = null;
		m_NetworkStream = null;
		m_ReceivingData = null;
		OnDisconnect();
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
			Debug.Log("Send: " + text + "\\n");
#endif
		}
		catch (IOException)
		{
			Disconnect();
		}
	}

	public string ReadLine()
	{
		return m_ReceivingData.Dequeue();
	}

	IEnumerator Read()
	{
		while (m_Client != null)
		{
			if (m_Client.Available > 0)
			{
				var data = new byte[256];
				m_NetworkStream.Read(data, 0, data.Length);
				var lines = Encoding.UTF8.GetString(data).Split('\n');

				// Last Element is Empty.
				var limit = lines.Length - 1;

				for (var i = 0; i < limit; i++)
				{
					m_ReceivingData.Enqueue(lines[i]);
				}

#if UNITY_EDITOR
				Debug.Log("Receive: " + Encoding.UTF8.GetString(data).Replace("\n", "\\n"));
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
