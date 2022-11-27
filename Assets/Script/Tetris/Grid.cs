using System.Text;
using UnityEngine;

public class Grid : IGrid
{
	protected bool[,] InnerGrid;

	public Grid(int rowsCount, int columnsCount)
	{
		RowsCount = rowsCount;
		ColumnsCount = columnsCount;

		InnerGrid = new bool[RowsCount, ColumnsCount];
		for (int i = 0; i < RowsCount; i++) for (int j = 0; j < ColumnsCount; j++) EmptyBlock(new Vector2Int(j, i));
	}

	public override string ToString()
	{
		StringBuilder sb = new();
		for (int i = RowsCount - 1; i >= 0; i--)
		{
			for (int j = 0; j < ColumnsCount; j++) sb.Append($"\t {InnerGrid[i, j]}");
			sb.AppendLine();
		}
		return (sb.ToString());
	}
	public int RowsCount { get; protected set; }
	public int ColumnsCount { get; protected set; }

	public bool IsRowEmpty(int rowIndex)
	{
		for (int i = 0; i < ColumnsCount; i++) if (!IsBlockEmpty(new Vector2Int(i, rowIndex))) return (false);
		return (true);
	}
	public bool IsRowFull(int rowIndex)
	{
		for (int i = 0; i < ColumnsCount; i++) if (IsBlockEmpty(new Vector2Int(i, rowIndex))) return (false);
		return (true);
	}
	protected bool IsBlockEmpty(Vector2Int blockPosition) => InnerGrid[blockPosition.y, blockPosition.x];
	protected void FillBlock(Vector2Int blockPosition) => InnerGrid[blockPosition.y, blockPosition.x] = false;
	protected void EmptyBlock(Vector2Int blockPosition) => InnerGrid[blockPosition.y, blockPosition.x] = true;
	public void EmptyRow(int rowIndex)
	{
		for (int i = 0; i < ColumnsCount; i++) EmptyBlock(new Vector2Int(i, rowIndex));
	}
	public void MoveRow(int sourceRowIndex, int targetRowIndex)
	{
		for (int i = 0; i < ColumnsCount; i++) InnerGrid[targetRowIndex, i] = InnerGrid[sourceRowIndex, i];
		EmptyRow(sourceRowIndex);
	}
	public void Add(ITetromino tetros)
	{
		for (int i = 0; i < tetros.Blocks.Length; i++) FillBlock(tetros.Position + tetros.Blocks[i]);
	}
	public bool IsCollideGrid(ITetromino tetros)
	{
		for (int i = 0; i < tetros.Blocks.Length; i++)
		{
			Vector2Int blockPosition = tetros.Position + tetros.Blocks[i];
			if (!IsInGrid(tetros) || !IsBlockEmpty(blockPosition)) return (true);
		}
		return (false);
	}
	public bool IsInGrid(ITetromino tetros)
	{
		for (int i = 0; i < tetros.Blocks.Length; i++)
		{
			Vector2Int blockPosition = tetros.Position + tetros.Blocks[i];
			if ((blockPosition.x < 0) || (blockPosition.x >= ColumnsCount) || (blockPosition.y < 0) || (blockPosition.y >= RowsCount)) return (false);
		}
		return (true);
	}
}
