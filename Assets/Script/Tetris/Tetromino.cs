using UnityEngine;

public class Tetromino : ITetromino
{
	public TetrominoType Type { get; protected set; }
	public Vector2Int Position { get; protected set; }
	public Vector2Int Size => Matrices[CurrentMatrix].Size;
	public Vector2Int[] Blocks => Matrices[CurrentMatrix].Blocks;

	public int CurrentMatrix { get => Matrix; set => Matrix = value % Matrices.Length; }
	protected int Matrix;
	protected Matrix[] Matrices;

	public Tetromino(TetrominoType type, Matrix[] matrices)
	{
		Type = type;
		Matrices = matrices;
		Matrix = 0;
	}
	public ITetromino Clone()
	{
		return (new Tetromino(Type, Matrices)
		{
			Position = new Vector2Int(this.Position.x, this.Position.y),
			Matrix = this.Matrix
		});
	}
	public ITetromino Move(Vector2Int delta)
	{
		Position += delta;
		return (this);
	}
	public ITetromino MoveTo(Vector2Int position)
	{
		Position = position;
		return (this);
	}
	public ITetromino Rotate(Vector2Int move)
	{
		int matrixRotation = move.x > 0 ? 1 : (move.x < 0 ? Matrices.Length - 1 : 0);
		CurrentMatrix = (CurrentMatrix + matrixRotation) % Matrices.Length;
		return (this);
	}
}
