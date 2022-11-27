using UnityEngine;

public class BasicSpeedStrategy : ISpeedStrategy
{
	public float GetSpeed(int level) => Mathf.Pow(.92f, level);
}
