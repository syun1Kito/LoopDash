using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class TeleporterTile : AnimatedTile
{
   






#if UNITY_EDITOR
    // The following is a helper that adds a menu item to create a RoadTile Asset
    [MenuItem("Assets/LoopDash/TeleporterTile")]
    public static void CreateTeleporterTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Teleporter Tile", "New Teleporter Tile", "Asset", "Save Road Tile", "Assets");
        if (path == "")
            return;
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<TeleporterTile>(), path);
    }
#endif
}