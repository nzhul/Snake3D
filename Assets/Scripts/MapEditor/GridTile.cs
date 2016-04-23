using UnityEngine;
using System.Collections;

public class GridTile : MonoBehaviour {

	public Coord position;

	void OnMouseDown()
	{
		Debug.Log(string.Format("Tile Clicked at: x:{0} y:{1}", this.position.x, this.position.y));
	}

	//TODO: 
	// Add UI controls for editing the map properties: width, height, choosing obstacle prefabs and so on
	// Add UI button for saving the current map grid as bool[,] with 1 for obstacle and 0 for clear
	// Save the grid matrix in file
	// Add option in the game menu to choose "Use Custom Map"
	// If the player choose - custom map - he must be presented with list of all maps and choose the map by name
	
	// OVERAL CONCEPT:
	// The player must be able to create/edit custom maps and save them
	// later he can choose to play custom map


}