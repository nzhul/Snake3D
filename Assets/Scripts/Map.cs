using UnityEngine;

[System.Serializable]
public class Map
{
	public int mapWidth;
	public int mapHeight;

	[Range(0, 1)]
	public float obstaclePercent;
	public int seed;

	public float minObstacleHeight;
	public float maxObstacleHeight;

	public Color foregroundColor;
	public Color backgroundColor;

	public Coord mapCenter
	{
		get
		{
			return new Coord(mapWidth / 2, mapHeight / 2);
		}
	}
}