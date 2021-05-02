using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.AI;
public class GridCreator : MonoBehaviour
{
    private bool isBlocked = false;

    public Vector2 GridSize = new Vector2(16, 16);
    public Vector2 GridCellSize = new Vector2(1, 1);

    public GameObject GridObject;
    private NavMeshHit hit;

    [ShowInInspector]
    private List<Cell> Grid = new List<Cell>();
    [ShowInInspector]
    private bool gridToggle = false;

    NavMeshPath NavMeshPath;

    public NavMeshAgent NavMeshAgent;

    private void FixedUpdate()
    {
        if (Grid.Count == 0) return;

        foreach (Cell cell in Grid)
        {
            if (!cell.IsActive) break;
                cell.ToggleHighlight(Vector3.Distance(MouseInputManager.Instance.MousePosition, cell.transform.position) < 0.5f);
        }
    }

    [Button]
    public void ToggleGrid()
    {
        if (Grid.Count == 0) return;

        gridToggle = !gridToggle;

        foreach (Cell cell in Grid)
        {
            cell.ToggleGraphics(gridToggle);
        }
        NavMeshAgent.Warp(Grid[0].transform.position);
        NavMeshPath = new NavMeshPath();
        NavMeshAgent.CalculatePath(Grid[Grid.Count - 1].transform.position, NavMeshPath);
        
    }

    [Button]
    public void ClearGrid()
    {
        gridToggle = false;

        foreach (Cell gridObject in Grid)
        {
            DestroyImmediate(gridObject.gameObject);
        }
        Grid.Clear();

    }

    private void OnDrawGizmos()
    {
        if (NavMeshPath == null) return;
        Gizmos.color = Color.red;
        for (int i = 0; i < NavMeshPath.corners.Length; i++)
        {
            if (i == 0)
                Gizmos.DrawLine(NavMeshPath.corners[0], NavMeshPath.corners[1]);
            else if (i == NavMeshPath.corners.Length - 1)
                Gizmos.DrawSphere(NavMeshPath.corners[i], 0.25f);
            else
                Gizmos.DrawLine(NavMeshPath.corners[i], NavMeshPath.corners[i+1]);

        }
    }

    [Button]
    public void InitializeGrid()
    {
        ClearGrid();

        Vector3 PointZero = transform.position - new Vector3((GridCellSize.x/2) + (GridCellSize.x * (GridSize.x/2)), -0.01f, GridCellSize.y / 2 + GridCellSize.y * GridSize.y / 2);

        Vector3 TargetPosition = PointZero;
        Vector3 LocalPosition = PointZero;
        Debug.Log(PointZero);
        for (int i = 1; i < GridSize.x+1; i++)
        {
            for (int j = 1; j < GridSize.y+1; j++)
            {
                TargetPosition = PointZero + new Vector3(GridCellSize.x * i, 0, GridCellSize.y * j);
                isBlocked = NavMesh.SamplePosition(TargetPosition, out hit, 0.5f, NavMesh.AllAreas);
                if (isBlocked)
                {
                    GameObject gridObject = Instantiate(GridObject, TargetPosition, Quaternion.identity);
                    //gridObject.transform.rotation = Quaternion.Euler(90, 0, 0);
                    gridObject.name = "Cell["+i+"]["+j+"]";
                    Grid.Add(gridObject.GetComponent<Cell>());
                }
                LocalPosition = TargetPosition;
            }
        }
    }
}
