using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;

public class LevelConstructorWindow : EditorWindow
{
    private float tileSize = 1.2f;
    private int rows = 2;
    private int columns = 2;
    private int loadIndex = 0;
    private int tabIndex = 0;
    private string saveFileName = "name";
    private bool[] hideLayers = new bool[3];
    private List<GameObject> tiles = new List<GameObject>();
    private string path = "Assets/Resources/Editor/Prefabs";
    private int prefabIndex;
    private bool paintMode = false;
    private Vector3 centerPosition;

    private int paintingIndex = 0;
    private string[] paintingNames = { "Entities", "Tiles", "Scenery" };
    private string[] paths = { "Assets/Resources/LevelEditor/Prefabs/Entities", "Assets/Resources/LevelEditor/Prefabs/Tiles", "Assets/Resources/LevelEditor/Prefabs/Scenery" };


    private void OnEnable()
    {
        if(EditorLevel.Instance)
        {
            hideLayers = new bool[EditorLevel.Instance.LayersRoots.Count];

            for (int i = 0; i < EditorLevel.Instance.LayersRoots.Count; i++)
            {
                hideLayers[i] = true;
            }
            LevelCreator.HideLayers(hideLayers.ToList());
            Repaint();
        }
    }

    private void OnDestroy()
    {
        if (EditorLevel.Instance)
        {
            for (int i = 0; i < EditorLevel.Instance.LayersRoots.Count; i++)
            {
                hideLayers[i] = true;
            }
            LevelCreator.HideLayers(hideLayers.ToList());
            Repaint();
        }
        SceneView.duringSceneGui -= this.OnSceneGUI;
    }
    private void OnGUI()
    {
        tabIndex = GUILayout.Toolbar(tabIndex, new string[] { "Creation", "Save", "Load" });

        if (EditorLevel.Instance && EditorLevel.Instance.Level != null)
        {
            rows = EditorLevel.Instance.Level.Rows;
            columns = EditorLevel.Instance.Level.Columns;
            tileSize = EditorLevel.Instance.Level.TileSize;

            Repaint();
        }

        switch (tabIndex)
        {
            case 0:
                EditorGUILayout.Space();
                rows = EditorGUILayout.IntField("Rows", rows);
                columns = EditorGUILayout.IntField("Columns", columns);
                tileSize = EditorGUILayout.FloatField("Tile Size", tileSize);

                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Hide Layers");

                hideLayers[0] = EditorGUILayout.Toggle("Entities", hideLayers[0]);
                hideLayers[1] = EditorGUILayout.Toggle("Tiles", hideLayers[1]);
                hideLayers[2] = EditorGUILayout.Toggle("Scenery", hideLayers[2]);

                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Prefabs");
                paintMode = GUILayout.Toggle(paintMode, "Start painting", "Button", GUILayout.Height(60f));

                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Paint in:");
                GUILayout.BeginHorizontal();

                paintingIndex = GUILayout.Toolbar(paintingIndex, paintingNames, GUILayout.Height(30f));

                GUILayout.EndHorizontal();
                EditorGUILayout.Space();

                List<GUIContent> tileIcons = new List<GUIContent>();
                foreach (GameObject prefab in tiles)
                {
                    Texture2D texture = AssetPreview.GetAssetPreview(prefab);
                    tileIcons.Add(new GUIContent(texture));
                }
                prefabIndex = GUILayout.SelectionGrid(prefabIndex, tileIcons.ToArray(), 6);

                break;
            case 1:
                EditorGUILayout.Space();
                saveFileName = EditorGUILayout.TextField("File Name:", saveFileName);
                if (GUILayout.Button("Save current level"))
                {
                    LevelCreator.SaveLevel(saveFileName);
                }
                break;
            case 2:
                EditorGUILayout.Space();
                loadIndex = EditorGUILayout.Popup(loadIndex, GetFileInPath().ToArray());
                if (GUILayout.Button("Generate and Load from file"))
                {
                    LevelCreator.GenerateLoadLevel(GetFileInPath()[loadIndex]);
                }
                break;
        }
        LevelCreator.HideLayers(hideLayers.ToList());
        RefreshPrefabs();
    }
    private void OnFocus()
    {
        SceneView.duringSceneGui -= this.OnSceneGUI;
        SceneView.duringSceneGui += this.OnSceneGUI;
    }

    private List<string> GetFileInPath()
    {
        var info = new DirectoryInfo("Assets/Resources/LevelEditor/Levels/");
        var filesInfo = info.GetFiles("*").Where(f => f.Extension != ".meta");

        List<string> fileNames = new List<string>();
        foreach (var file in filesInfo)
        {
            fileNames.Add(file.Name);
        }
        return fileNames;
    }
    private void DisplayVisualHelp()
    {
        Ray guiRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        Vector3 mousePosition = guiRay.origin - guiRay.direction * (guiRay.origin.z / guiRay.direction.z);
        centerPosition = mousePosition;
    }
    private void RefreshPrefabs()
    {
        tiles.Clear();

        string[] prefabFiles = Directory.GetFiles(paths[paintingIndex], "*.prefab");

        foreach (string prefabFile in prefabFiles)
            tiles.Add(AssetDatabase.LoadAssetAtPath(prefabFile, typeof(GameObject)) as GameObject);
    }
    private void OnSceneGUI(SceneView sceneView) 
    {
        if (paintMode)
        {
            Vector2 cellCenter = centerPosition;
            DisplayVisualHelp();
            HandleSceneViewInputs(cellCenter);
            sceneView.Repaint();
        }
    }
    private void HandleSceneViewInputs(Vector2 cellCenter)
    {
        // Filter the left click so that we can't select objects in the scene
        if (Event.current.type == EventType.Layout)
        {
            HandleUtility.AddDefaultControl(0); // Consume the event
        }

        if (prefabIndex < tiles.Count && Event.current.type == EventType.MouseDown && Event.current.button == 0 && prefabIndex != -1)
        {
            // Create the prefab instance while keeping the prefab link
            GameObject prefab = tiles[prefabIndex];
            GameObject gameObject = PrefabUtility.InstantiatePrefab(prefab, LevelCreator.GetLayerTransform(paintingIndex)) as GameObject;
            gameObject.transform.localPosition = new Vector3(cellCenter.x,cellCenter.y, 0);

            // Allow the use of Undo (Ctrl+Z, Ctrl+Y).
            Undo.RegisterCreatedObjectUndo(gameObject, "");
        }

        if(Event.current.type == EventType.MouseDown && Event.current.button == 1 && prefabIndex != -1)
        {
            prefabIndex = -1;
        }
    }
}

public class EditorWindows : EditorWindow
{
    [MenuItem("Level/Level Constructor")]
    private static void InitConstructorWindow()
    {
        LevelConstructorWindow window = (LevelConstructorWindow)GetWindow(typeof(LevelConstructorWindow));
        window.Show();
    }
}
