using UnityEngine;

public interface ITetromino
{
	Vector2Int[] Blocks { get; }
	int CurrentMatrix { get; set; }
	Vector2Int Position { get; }
	Vector2Int Size { get; }
	TetrominoType Type { get; }

	ITetromino Clone();
	ITetromino Move(Vector2Int delta);
	ITetromino MoveTo(Vector2Int position);
	ITetromino Rotate(Vector2Int move);
}