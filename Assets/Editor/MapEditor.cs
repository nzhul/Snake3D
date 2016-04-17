using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapManager))]
public class MapEditor : Editor
{

	public override void OnInspectorGUI()
	{
		MapManager map = target as MapManager;

		if (DrawDefaultInspector())
		{
			map.GenerateMap();
		}

		if (GUILayout.Button("Generate Map"))
		{
			map.GenerateMap();
		}
	}
}
