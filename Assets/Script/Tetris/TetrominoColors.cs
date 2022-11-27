using System.Collections.Generic;
using UnityEngine;

public static class TetrominoColors
{
	private static readonly IDictionary<TetrominoType, (Color fore, Color back)> Colors;
	static TetrominoColors()
	{
		Colors = new Dictionary<TetrominoType, (Color fore, Color back)>()
		{
			{ TetrominoType.I, (new Color(43 / 255f, 172 / 255f, 226 / 255f), new Color(0 / 255f, 122 / 255f, 206 / 255f)) },
			{ TetrominoType.J, (new Color(0 / 255f, 90 / 255f, 157 / 255f), new Color(0 / 255f, 0 / 255f, 115 / 255f)) },
			{ TetrominoType.L, (new Color(248 / 255f, 150 / 255f, 34 / 255f), new Color(180 / 255f, 87 / 255f, 0 / 255f)) },
			{ TetrominoType.O, (new Color(253 / 255f, 225 / 255f, 0 / 255f), new Color(239 / 255f, 170 / 255f, 0 / 255f)) },
			{ TetrominoType.S, (new Color(78 / 255f, 183 / 255f, 72 / 255f), new Color(0 / 255f, 153 / 255f, 0 / 255f)) },
			{ TetrominoType.Z, (new Color(238 / 255f, 39 / 255f, 51 / 255f), new Color(153 / 255f, 0 / 255f, 0 / 255f)) },
			{ TetrominoType.T, (new Color(146 / 255f, 43 / 255f, 140 / 255f), new Color(102 / 255f, 0 / 255f, 102 / 255f)) },
		};
	}
	public static (Color fore, Color back) Get(TetrominoType tetrosType) => Colors[tetrosType];
}
