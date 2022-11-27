public interface IGrid
{
	int ColumnsCount { get; }
	int RowsCount { get; }

	void Add(ITetromino tetromino);
	void EmptyRow(int rowIndex);
	bool IsRowEmpty(int rowIndex);
	bool IsRowFull(int rowIndex);
	bool IsCollideGrid(ITetromino tetromino);
	bool IsInGrid(ITetromino tetromino);
	void MoveRow(int fromIndex, int toIndex);
	string ToString();
}