using UnityEngine;

[RequireComponent(typeof(Collider))]
public class GazedObject : MonoBehaviour
{
	public int gazedObjectID
	{
		get { return m_GazedObjectID; }
	}
	[SerializeField]
	private int m_GazedObjectID;

	void Awake()
	{
#if UNITY_EDITOR
		if (m_GazedObjectID < 0)
			Debug.Log("I don't have GazedObjectID.");
#endif
	}

	void OnEnable()
	{
		GazedObjectManager.Instance.Add(m_GazedObjectID, this);
	}

	void OnDisable()
	{
		GazedObjectManager.Instance.Remove(m_GazedObjectID);
	}

#if UNITY_EDITOR
	void Reset()
	{
		gameObject.layer = LayerMask.NameToLayer("GazedObject");
		m_GazedObjectID = -1;
	}

	[ContextMenu("AssignGazedObjectIDAll")]
	void AssignGazedObjectIDAll()
	{
		var gazedObjects = FindObjectsOfType<GazedObject>();

		for (int i = 0; i < gazedObjects.Length; ++i)
		{
			gazedObjects[i].m_GazedObjectID = 100 + i;
		}
	}
#endif
}
