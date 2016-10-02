using System;
using UnityEngine;

public class GazedBehaviour : MonoBehaviour
{
	public virtual int gazedObjectID
	{
		get { throw new NotSupportedException(); }
	}
}
