using System;
using System.Collections;
﻿using UnityEngine;
using UnityEngine.UI;

public class EyelidController : MonoBehaviour
{
	[SerializeField]
	private RectTransform m_UpperEyelid;

	[SerializeField]
	private RectTransform m_LowerEyelid;

	private float m_OpenedUpperEyelidY;
	private float m_OpenedLowerEyelidY;
	private Coroutine m_Coroutine;

	void Awake()
	{
		m_OpenedUpperEyelidY = m_UpperEyelid.localPosition.y;
		m_OpenedLowerEyelidY = m_LowerEyelid.localPosition.y;
	}

	public void Open(float time, Action onFinish)
	{
		if (m_Coroutine != null)
			StopCoroutine(m_Coroutine);

		StartCoroutine(MoveEyelidsCoroutine(m_OpenedUpperEyelidY, m_OpenedLowerEyelidY, time, onFinish));
	}

	public void Close(float time, Action onFinish)
	{
		if (m_Coroutine != null)
			StopCoroutine(m_Coroutine);

		StartCoroutine(MoveEyelidsCoroutine(0.0f, 0.0f, time, onFinish));
	}

	private IEnumerator MoveEyelidsCoroutine(float upperGoal, float lowerGoal, float time, Action onFinish)
	{
		var upperStart = m_UpperEyelid.localPosition.y;
		var lowerStart = m_LowerEyelid.localPosition.y;

		for (var t = 0f; t < time; t += Time.deltaTime)
		{
			m_UpperEyelid.localPosition = Vector3.up * Mathf.Lerp(upperStart, upperGoal, t / time);
			m_LowerEyelid.localPosition = Vector3.up * Mathf.Lerp(lowerStart, lowerGoal, t / time);

			yield return null;
		}

		m_UpperEyelid.localPosition = Vector3.up * upperGoal;
		m_LowerEyelid.localPosition = Vector3.up * lowerGoal;

		onFinish();
	}

#if UNITY_EDITOR
	void Reset()
	{
		m_UpperEyelid = transform.FindChild("UpperEyelid").GetComponent<RectTransform>();
		m_LowerEyelid = transform.FindChild("LowerEyelid").GetComponent<RectTransform>();
	}
#endif
}
