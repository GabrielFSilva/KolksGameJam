using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EditorLevel : MonoBehaviour
{
    public static EditorLevel Instance;

    public Level Level;
    public List<Transform> LayersRoots;
    public EditorLevelGrid Grid;
    
    public Color RightPositionColor = new Color(0f, 1f, 0f, 0.5f);
    public Color WrongPositionColor = new Color(1f, 0f, 0f, 0.5f);

    private void InstantiateTiles()
    {
        foreach (Layer layer in Level.Layers)
        {
            for (int index = 0; index < layer.Tiles.Count; index++)
            {
                //if (layer.Tiles[index].TileType != TileType.None)
                //{
                //    GameObject.Instantiate(LevelCreator.GetPrefabByName(layer.Tiles[index].PrefabName),
                //        layer.Tiles[index].Position,
                //        Quaternion.identity,
                //        LayersRoots[layer.LayerId]);
                //}
            }
        }
    }

    private void OnDrawGizmos()
    {
        if(Grid != null && Level != null)
            Grid.Draw();
    }

    public void Setup()
    {
        Instance = this;
        Grid = new EditorLevelGrid(transform, Level.Columns, Level.Rows, Level.TileSize);
        InstantiateTiles();
    }

    public void Setup(Level level)
    {
        Instance = this;
        Level = level;
        Grid = new EditorLevelGrid(transform, Level.Columns, Level.Rows, Level.TileSize);
    }
}

public class EditorLevelGrid
{
    private Transform transform;

    private int columns;
    private int rows;
    private float tileSize;

    private Color normalColor = Color.grey;

    public EditorLevelGrid(Transform transform, int columns, int rows, float tileSize)
    {
        this.transform = transform;
        this.columns = columns;
        this.rows = rows;
        this.tileSize = tileSize;
    }

    public void Draw()
    {
        Color oldColor = Gizmos.color;
        Matrix4x4 oldMatrix = Gizmos.matrix;
        Gizmos.matrix = transform.localToWorldMatrix;

        Gizmos.color = normalColor;
        GridGizmo(columns, rows);
        GridFrameGizmo(columns, rows);

        Gizmos.color = oldColor;
        Gizmos.matrix = oldMatrix;
    }

    private void GridFrameGizmo(int cols, int rows)
    {
        Gizmos.DrawLine(new Vector3(0, 0, 0), new Vector3(0, rows * tileSize, 0));
        Gizmos.DrawLine(new Vector3(0, 0, 0), new Vector3(cols * tileSize, 0, 0));
        Gizmos.DrawLine(new Vector3(cols * tileSize, 0, 0), new Vector3(cols * tileSize, rows * tileSize, 0));
        Gizmos.DrawLine(new Vector3(0, rows * tileSize, 0), new Vector3(cols * tileSize, rows * tileSize, 0));
    }
    private void GridGizmo(int cols, int rows)
    {
        for (int i = 1; i < cols; i++)
        {
            Gizmos.DrawLine(new Vector3(i * tileSize, 0, 0), new Vector3(i * tileSize, rows * tileSize, 0));
        }
        for (int j = 1; j < rows; j++)
        {
            Gizmos.DrawLine(new Vector3(0, j * tileSize, 0), new Vector3(cols * tileSize, j * tileSize, 0));
        }
    }

    public Vector3 WorldToGridCoordinates(Vector3 point)
    {
        Vector3 gridPoint = new Vector3(
        (int)((point.x - transform.position.x) / tileSize),
        (int)((point.y - transform.position.y) / tileSize), 0.0f);
        return gridPoint;
    }
    public Vector3 GridToWorldCoordinates(int col, int row)
    {
        Vector3 worldPoint = new Vector3(
        transform.position.x + (col * tileSize + tileSize / 2.0f),
        transform.position.y + (row * tileSize + tileSize / 2.0f),
        0.0f);
        return worldPoint;
    }
    public bool IsInsideGridBounds(Vector3 point)
    {
        float minX = transform.position.x;
        float maxX = minX + columns * tileSize;
        float minY = transform.position.y;
        float maxY = minY + rows * tileSize;
        return (point.x > minX && point.x < maxX && point.y > minY &&
       point.y < maxY);
    }
    public Vector3 KeepInsideGridBounds(Vector3 position)
    {
        float minX = transform.position.x;
        float maxX = minX + columns * tileSize;
        float minY = transform.position.y;
        float maxY = minY + rows * tileSize;

        return new Vector3(Mathf.Clamp(position.x, minX, maxX),
            Mathf.Clamp(position.y, minY, maxY),
            position.z);
    }
}
