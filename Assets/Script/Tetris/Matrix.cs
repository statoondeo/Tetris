using UnityEngine;

public class Matrix
{
	public Matrix(Vector2Int[] blocks)
	{
		int minX = int.MaxValue;
		int minY = int.MaxValue;
		int maxX = 0;
		int maxY = 0;
		Blocks = blocks;
		for (int i = 0; i < blocks.Length; i++)
		{
			minX = Mathf.Min(blocks[i].x, minX);
			minY = Mathf.Min(blocks[i].y, minY);
			maxX = Mathf.Max(blocks[i].x, maxX);
			maxY = Mathf.Max(blocks[i].y, maxY);
		}
		Size = new Vector2Int(maxX - minX, maxY - minY) + Vector2Int.one;
	}

	public Vector2Int Size { get; protected set; }
	public Vector2Int[] Blocks { get; protected set; }
}
