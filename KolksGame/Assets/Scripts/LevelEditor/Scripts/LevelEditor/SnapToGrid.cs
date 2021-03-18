using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class SnapToGrid : MonoBehaviour
{
    private void Update()
    {
        if(EditorLevel.Instance)
        {
            Vector3 gridCoord = EditorLevel.Instance.Grid.WorldToGridCoordinates(transform.position);
            transform.position = new Vector3(
                EditorLevel.Instance.Grid.GridToWorldCoordinates((int)gridCoord.x, (int)gridCoord.y).x,
                EditorLevel.Instance.Grid.GridToWorldCoordinates((int)gridCoord.x, (int)gridCoord.y).y,
                transform.position.z);
            transform.position = EditorLevel.Instance.Grid.KeepInsideGridBounds(transform.position);
        }
    }

    private void OnDrawGizmos()
    {
        if(Selection.activeGameObject == gameObject)
        {
            if(EditorLevel.Instance)
            {
                bool isInsideGridBounds = EditorLevel.Instance.Grid.IsInsideGridBounds(transform.position);
                Gizmos.color = isInsideGridBounds ? EditorLevel.Instance.RightPositionColor : EditorLevel.Instance.WrongPositionColor;
                Gizmos.DrawCube(transform.position, Vector3.one * EditorLevel.Instance.Level.TileSize);
            }
        }
    }
}
