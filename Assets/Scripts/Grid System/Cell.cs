﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public GameObject Graphics;
    public GameObject Highlight;

    public bool IsActive = false;

    LayerMask LayerMask;

    public Cell NorthCell;
    public Cell SouthCell;
    public Cell EastCell;
    public Cell WestCell;

    private void Start()
    {
        LayerMask = LayerMask.GetMask("GridCell");
    }

    private void OnEnable()
    {
        StartCoroutine(WaitForInitialize());
    }

    private IEnumerator WaitForInitialize()
    {
        yield return new WaitForSeconds(0.01f);
        AssignNeighbors();
    }

    private void AssignNeighbors()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out hit, 1f, LayerMask))
        {
            NorthCell = hit.transform.GetComponent<Cell>();
        }

        ray = new Ray(transform.position, -transform.forward);

        if (Physics.Raycast(ray, out hit, 1f, LayerMask))
        {
            SouthCell = hit.transform.GetComponent<Cell>();
        }

        ray = new Ray(transform.position, transform.right);

        if (Physics.Raycast(ray, out hit, 1f, LayerMask))
        {
            WestCell = hit.transform.GetComponent<Cell>();
        }
        ray = new Ray(transform.position, -transform.right);

        if (Physics.Raycast(ray, out hit, 1f, LayerMask))
        {
            EastCell = hit.transform.GetComponent<Cell>();
        }
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawRay(new Ray(transform.position, transform.forward));
    //    Gizmos.DrawRay(new Ray(transform.position, -transform.forward));
    //    Gizmos.DrawRay(new Ray(transform.position, transform.right));
    //    Gizmos.DrawRay(new Ray(transform.position, -transform.right));
    //}

    public void ToggleHighlight(bool toggle)
    {
        Highlight.SetActive(toggle);
    }

    public void ToggleGraphics(bool toggle)
    {
        Graphics.SetActive(toggle);
        IsActive = toggle;
    }

}
