using UnityEngine;

public class CardController : MonoBehaviour
{
	private int m_Number;

	private int m_PlayerID;

	private float m_Scale;

	private CardManager m_Manager;

	private Collider m_Collider;

	public int number
	{
		get { return m_Number; }
	}

	public bool active { get; set; }

	void Awake()
	{
		m_Number = int.Parse(GetComponent<Renderer>().material.name.Substring(0, 2));
		m_Scale = (transform.localScale.x + transform.localScale.y + transform.localScale.z) / 3f;
		m_Manager = FindObjectOfType<CardManager>();
		m_Collider = GetComponent<Collider>();
		active = false;
	}

	void Update()
	{
		if (active)
		{
			transform.localScale = m_Scale * Vector3.one;
			m_Collider.enabled = true;
		}
		else
		{
			var scale = Mathf.Clamp(transform.localScale.x - m_Scale * 2f * Time.deltaTime, 0f, m_Scale);

			transform.localScale = scale * Vector3.one;

			m_Collider.enabled = false;
		}
	}

	public void SetPlayerID(int playerID)
	{
		m_PlayerID = playerID;

		GetComponent<Renderer> ().material.color = PlayerColors.Get (playerID);
	}

	public void Find()
	{
		if (!m_Manager.FoundedAll(number) || FindObjectOfType<MainUser>().gazedObjectID != m_PlayerID)
		{
			SeManager.Instance.Play("bat");

			return;
		}

		active = false;

		SeManager.Instance.Play("correct");
		NetworkManager.Instance.TellFoundCard(number);
	}
}
