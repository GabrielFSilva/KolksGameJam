using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class EditorPaintBlock
{
    private string[] paths = { "Assets/Resources/LevelEditor/Prefabs/Entities", "Assets/Resources/LevelEditor/Prefabs/Tiles", "Assets/Resources/LevelEditor/Prefabs/Scenery" };
    private string[] paintingNames = { "Entities", "Tiles", "Scenery" };
    private int paintingIndex = 0;
    private int prefabIndex = 0;
    private Vector3 centerPosition;

    public List<GameObject> Tiles = new List<GameObject>();
    public bool IsPainting = false;

    public void RefreshPrefabs()
    {
        Tiles.Clear();

        string[] prefabFiles = Directory.GetFiles(paths[paintingIndex], "*.prefab");

        foreach (string prefabFile in prefabFiles)
            Tiles.Add(AssetDatabase.LoadAssetAtPath(prefabFile, typeof(GameObject)) as GameObject);
    }
    private void HandleSceneViewInputs(Vector2 cellCenter)
    {
        if (Event.current.type == EventType.Layout)
        {
            HandleUtility.AddDefaultControl(0);
        }

        if (prefabIndex < Tiles.Count && Event.current.type == EventType.MouseDown && Event.current.button == 0 && prefabIndex != -1)
        {
            GameObject prefab = Tiles[prefabIndex];
            GameObject gameObject = PrefabUtility.InstantiatePrefab(prefab, LevelCreator.GetLayerTransform(paintingIndex)) as GameObject;
            gameObject.transform.localPosition = new Vector3(cellCenter.x, cellCenter.y, 0);
            Undo.RegisterCreatedObjectUndo(gameObject, "");
        }

        if (Event.current.type == EventType.MouseDown && Event.current.button == 1 && prefabIndex != -1)
        {
            prefabIndex = -1;
        }
    }
    private void DisplayVisualHelp()
    {
        Ray guiRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        Vector3 mousePosition = guiRay.origin - guiRay.direction * (guiRay.origin.z / guiRay.direction.z);
        centerPosition = mousePosition;
    }
    public void Draw()
    {
        EditorGUILayout.BeginVertical("window");
            EditorGUILayout.BeginHorizontal();
                paintingIndex = GUILayout.Toolbar(paintingIndex, paintingNames, GUILayout.Height(30f));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();
            IsPainting = GUILayout.Toggle(IsPainting, "Start painting", "Button");
            List<GUIContent> tileIcons = new List<GUIContent>();
            foreach (GameObject prefab in Tiles)
            {
                Texture2D texture = AssetPreview.GetAssetPreview(prefab);
                tileIcons.Add(new GUIContent(texture));
            }
            prefabIndex = GUILayout.SelectionGrid(prefabIndex, tileIcons.ToArray(), 6);
        EditorGUILayout.EndVertical();
    }

    public void OnSceneGUI(SceneView sceneView)
    {
        if (IsPainting)
        {
            Vector2 cellCenter = centerPosition;
            DisplayVisualHelp();
            HandleSceneViewInputs(cellCenter);
            sceneView.Repaint();
        }
    }
}
public class EditorGridBlock
{
    private float tileSize = 1.2f;
    private int rows = 2;
    private int columns = 2;

    public void Draw()
    {
        EditorGUILayout.BeginVertical("window");
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Generate Grid")) { LevelCreator.GenerateEmptyLevel(); LevelCreator.UpdateLevel(rows, columns, tileSize); UnityEditorInternal.InternalEditorUtility.RepaintAllViews(); }
            if (GUILayout.Button("Delete Grid")) { LevelCreator.DeleteLevel(); UnityEditorInternal.InternalEditorUtility.RepaintAllViews(); }
            if (GUILayout.Button("Update Grid")) { LevelCreator.UpdateLevel(rows, columns, tileSize); UnityEditorInternal.InternalEditorUtility.RepaintAllViews(); }
            GUILayout.EndHorizontal();

            rows = EditorGUILayout.IntField("Rows", rows);
            columns = EditorGUILayout.IntField("Columns", columns);
            tileSize = EditorGUILayout.FloatField("Tile Size", tileSize);
        EditorGUILayout.EndVertical();
        EditorGUILayout.Space();
    }
}
public class EditorSaveGridBlock
{
    private string saveFileName = "name";

    public void Draw()
    {
        EditorGUILayout.Space();
        saveFileName = EditorGUILayout.TextField("File Name:", saveFileName);
        if (GUILayout.Button("Save current grid")) { LevelCreator.SaveLevel(saveFileName); }
    }
}
public class EditorLoadGridBlock
{
    private int loadIndex = 0;

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
    public void Draw()
    {
        EditorGUILayout.Space();
        loadIndex = EditorGUILayout.Popup(loadIndex, GetFileInPath().ToArray());
        if (GUILayout.Button("Generate and Load from file")) { LevelCreator.GenerateLoadLevel(GetFileInPath()[loadIndex]); }
    }
}

public class LevelConstructorWindow : EditorWindow
{
    private int tabIndex = 0;
    private EditorPaintBlock paintBlock;
    private EditorGridBlock gridBlock;
    private EditorSaveGridBlock gridSaveBlock;
    private EditorLoadGridBlock gridLoadBlock;


    [MenuItem("Level/Level Constructor")]
    private static void InitConstructorWindow()
    {
        LevelConstructorWindow window = (LevelConstructorWindow)GetWindow(typeof(LevelConstructorWindow));
        window.Show();
    }

    private void OnEnable()
    {
        gridBlock = new EditorGridBlock();
        paintBlock = new EditorPaintBlock();
        gridSaveBlock = new EditorSaveGridBlock();
        gridLoadBlock = new EditorLoadGridBlock();
    }
    private void OnDestroy()
    {
        SceneView.duringSceneGui -= this.OnSceneGUI;
    }
    private void OnGUI()
    {
        tabIndex = GUILayout.Toolbar(tabIndex, new string[] { "Creation", "Save", "Load" });
        EditorGUILayout.Space();
        switch (tabIndex)
        {
            case 0:
                gridBlock.Draw();
                paintBlock.Draw();
            break;
            case 1:
                gridSaveBlock.Draw();
            break;
            case 2:
                gridLoadBlock.Draw();
            break;
        }
        paintBlock.RefreshPrefabs();
        Repaint();
    }
    private void OnFocus()
    {
        SceneView.duringSceneGui -= this.OnSceneGUI;
        SceneView.duringSceneGui += this.OnSceneGUI;
    }
    private void OnSceneGUI(SceneView sceneView) 
    {
        paintBlock.OnSceneGUI(sceneView);
    }
}
