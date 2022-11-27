public class BasicLevelStrategy : ILevelStrategy
{
	public int GetLevel(int level, int score, int lines) => lines / 10;
}