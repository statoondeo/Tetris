public class BasicScoreStrategy : IScoreStrategy
{
	public int GetPoints(int level, int linesCount) => (level + 1) * linesCount switch { 1 => 40, 2 => 100, 3 => 300, 4 => 1200, };
}
