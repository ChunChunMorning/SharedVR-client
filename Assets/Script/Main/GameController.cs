using System.Collections;
﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
	[SerializeField]
	EyelidController m_EyelidController;

	void Awake()
	{
#if UNITY_EDITOR
		// If scene is already loaded.
		if (SceneManager.sceneCount > 1)
			return;
#endif
		SceneManager.LoadScene("Setting", LoadSceneMode.Additive);
	}

	public void SwitchScene(string unloadScene, string loadScene, float time)
	{
		StartCoroutine(SwitchSceneCoroutine(unloadScene, loadScene, time));
	}

	private IEnumerator SwitchSceneCoroutine(string unloadScene, string loadScene, float time)
	{
		m_EyelidController.Close(
			time,
			() => {
				SceneManager.UnloadScene(unloadScene);
				SceneManager.LoadScene(loadScene, LoadSceneMode.Additive);
			}
		);

		for (;;)
		{
			// TODO Replace Trigger.
			if (Input.GetMouseButtonDown(0))
				break;

			yield return null;
		}

		m_EyelidController.Open(time, () => { });
	}

#if UNITY_EDITOR
	void Reset()
	{
		m_EyelidController = FindObjectOfType<EyelidController>();
	}
#endif
}
