using UnityEngine;
using System.Collections;

public static class PlayerColors
{
	public static Color Player1
	{
		get { return new Color32(50, 233, 50, 255); }
	}

	public static Color Player2
	{
		get { return new Color32 (223, 255, 64, 255); }
	}

	public static Color Player3
	{
		get { return new Color32 (126, 220, 255, 255); }
	}

	public static Color Get(int number)
	{
		switch (number)
		{
		case 0:
			return Player1;

		case 1:
			return Player2;

		case 2:
			return Player3;
		}

		return Color.white;
	}
}
