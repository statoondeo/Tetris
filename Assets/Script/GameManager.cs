using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	[SerializeField] protected GameObject BlockPrefab;
	[SerializeField] protected RectTransform PlayCanvas;
	[SerializeField] protected RectTransform NextTetromino;
	[SerializeField] protected TextMeshProUGUI ScoreText;
	[SerializeField] protected TextMeshProUGUI LinesText;
	[SerializeField] protected TextMeshProUGUI LevelText;
	[SerializeField] protected Transform GridContainer;
	[SerializeField] protected Transform TetrosContainer;

	protected IGame Game;
	protected float BlockSize;
	protected Vector2 GridBasePosition;
	protected Vector2 GridNextPosition;
	protected RectTransform[] NextBlocks;
	protected RectTransform[] CurrentBlocks;
	protected RectTransform[,] AnchoredBlocks;
	protected UserControls UserControls;
	protected Vector2Int Move;

	protected void Awake()
	{
		UserControls = new UserControls();
		UserControls.InGame.Move.started += ctx => Move = Vector2Int.RoundToInt(ctx.ReadValue<Vector2>());
		UserControls.InGame.Move.canceled += ctx => Move = Vector2Int.zero;

		IRandomizer bag = new DoubleBagRandomizer(
					new Tetromino(TetrominoType.I, Matrices.Get(TetrominoType.I)), new Tetromino(TetrominoType.I, Matrices.Get(TetrominoType.I)),
					new Tetromino(TetrominoType.J, Matrices.Get(TetrominoType.J)), new Tetromino(TetrominoType.J, Matrices.Get(TetrominoType.J)),
					new Tetromino(TetrominoType.L, Matrices.Get(TetrominoType.L)), new Tetromino(TetrominoType.L, Matrices.Get(TetrominoType.L)),
					new Tetromino(TetrominoType.O, Matrices.Get(TetrominoType.O)), new Tetromino(TetrominoType.O, Matrices.Get(TetrominoType.O)),
					new Tetromino(TetrominoType.S, Matrices.Get(TetrominoType.S)), new Tetromino(TetrominoType.S, Matrices.Get(TetrominoType.S)),
					new Tetromino(TetrominoType.T, Matrices.Get(TetrominoType.T)), new Tetromino(TetrominoType.T, Matrices.Get(TetrominoType.T)),
					new Tetromino(TetrominoType.Z, Matrices.Get(TetrominoType.Z)), new Tetromino(TetrominoType.Z, Matrices.Get(TetrominoType.Z)));
		Game = new Game(1.0f, 0, 0, 0, new BasicSpeedStrategy(), new BasicScoreStrategy(), new BasicLevelStrategy(), new Grid(20, 10), bag);
		BlockSize = BlockPrefab.GetComponentInChildren<Image>().rectTransform.sizeDelta.x;
		GridBasePosition = new Vector2(0.5f * BlockSize * (1 - Game.ColumnsCount), 0.5f * BlockSize * (1 - Game.RowsCount));
		GridNextPosition = PlayCanvas.rect.position + 0.5f * NextTetromino.rect.size;
		AnchoredBlocks = new RectTransform[Game.RowsCount, Game.ColumnsCount];
	}
	protected void OnEnable() => UserControls.InGame.Enable();
	protected void OnDisable() => UserControls.InGame.Disable();
	protected void Start()
	{
		Game.CurrentTetrosChanged += CreateCurrentBlocks;
		Game.TetrosControlled += DrawTetros;
		Game.TetrosMoved += DrawTetros;
		Game.LineCleared += LineCleared;
		Game.LineMoved += LineMoved;
		Game.TetrosAnchored += TetrosAnchored;
		Game.LinesUpdated += (lines) => { LinesText.text = lines.ToString(); };
		Game.ScoreUpdated += (score) => { ScoreText.text = score.ToString(); };
		Game.LevelUpdated += (level) => { LevelText.text = level.ToString(); };
		DrawGrid();
		Game.Start();
	}
	protected void Update() => Game.Update(Time.deltaTime, Move);
	protected void DrawGrid()
	{
		for (int i = 0; i < Game.RowsCount; i++)
			for (int j = 0; j < Game.ColumnsCount; j++)
				Instantiate(BlockPrefab, Vector3.zero, Quaternion.identity, GridContainer)
					.GetComponent<RectTransform>().anchoredPosition = GetCanvasPosition(new Vector2Int(j, i)); 
	}
	protected void DrawTetros()
	{
		for (int i = 0; i < Game.Current.Blocks.Length; i++)
			CurrentBlocks[i].anchoredPosition = GetCanvasPosition(Game.Current.Position + Game.Current.Blocks[i]);
	}
	protected void TetrosAnchored()
	{
		for (int i = 0; i < CurrentBlocks.Length; i++)
		{
			Vector2Int gridPosition = GetGridPosition(CurrentBlocks[i].anchoredPosition);
			AnchoredBlocks[gridPosition.y, gridPosition.x] = CurrentBlocks[i];
		}
	}
	protected void LineMoved(int from, int to)
	{
		for (int i = 0; i < Game.ColumnsCount; i++)
		{
			if (null != AnchoredBlocks[from, i]) AnchoredBlocks[from, i].anchoredPosition = GetCanvasPosition(new Vector2Int(i, to));
			AnchoredBlocks[to, i] = AnchoredBlocks[from, i];
			AnchoredBlocks[from, i] = null;
		}
	}
	protected void LineCleared(IList<int> lines)
	{
		for (int i = 0; i < lines.Count; i++)
			for (int j = 0; j < Game.ColumnsCount; j++)
			{
				Destroy(AnchoredBlocks[lines[i], j].gameObject);
				AnchoredBlocks[lines[i], j] = null;
			}
	}
	protected Vector2 GetCanvasPosition(Vector2Int gridPosition) => GridBasePosition + BlockSize * gridPosition.ConvertTo<Vector2>();
	protected Vector2Int GetGridPosition(Vector2 canvasPosition) => (Vector2Int.RoundToInt((canvasPosition - GridBasePosition) / BlockSize));
	protected RectTransform[] CreateNextBlocks()
	{
		ITetromino next = Game.Next;
		RectTransform[] nextBlocks = new RectTransform[next.Blocks.Length];
		Vector2 basePosition = GridNextPosition - 0.5f * BlockSize * (next.Size.x * Vector2.right + next.Size.y * Vector2.up);
		for (int i = 0; i < next.Blocks.Length; i++)
		{
			GameObject go = Instantiate(BlockPrefab, Vector3.zero, Quaternion.identity, GridContainer);
			nextBlocks[i] = go.GetComponent<RectTransform>();
			nextBlocks[i].anchoredPosition = basePosition + BlockSize * new Vector2(next.Blocks[i].x, next.Blocks[i].y);
			go.transform.Find("Block").GetComponent<Image>().color = TetrominoColors.Get(next.Type).fore;
			go.transform.Find("Background").GetComponent<Image>().color = TetrominoColors.Get(next.Type).back;
		}
		return (nextBlocks);
	}
	protected void CreateCurrentBlocks()
	{
		CurrentBlocks = NextBlocks ?? CreateNextBlocks();
		for (int i = 0; i < Game.Current.Blocks.Length; i++)
		{
			//CurrentBlocks[i].transform.SetParent(GridContainer);
			CurrentBlocks[i].anchoredPosition = GetCanvasPosition(Game.Current.Position + Game.Current.Blocks[i]);
		}
		NextBlocks = CreateNextBlocks();
	}
}
