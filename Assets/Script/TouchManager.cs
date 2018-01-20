using UnityEngine;
using System.Collections;

public class TouchManager : MonoBehaviour
{
	public static bool Down()
	{
#if UNITY_EDITOR
		return Input.GetMouseButtonDown(0);
#else
		foreach (var touch in Input.touches)
		{
			if (touch.phase == TouchPhase.Began)
			{
				return true;
			}
		}

		return false;
#endif
	}
}
