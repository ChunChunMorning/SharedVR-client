using UnityEngine;

public enum SynchronizeMode
{
	Gaze, Rotation
}

public class NetworkManager : MonoBehaviour
{
	[SerializeField]
	private SynchronizeMode m_Mode;

	public SynchronizeMode mode
	{
		get { return m_Mode; }
		set { m_Mode = value; }
	}

	[SerializeField]
	private SocketObserver m_SocketObserver;

	public static NetworkManager Instance
	{
		get
		{
#if UNITY_EDITOR
			if (instance == null && !Application.isPlaying)
			{
				instance = FindObjectOfType<NetworkManager>();
			}
#endif
			if (instance == null)
			{
				Debug.LogError("Instance is not found!");
			}
			return instance;
		}
	}
	private static NetworkManager instance = null;

	public bool isConnected
	{
		get { return m_SocketObserver.isConnected; }
	}

	void Awake()
	{
		if (instance == null)
			instance = this;

		if (instance != this)
		{
			Debug.LogError("There are two instance!");
			DestroyImmediate(this);
		}
	}

	void Start()
	{
		Connect();
	}

	void Update()
	{
		while (m_SocketObserver.count > 0)
		{
			var message = m_SocketObserver.ReadLine ();
			var args = message.Split(',');

			if (args.Length < 2)
			{
#if UNITY_EDITOR
				Debug.LogWarning (message);
#endif

				continue;
			}

			switch (args[1])
			{
				case "you":
					UserManager.Instance.SetMainUser(int.Parse(args[2]));
					break;

				case "add":
					UserManager.Instance.Add(int.Parse(args[2]));
					break;

				case "erase":
					UserManager.Instance.Erase(int.Parse(args[2]));
					break;

				case "mode":
					m_Mode = (args[2] == "gaze") ? SynchronizeMode.Gaze : SynchronizeMode.Rotation;
					break;

				case "gaze":
					if (m_Mode == SynchronizeMode.Gaze)
					{
						UserManager.Instance.Get(int.Parse(args[0])).LookAt(int.Parse(args[2]));
					}
					break;

				case "rot":
					if (m_Mode == SynchronizeMode.Rotation)
					{
						UserManager.Instance.Get(int.Parse(args[0])).SetRotation(
							float.Parse(args[2]),
							float.Parse(args[3]),
							float.Parse(args[4])
						);
					}
					break;

				case "find":
					if (args[2] == "10")
					{
						SeManager.Instance.Play("finish");
					}

					CardManager.Instance.Find(int.Parse(args[2]));
					break;

				case "shuffle":
					CardManager.Instance.Shuffle(args[2], args[3]);
					break;

				case "set":
					if (int.Parse(args[2]) == UserManager.Instance.mainUser.gazedObjectID)
					{
						UserManager.Instance.SetPosition(int.Parse(args[3]));
					}
					break;
			}
		}
	}

	public bool Connect()
	{
		if (m_SocketObserver.isConnected)
		{
			return false;
		}

		return m_SocketObserver.Connect(Config.IPAddress, Config.Port);
	}

	public void TellGazedObjectID(int gazedObjectID)
	{
		if (!m_SocketObserver.isConnected)
			return;

		m_SocketObserver.Write("gaze," + gazedObjectID);
	}

	public void TellRotation(Quaternion rotation)
	{
		if (!m_SocketObserver.isConnected)
			return;

		m_SocketObserver.Write(
			"rot," +
			rotation.eulerAngles.x.ToString("##0") + ',' +
			rotation.eulerAngles.y.ToString("##0") + ',' +
			rotation.eulerAngles.z.ToString("##0")
		);
	}

	public void TellFoundCard(int number)
	{
		if (!m_SocketObserver.isConnected)
			return;

		if (number == 10)
		{
			SeManager.Instance.Play("finish");
		}

		m_SocketObserver.Write("find," + number);
	}

#if UNITY_EDITOR
	void Reset()
	{
		m_SocketObserver = GetComponent<SocketObserver>();
	}
#endif
}
