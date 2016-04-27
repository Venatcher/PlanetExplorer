using UnityEngine;
using System.Collections;
using WorldGeneration;

public class LevelManager : MonoBehaviour {

	[SerializeField]
	int MapHeight;
	[SerializeField]
	int MapWidtht;

	WorldGenerator worlGenerator;
	WorldGeneration.Tile[,] Tiles;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void UpdateNeighbors()
	{
		for (var x = 0; x < MapWidtht; x++)
		{
			for (var y = 0; y < MapHeight; y++)
			{
				WorldGeneration.Tile t = Tiles[x,y];
				
				t.Top = GetTop(t);
				t.Bottom = GetBottom (t);
				t.Left = GetLeft (t);
				t.Right = GetRight (t);
			}
		}
	}

	private WorldGeneration.Tile GetTop(WorldGeneration.Tile t)
	{
		return Tiles [t.X, MathHelper.Mod (t.Y - 1, MapHeight)];
	}
	private WorldGeneration.Tile GetBottom(WorldGeneration.Tile t)
	{
		return Tiles [t.X, MathHelper.Mod (t.Y + 1, MapHeight)];
	}
	private WorldGeneration.Tile GetLeft(WorldGeneration.Tile t)
	{
		return Tiles [MathHelper.Mod(t.X - 1, MapWidtht), t.Y];
	}
	private WorldGeneration.Tile GetRight(WorldGeneration.Tile t)
	{
		return Tiles [MathHelper.Mod (t.X + 1, MapWidtht), t.Y];
	}
}
