using System.Collections;
using System.Collections.Generic;
using TerrainGen;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapPreview))]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MapPreview mapPreview = (MapPreview)target;

        if (DrawDefaultInspector())
        {
            if (mapPreview.AutoUpdate)
            {
                mapPreview.DrawMapInEditor();
            }
        }

        if (GUILayout.Button("Generate"))
        {
            mapPreview.DrawMapInEditor();
        }

    }
	
}
