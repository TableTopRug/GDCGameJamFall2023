using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class AlleyMapGenerator : MonoBehaviour
{
    // Tilemap you draw onto
    public Tilemap tilemap;
    // Tile to draw (use RuleTile)
    public TileBase tile;

    // Width of map
    public int width;
    // Height of map
    public int height;

    // ?
    public MapSettings mapSettings;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            ClearMap();
            GenerateMap();
        }
    }

    [ExecuteInEditMode]
    public void GenerateMap()
    {
        ClearMap();
        int[,] map = new int[width, height];
        float seed;
        if (mapSetting.randomSeed)
        {
            seed = Time.time;
        }
        else
        {
            seed = mapSetting.seed;
        }

        switch (mapSetting.algorithm)
        {
            case Algorithm.DirectionalTunnel:
                map = MapFunctions.GenerateArray(width, height, false);
                map = MapFunctions.DirectionalTunnel(map, mapSetting.minPathWidth, mapSetting.maxPathWidth, mapSetting.maxPathChange, mapSetting.roughtness, mapSetting.windyness);
                break;
        }

        MapFunctions.RenderMap(map, tilemap, tile);
    }

    public void ClearMap()
    {
        tilemap.ClearAllTiles();
    }
}


[CustomEditor(typeof(AlleyMapGenerator))]
public class AlleyMapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        AlleyMapGenerator levelGen = (AlleyMapGenerator)target;

        if (levelGen.mapSetting != null)
        {
            Editor mapSettingEditor = CreateEditor(levelGen.mapSetting);
            mapSettingEditor.OnInspectorGUI();

            if (GUILayout.Button("Generate"))
            {
                levelGen.GenerateMap();
            }

            if (GUILayout.Button("Clear"))
            {
                levelGen.ClearMap();
            }
        }
    }
}
