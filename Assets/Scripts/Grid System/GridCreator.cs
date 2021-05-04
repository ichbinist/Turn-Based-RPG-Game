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
    private List<Cell> GridPath = new List<Cell>();
    [ShowInInspector]
    private Cell SelectedCell;
    [ShowInInspector]
    private bool gridToggle = false;

    NavMeshPath NavMeshPath;

    public NavMeshAgent NavMeshAgent;

    private void OnEnable()
    {
        MouseInputManager.Instance.OnMouseClicked.AddListener(SelectCell);
        CharacterManager.Instance.Character.OnGridChange.AddListener(ClearPath);
    }

    private void OnDisable()
    {
        MouseInputManager.Instance.OnMouseClicked.RemoveListener(SelectCell);
        CharacterManager.Instance.Character.OnGridChange.RemoveListener(ClearPath);
    }

    private void ClearPath()
    {
        GridPath.Clear();
    }

    private void SelectCell(Vector3 clickedPosition)
    {
        GridPath.Clear();
 
        Debug.Log("Select Cell");

        foreach (Cell cell in Grid)
        {
            if (!cell.IsActive) break;
            AddSelectedCell(Vector3.Distance(clickedPosition, cell.transform.position) < 0.75f,cell);
            if (Vector3.Distance(clickedPosition, cell.transform.position) < 0.75f) break;
        }
    }

    private void AddSelectedCell(bool check,Cell cell)
    {
        if (check)
        {
            Debug.Log(Vector3.Distance(MouseInputManager.Instance.HitPosition, cell.transform.position));
            SelectedCell = cell;
        }
    }

    private void FixedUpdate()
    {
        if (Grid.Count == 0) return;

        foreach (Cell cell in Grid)
        {
            if (!cell.IsActive) break;
            cell.ToggleHighlight(Vector3.Distance(MouseInputManager.Instance.MousePosition, cell.transform.position) < 0.5f);
        }

        if (SelectedCell != null)
        {
            FindPath(CharacterManager.Instance.Character.CurrentCell, SelectedCell);
            SelectedCell.ToggleHighlight(true);
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

    }

    [Button]
    private void FindPath(Cell StartPosition,  Cell FinishPosition)
    {
        
        NavMeshAgent.Warp(Grid[Grid.IndexOf(StartPosition)].transform.position);
        NavMeshPath = new NavMeshPath();
        NavMeshAgent.CalculatePath(Grid[Grid.IndexOf(FinishPosition)].transform.position, NavMeshPath);

        for (int i = 0; i < NavMeshPath.corners.Length-1; i++)
        {
            RaycastHit[] hits;
            hits = Physics.RaycastAll(NavMeshPath.corners[i], NavMeshPath.corners[i+1] - NavMeshPath.corners[i], Vector3.Distance(NavMeshPath.corners[i], NavMeshPath.corners[i+1]), LayerMask.GetMask("GridCell"));
            System.Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance));
            for (int j = 0; j < hits.Length; j++)
            {
                RaycastHit hit = hits[j];
                Cell cell = hit.transform.GetComponent<Cell>();

                if (cell && !GridPath.Contains(cell))
                {
                    GridPath.Add(cell);
                }
            }
        }
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

        for (int i = 0; i < NavMeshPath.corners.Length-1; i++)
        {
                Gizmos.DrawLine(NavMeshPath.corners[i], NavMeshPath.corners[i+1]);
        }

        Gizmos.color = Color.blue;

        if(GridPath.Count != 0)
        {
            for (int i = 0; i < GridPath.Count-1; i++)
            {
                if(GridPath[i].Neighbors.Contains(GridPath[i + 1]))
                    Gizmos.DrawLine(GridPath[i].transform.position, GridPath[i+1].transform.position);
            }
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
