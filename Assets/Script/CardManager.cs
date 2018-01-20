using System;
using UnityEngine;

public class CardManager : MonoBehaviour
{
	public const int CardNum = 9;

	private CardController[] m_Cards;

	private Vector3[] m_Positions;

	public static CardManager Instance
	{
		get
		{
#if UNITY_EDITOR
			if (instance == null && !Application.isPlaying)
			{
				instance = FindObjectOfType<CardManager>();
			}
#endif
			if (instance == null)
			{
				Debug.LogError("Instance is not found!");
			}
			return instance;
		}
	}
	private static CardManager instance = null;

	void Awake()
	{
		if (instance == null)
			instance = this;

		if (instance != this)
		{
			Debug.LogError("There are two instance!");
			DestroyImmediate(this);
		}
	}

	void Start()
	{
		m_Cards = FindObjectsOfType<CardController>();

		Array.Sort(m_Cards, (lhs, rhs) => lhs.number - rhs.number);

		Debug.Assert(m_Cards.Length == CardNum);

		m_Positions = new Vector3[CardNum];

		for (var i = 0; i < CardNum; ++i)
		{
			m_Positions[i] = m_Cards[i].transform.position;
		}
	}

	public int Founded()
	{
		int number = 0;

		foreach (var card in m_Cards)
		{
			if (card.active)
			{
				break;
			}
			else
			{
				number = card.number;
			}
		}

		return number;
	}

	public bool FoundedAll(int number)
	{
		foreach (var card in m_Cards)
		{
			if (card.number == number)
			{
				return true;
			}

			if (card.active)
			{
				return false;
			}
		}

		return true;
	}

	public void Shuffle(string indexes, string ids)
	{
		Debug.Assert(indexes.Length == CardNum);
		Debug.Assert(ids.Length == CardNum);

		for (var i = 0; i < CardNum; ++i)
		{
			m_Cards[i].transform.position = m_Positions[int.Parse(indexes.Substring(i, 1))];
			m_Cards[i].active = true;
			m_Cards [i].SetPlayerID (int.Parse(ids.Substring(i, 1)));
		}
	}

	public void Find(int number)
	{
		m_Cards[number - 1].active = false;
	}
}
