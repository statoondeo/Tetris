using System.Collections.Generic;
using UnityEngine;

public class Game : IGame
{
	public enum GameState { Waiting, Running, GameOver }

	protected static readonly float MaxControlTtl = 0.15f;

	protected IGrid Grid;
	protected IRandomizer Randomizer;
	protected GameState State;
	protected float ControlTtl;
	protected float GravityTtl;

	protected float Speed;
	protected int Score;
	protected int Level;
	protected int Lines;
	protected ISpeedStrategy SpeedStrategy;
	protected IScoreStrategy ScoreStrategy;
	protected ILevelStrategy LevelStrategy;

	public int RowsCount => Grid.RowsCount;
	public int ColumnsCount => Grid.ColumnsCount;
	public ITetromino Current { get; protected set; }
	public ITetromino Next { get; protected set; }

	public System.Action PlayerLoose { get; set; }
	public System.Action CurrentTetrosChanged { get; set; }
	public System.Action TetrosControlled { get; set; }
	public System.Action TetrosMoved { get; set; }
	public System.Action TetrosAnchored { get; set; }
	public System.Action<IList<int>> LineCleared { get; set; }
	public System.Action<int, int> LineMoved { get; set; }
	public System.Action<int> LinesUpdated { get; set; }
	public System.Action<int> LevelUpdated { get; set; }
	public System.Action<int> ScoreUpdated { get; set; }

	public Game(float speed, int score, int level, int lines, ISpeedStrategy speedStrategy, IScoreStrategy scoreStrategy, ILevelStrategy levelStrategy, IGrid tetrosGrid, IRandomizer randomizer)
	{
		Speed = speed;
		Score = score;
		Level = level;
		Lines = lines;
		SpeedStrategy = speedStrategy;
		ScoreStrategy = scoreStrategy;
		LevelStrategy = levelStrategy;
		Grid = tetrosGrid;
		Randomizer = randomizer;
		State = GameState.Waiting;
	}

	public void Start()
	{
		SpawnTetros();
		GravityTtl = Speed;
		ControlTtl = 0.0f;
		State = GameState.Running;
		ScoreUpdated?.Invoke(Score);
		LinesUpdated?.Invoke(Lines);
		LevelUpdated?.Invoke(Level);
	}
	public void ControlTetromino(Vector2Int move)
	{
		if (ControlTtl > 0.0f) return;
		ControlTtl = MaxControlTtl;
		ControlHMove(move * Vector2Int.right);
		ControlVMove(move * Vector2Int.up);
		TetrosControlled?.Invoke();
	}
	public void Update(float deltaTime, Vector2Int move)
	{
		if (State != GameState.Running) return;
		ControlTetromino(move);
		UpdateGravity(deltaTime);
		UpdateControl(deltaTime);
	}
	protected void UpdateGravity(float deltaTime)
	{
		GravityTtl -= deltaTime;
		if (GravityTtl > 0.0f) return;
		TriggerGravity();
		GravityTtl = Speed;
	}
	protected void UpdateControl(float deltaTime)
	{
		if (ControlTtl == 0.0f) return;
		ControlTtl -= deltaTime;
		if (ControlTtl > 0.0f) return;
		ControlTtl = 0.0f;
	}
	protected bool CanRotate()
	{
		ITetromino tetromino = Current.Clone();
		tetromino.Rotate(Vector2Int.right);
		tetromino.MoveTo(
			new Vector2Int(
				Mathf.Clamp(tetromino.Position.x, 0, ColumnsCount - tetromino.Size.x),
				Mathf.Clamp(tetromino.Position.y, 0, RowsCount - tetromino.Size.y)));
		return (!Grid.IsCollideGrid(tetromino));
	}
	protected void ControlUp()
	{
		if (!CanRotate()) return;
		Current.Rotate(Vector2Int.right);
		Current.MoveTo(
			new Vector2Int(
				Mathf.Clamp(Current.Position.x, 0, ColumnsCount - Current.Size.x),
				Mathf.Clamp(Current.Position.y, 0, RowsCount - Current.Size.y)));
	}
	protected void ControlDown()
	{
		if (Grid.IsCollideGrid(Current.Clone().Move(Vector2Int.down))) return;
		Current.Move(Vector2Int.down);
	}
	protected void ControlVMove(Vector2Int move)
	{
		if (move.y == 0) return;
		if (move.y > 0) ControlUp();
		else ControlDown();
	}
	protected void ControlHMove(Vector2Int move)
	{
		if ((move.x == 0) || Grid.IsCollideGrid(Current.Clone().Move(move))) return;
		Current.Move(move);
	}
	protected IList<int> ClearLines()
	{
		int line = 0;
		IList<int> linesCleared = new List<int>();
		while ((line < RowsCount) && !Grid.IsRowEmpty(line))
		{
			if (Grid.IsRowFull(line))
			{
				linesCleared.Add(line);
				Grid.EmptyRow(line);
			}
			line++;
		}
		if (linesCleared.Count > 0) UpdateLineCleared(linesCleared);
		return (linesCleared);
	}
	protected void UpdateLineCleared(IList<int> linesCleared)
	{
		LineCleared?.Invoke(linesCleared);
		UpdateLines(linesCleared.Count);
		UpdateScore(linesCleared.Count);
		UpdateLevel();
	}
	protected void MoveLines(IList<int> lines)
	{
		for (int i = lines.Count - 1; i >= 0; i--)
		{
			int sourceLine = lines[i] + 1;
			while ((sourceLine < RowsCount) && !Grid.IsRowEmpty(sourceLine))
			{
				MoveLine(sourceLine, sourceLine - 1);
				sourceLine++;
			}
		}
	}
	protected void MoveLine(int from, int to)
	{
		Grid.MoveRow(from, to);
		LineMoved?.Invoke(from, to);
	}
	protected void CheckLines() => MoveLines(ClearLines());
	protected void AnchorTetros()
	{
		Grid.Add(Current);
		TetrosAnchored?.Invoke();
	}
	protected void TriggerGravity()
	{
		if (Grid.IsCollideGrid(Current.Clone().Move(Vector2Int.down)))
		{
			AnchorTetros();
			CheckLines();
			SpawnTetros();
			if (Grid.IsCollideGrid(Current))
			{
				State = GameState.GameOver;
				PlayerLoose?.Invoke();
			}
		}
		else
		{
			Current.Move(Vector2Int.down);
			TetrosMoved?.Invoke();
		}
	}
	protected void UpdateLines(int lineCount)
	{
		Lines += lineCount;
		LinesUpdated?.Invoke(Lines);
	}
	protected void UpdateScore(int lineCount)
	{
		Score += ScoreStrategy.GetPoints(Level, lineCount);
		ScoreUpdated?.Invoke(Score);
	}
	protected void UpdateLevel()
	{
		int newLevel = LevelStrategy.GetLevel(Level, Score, Lines);
		if (Level == newLevel) return;
		Level = newLevel;
		Speed = SpeedStrategy.GetSpeed(Level);
		LevelUpdated?.Invoke(Level);
	}
	protected void SpawnTetros()
	{
		Current = Next ?? Randomizer.GetNext();
		Next = Randomizer.GetNext();
		Next.CurrentMatrix = UnityEngine.Random.Range(0, Current.Blocks.Length);
		Current.MoveTo(Vector2Int.RoundToInt(new Vector2(0.5f * (Grid.ColumnsCount - Current.Size.x), Grid.RowsCount)));
		while (!Grid.IsInGrid(Current)) Current.Move(Vector2Int.down);
		CurrentTetrosChanged?.Invoke();
	}
}
