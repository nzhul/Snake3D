using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GridManager))]
public class GridEditor : Editor
{
	public override void OnInspectorGUI()
	{
		GridManager map = target as GridManager;

		if (DrawDefaultInspector())
		{
			map.GenerateGrid();
		}

		if (GUILayout.Button("Generate Map"))
		{
			map.GenerateGrid();
		}
	}
}