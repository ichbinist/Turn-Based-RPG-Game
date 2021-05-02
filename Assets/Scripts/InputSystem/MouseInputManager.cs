using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInputManager : Singleton<MouseInputManager>
{
    public Vector3 HitPosition;
    LayerMask LayerMask;

    private void Start()
    {
        LayerMask = LayerMask.GetMask("Ground");
    }
    private void FixedUpdate()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, LayerMask))
        {
            HitPosition = hit.point;
        }
    }
}
