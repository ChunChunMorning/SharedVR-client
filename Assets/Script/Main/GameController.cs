using System.Collections;
﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
	void Awake()
	{
#if UNITY_EDITOR
		// If scene is already loaded.
		if (SceneManager.sceneCount > 1)
			return;
#endif
		SceneManager.LoadScene("Setting", LoadSceneMode.Additive);
	}
}
