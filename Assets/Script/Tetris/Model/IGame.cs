using System;
using System.Collections.Generic;
using UnityEngine;

public interface IGame
{
	int ColumnsCount { get; }
	ITetromino Current { get; }
	ITetromino Next { get; }
	int RowsCount { get; }

	void ControlTetromino(Vector2Int move);
	void Start();
	void Update(float deltaTime, Vector2Int move);

	Action PlayerLoose { get; set; }
	Action CurrentTetrosChanged { get; set; }
	Action TetrosControlled { get; set; }
	Action TetrosMoved { get; set; }
	Action TetrosAnchored { get; set; }
	Action<IList<int>> LineCleared { get; set; }
	Action<int, int> LineMoved { get; set; }
	Action<int> LinesUpdated { get; set; }
	Action<int> LevelUpdated { get; set; }
	Action<int> ScoreUpdated { get; set; }
}