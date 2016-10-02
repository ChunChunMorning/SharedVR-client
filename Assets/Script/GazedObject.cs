using UnityEngine;

[RequireComponent(typeof(Collider))]
public class GazedObject : GazedBehaviour
{
	public override int gazedObjectID
	{
		get { return m_GazedObjectID; }
	}
	[SerializeField]
	private int m_GazedObjectID;

#if UNITY_EDITOR
	void Awake()
	{
		if (m_GazedObjectID < 0)
			Debug.LogError("I don't have GazedObjectID.");
	}
#endif

	void Start()
	{
		GazedObjectManager.Instance.Add(m_GazedObjectID, this);
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
			gazedObjects[i].m_GazedObjectID = int.MaxValue - i;
		}
	}
#endif
}
