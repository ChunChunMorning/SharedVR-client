using System.Collections.Generic;
using UnityEngine;

public class DummyUser : MonoBehaviour
{
	[SerializeField]
	private float m_Smoothness;

	private Quaternion m_Target = Quaternion.identity;

	[SerializeField]
	private GazedBehaviour m_GazedBehaviour;

	public int gazedObjectID
	{
		set { m_GazedBehaviour.gazedObjectID = value; }
		get { return m_GazedBehaviour.gazedObjectID; }
	}

	void FixedUpdate()
	{
		transform.rotation = Quaternion.Slerp(transform.rotation, m_Target, m_Smoothness);
	}

	public void LookAt(int targetID)
	{
		try
		{
			var target = GazedObjectManager.Instance.Get(targetID).transform;

			m_Target = Quaternion.LookRotation(target.position - transform.position);
		}
		catch (KeyNotFoundException)
		{
			Debug.LogWarning(gazedObjectID + " see " + targetID + ".");
		}
	}

	public void SetRotation(float x, float y, float z)
	{
		m_Target = Quaternion.Euler(x, y, z);
	}

#if UNITY_EDITOR
	void Reset()
	{
		m_GazedBehaviour = GetComponent<GazedBehaviour>();
	}
#endif
}
