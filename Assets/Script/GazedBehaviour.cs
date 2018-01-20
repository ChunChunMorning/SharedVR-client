using UnityEngine;

public class GazedBehaviour : MonoBehaviour
{
	[SerializeField]
	private int m_GazedObjectID;

	public int gazedObjectID
	{
		set
		{
			GazedObjectManager.Instance.Remove(m_GazedObjectID);
			GazedObjectManager.Instance.Add(value, this);
			m_GazedObjectID = value;
		}
		get
		{
			return m_GazedObjectID;
		}
	}

	void Start()
	{
		GazedObjectManager.Instance.Add(m_GazedObjectID, this);
	}

#if UNITY_EDITOR
	public void SetInitGazedObjectID(int id)
	{
		m_GazedObjectID = id;
	}
#endif
}
