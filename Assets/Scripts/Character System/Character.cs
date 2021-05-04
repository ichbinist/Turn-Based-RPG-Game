using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
public class Character : MonoBehaviour
{
    public Cell CurrentCell;

    LayerMask LayerMask;

    public UnityEvent OnGridChange = new UnityEvent();

    private void Start()
    {
        LayerMask = LayerMask.GetMask("GridCell");
    }

    private void OnEnable()
    {
        CharacterManager.Instance.Character = this;

    }

    private void OnDisable()
    {
        CharacterManager.Instance.Character = null;

    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position + transform.up,- transform.up);

        if (Physics.Raycast(ray, out hit, 1f, LayerMask))
        {
            Cell cell = hit.transform.GetComponent<Cell>();
            if (CurrentCell != cell)
            {
                CurrentCell = hit.transform.GetComponent<Cell>();
                OnGridChange.Invoke();
            }    
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + Vector3.up * 2f, transform.position-transform.up);
    }
}
