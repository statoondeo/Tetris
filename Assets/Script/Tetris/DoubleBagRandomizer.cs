using UnityEngine;

public class DoubleBagRandomizer : IRandomizer
{
	protected ITetromino[] Tetroses;
	protected int Size;
	protected int CurrentIndex;

	public DoubleBagRandomizer(params Tetromino[] tetroses)
	{
		Tetroses = tetroses;
		Size = Tetroses?.Length ?? 0;
		CurrentIndex = 0;
	}
	protected void Shuffle()
	{
		for (int i = 0; i < Size; i++)
		{
			int j = Random.Range(i, Tetroses.Length);
			(Tetroses[j], Tetroses[i]) = (Tetroses[i], Tetroses[j]);
		}
	}
	public ITetromino GetNext()
	{
		if (CurrentIndex == 0) Shuffle();
		CurrentIndex = (CurrentIndex + 1) % Size;
		return (Tetroses[CurrentIndex].Clone());
	}
}
