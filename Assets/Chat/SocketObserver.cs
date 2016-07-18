using UnityEngine;
using System.Collections;
using System.Text;
using System.Net;
using System.Net.Sockets;

public class SocketObserver
{
	int m_PortNumber;
	TcpClient m_Client;
	NetworkStream m_NetworkStream;

	public SocketObserver (int portNumber)
	{
		m_PortNumber = portNumber;
	}

	~SocketObserver ()
	{
		if (m_Client.Connected)
		{
			Disconnect ();
		}
	}

	public void Connect (string address)
	{
		m_Client = new TcpClient ();
		m_Client.Connect (IPAddress.Parse (address), m_PortNumber);

		#if UNITY_EDITOR

		Debug.Log ("Connection is started on " + m_PortNumber + '.');

		#endif

		m_NetworkStream = m_Client.GetStream ();
	}

	public void Write (string text)
	{
		var data = Encoding.UTF8.GetBytes (text + '\n');
		m_NetworkStream.Write (data, 0, data.Length);
		m_NetworkStream.Flush ();
	}

	public void Disconnect ()
	{
		m_Client.Close ();
		m_Client = null;
		m_NetworkStream = null;

		#if UNITY_EDITOR

		Debug.Log ("Disconnect.");

		#endif
	}
}
