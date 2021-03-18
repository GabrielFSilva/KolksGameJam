using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class LevelCreator
{
    [MenuItem("Level/Generate Empty Level")]
    public static void GenerateEmptyLevel()
    {
        GameObject editorLevellGO = new GameObject("EditorLevel", typeof(EditorLevel));
        EditorLevel editorLevel = editorLevellGO.GetComponent<EditorLevel>();

        GameObject levelGO = new GameObject("Level");
        levelGO.transform.SetParent(editorLevellGO.transform);

        GameObject layer1GO = new GameObject("Entities");
        GameObject layer2GO = new GameObject("Tiles");
        GameObject layer3GO = new GameObject("Scenery");

        layer1GO.transform.SetParent(levelGO.transform);
        layer2GO.transform.SetParent(levelGO.transform);
        layer3GO.transform.SetParent(levelGO.transform);

        editorLevel.LayersRoots = new List<Transform> { layer1GO.transform, layer2GO.transform, layer3GO.transform};
        editorLevel.Setup(new Level(5, 5, 1f));
    }

    [MenuItem("Level/Delete Level")]
    public static void DeleteLevel()
    {
        try
        {
            GameObject editorLevelGO = GameObject.Find("EditorLevel");
            if (editorLevelGO)
            {
                EditorLevel gameLevel = editorLevelGO.GetComponent<EditorLevel>();
                GameObject.DestroyImmediate(gameLevel.gameObject);
            }
        }
        catch (Exception e)
        {
            Debug.Log($"Cannot delete EditorLevel GameObject, {e}");
        }
    }

    public static void GenerateLevel()
    {
        GameObject editorLevellGO = new GameObject("EditorLevel", typeof(EditorLevel));
        EditorLevel editorLevel = editorLevellGO.GetComponent<EditorLevel>();

        GameObject levelGO = new GameObject("Level");
        levelGO.transform.SetParent(editorLevellGO.transform);

        GameObject layer1GO = new GameObject("Entities");
        GameObject layer2GO = new GameObject("Tiles");
        GameObject layer3GO = new GameObject("Scenery");

        layer1GO.transform.SetParent(levelGO.transform);
        layer2GO.transform.SetParent(levelGO.transform);
        layer3GO.transform.SetParent(levelGO.transform);

        editorLevel.LayersRoots = new List<Transform> { layer1GO.transform, layer2GO.transform, layer3GO.transform };
    }

    public static void HideLayers(List<bool> layersStates)
    {
        GameObject editorLevelGO = GameObject.Find("EditorLevel");
        if(editorLevelGO)
        {
            EditorLevel gameLevel = editorLevelGO.GetComponent<EditorLevel>();
            if(gameLevel)
            {
                for (int i = 0; i < layersStates.Count; i++)
                {
                    gameLevel.LayersRoots[i].gameObject.SetActive(layersStates[i]);
                }
            }
        }
    }

    public static Transform GetLayerTransform(int selectedLayer)
    {
        EditorLevel editorLevel = GameObject.Find("EditorLevel").GetComponent<EditorLevel>();
        return editorLevel.LayersRoots[selectedLayer];
    }

    public static void SaveLevel(string fileName)
    {
        string path = $"Assets/Resources/LevelEditor/Levels/{fileName}";
        try
        {
            EditorLevel editorLevel = GameObject.Find("EditorLevel").GetComponent<EditorLevel>();
            List<Layer> layers = new List<Layer>();

            for (int index = 0; index < editorLevel.LayersRoots.Count; index++)
            {
                List<Tile> tiles = new List<Tile>();
                List<Transform> childs = editorLevel.LayersRoots[index].Cast<Transform>().ToList();
                // make sure to set the layer active 
                editorLevel.LayersRoots[index].gameObject.SetActive(true);

                foreach (var child in childs)
                {
                    //Entity entity = child.GetComponent<Entity>();
                    //tiles.Add(new Tile(entity.transform.position, entity.TileType, entity.PrefabName));
                }
                layers.Add(new Layer(editorLevel.LayersRoots[index].name, index, tiles));
            }
            editorLevel.Level.Layers = layers;
            
            string json = JsonUtility.ToJson(editorLevel.Level);
            File.WriteAllText(path, json);
        }
        catch (Exception e)
        {
            Debug.Log($"Cannot save {path} file, {e}");
        }
    }

    public static void LoadLevel(string fileName, Action<EditorLevel> callback)
    {
        string path = $"Assets/Resources/LevelEditor/Levels/{fileName}";
        try
        {
            GameObject editorLevelGO = GameObject.Find("EditorLevel");
            EditorLevel editorLevel = editorLevelGO.GetComponent<EditorLevel>();

            string content = File.ReadAllText(path);

            editorLevel.Level = JsonUtility.FromJson<Level>(content);
            callback?.Invoke(editorLevel);
            //gameLevel.BuildLevel();
        }
        catch (Exception e)
        {
            Debug.Log($"Cannot read {path} file, {e}");
        }
    }

    public static void GenerateLoadLevel(string fileName)
    {
        DeleteLevel();
        GenerateLevel();
        LoadLevel(fileName, (editorLevel) =>
        {
            editorLevel.Setup();
        });
    }

    public static GameObject GetPrefabByName(string name)
    {
        return Resources.Load<GameObject>($"Prefabs/{name}");
    }
}
