using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MouseInputManager : Singleton<MouseInputManager>
{
    public Vector3 MousePosition;
    public Vector3 HitPosition;
    LayerMask LayerMask;
    public MouseEvent OnMouseClicked = new MouseEvent();
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
            MousePosition = hit.point;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit, LayerMask))
            {
                HitPosition = hit.point;
                OnMouseClicked.Invoke(HitPosition);
            }
        }
    }
}

public class MouseEvent : UnityEvent<Vector3>
{

}
