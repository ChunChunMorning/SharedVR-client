using UnityEngine;

public class MainUser : MonoBehaviour
{
	public int gazedObjectID
	{
		set
		{
			transform.GetChild (0).GetComponent<Renderer> ().material.color = PlayerColors.Get (value);

			m_gazedObjectID = value;
		}

		get { return m_gazedObjectID; }
	}
	[SerializeField] private int m_gazedObjectID;

	void FixedUpdate ()
	{
		NetworkManager.Instance.TellRotation(transform.rotation);
	}
}
