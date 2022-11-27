using System.Collections.Generic;
using UnityEngine;

public static class Matrices
{
	private static readonly IDictionary<TetrominoType, Matrix[]> InnerMatrices;
	static Matrices()
	{
		InnerMatrices = new Dictionary<TetrominoType, Matrix[]>()
		{
			{
				TetrominoType.I,
				new Matrix[4]
				{
						new Matrix(new Vector2Int[] { new Vector2Int(2, 0), new Vector2Int(2, 1), new Vector2Int(2, 2), new Vector2Int(2, 3) }),
						new Matrix(new Vector2Int[] { new Vector2Int(0, 2), new Vector2Int(1, 2), new Vector2Int(2, 2), new Vector2Int(3, 2) }),
						new Matrix(new Vector2Int[] { new Vector2Int(1, 0), new Vector2Int(1, 1), new Vector2Int(1, 2), new Vector2Int(1, 3) }),
						new Matrix(new Vector2Int[] { new Vector2Int(0, 1), new Vector2Int(1, 1), new Vector2Int(2, 1), new Vector2Int(3, 1) }),
				}
			},
			{
				TetrominoType.J,
				new Matrix[4]
				{
						new Matrix(new Vector2Int[] { new Vector2Int(1, 0), new Vector2Int(1, 1), new Vector2Int(1, 2), new Vector2Int(0, 0) }),
						new Matrix(new Vector2Int[] { new Vector2Int(0, 1), new Vector2Int(1, 1), new Vector2Int(2, 1), new Vector2Int(2, 0) }),
						new Matrix(new Vector2Int[] { new Vector2Int(1, 0), new Vector2Int(1, 1), new Vector2Int(1, 2), new Vector2Int(2, 2) }),
						new Matrix(new Vector2Int[] { new Vector2Int(0, 1), new Vector2Int(1, 1), new Vector2Int(2, 1), new Vector2Int(0, 2) }),
				}
			},
			{
				TetrominoType.L,
				new Matrix[4]
				{
						new Matrix(new Vector2Int[] { new Vector2Int(1, 0), new Vector2Int(1, 1), new Vector2Int(1, 2), new Vector2Int(2, 0) }),
						new Matrix(new Vector2Int[] { new Vector2Int(0, 1), new Vector2Int(1, 1), new Vector2Int(2, 1), new Vector2Int(2, 2) }),
						new Matrix(new Vector2Int[] { new Vector2Int(1, 0), new Vector2Int(1, 1), new Vector2Int(1, 2), new Vector2Int(0, 2) }),
						new Matrix(new Vector2Int[] { new Vector2Int(0, 1), new Vector2Int(1, 1), new Vector2Int(2, 1), new Vector2Int(0, 0) }),
				}
			},
			{
				TetrominoType.O,
				new Matrix[1]
				{
						new Matrix(new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(1, 1) }),
				}
			},
			{
				TetrominoType.S,
				new Matrix[2]
				{
						new Matrix(new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(1, 1), new Vector2Int(2, 1) }),
						new Matrix(new Vector2Int[] { new Vector2Int(0, 1), new Vector2Int(0, 2), new Vector2Int(1, 0), new Vector2Int(1, 1) }),
				}
			},
			{
				TetrominoType.T,
				new Matrix[4]
				{
						new Matrix(new Vector2Int[] { new Vector2Int(1, 0), new Vector2Int(1, 1), new Vector2Int(1, 2), new Vector2Int(2, 1) }),
						new Matrix(new Vector2Int[] { new Vector2Int(0, 1), new Vector2Int(1, 1), new Vector2Int(2, 1), new Vector2Int(1, 2) }),
						new Matrix(new Vector2Int[] { new Vector2Int(1, 0), new Vector2Int(1, 1), new Vector2Int(1, 2), new Vector2Int(0, 1) }),
						new Matrix(new Vector2Int[] { new Vector2Int(0, 1), new Vector2Int(1, 1), new Vector2Int(2, 1), new Vector2Int(1, 0) }),
				}
			},
			{
				TetrominoType.Z,
				new Matrix[2]
				{
						new Matrix(new Vector2Int[] { new Vector2Int(0, 1), new Vector2Int(1, 1), new Vector2Int(1, 0), new Vector2Int(2, 0) }),
						new Matrix(new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(0, 1), new Vector2Int(1, 1), new Vector2Int(1, 2) }),
				}
			},
		};
	}
	public static Matrix[] Get(TetrominoType tetrosType) => InnerMatrices[tetrosType];
}