using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(EventTrigger))]
[RequireComponent(typeof(GazedBehaviour))]
public class GazedObject : MonoBehaviour
{
	[SerializeField]
	private GazedBehaviour m_GazedBehaviour;

	public void TellGazedObjectID()
	{
		NetworkManager.Instance.TellGazedObjectID(m_GazedBehaviour.gazedObjectID);
	}

#if UNITY_EDITOR
	void Reset()
	{
		gameObject.layer = LayerMask.NameToLayer("GazedObject");
		m_GazedBehaviour = GetComponent<GazedBehaviour>();
	}

	[ContextMenu("AssignGazedObjectIDAll")]
	void AssignGazedObjectIDAll()
	{
		var gazedObjects = FindObjectsOfType<GazedObject>();

		for (int i = 0; i < gazedObjects.Length; ++i)
		{
			var gazedObjectname = gazedObjects[i].name;

			if (
				gazedObjectname.IndexOf("user", System.StringComparison.CurrentCultureIgnoreCase) != -1 ||
				gazedObjectname.IndexOf("card", System.StringComparison.CurrentCultureIgnoreCase) != -1
			)
			{
				continue;
			}

			gazedObjects[i].m_GazedBehaviour.SetInitGazedObjectID(int.MaxValue - i);
		}
	}
#endif
}
