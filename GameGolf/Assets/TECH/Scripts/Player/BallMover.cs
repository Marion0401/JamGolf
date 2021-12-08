using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMover : MonoBehaviour
{
    [SerializeField] private LayerMask _floorMask = new LayerMask();
    [SerializeField] private GameObject _directionOnFloor = null;

    private Vector3 _currDirection = new Vector3();

    Vector3 mouseHit = new Vector3();

    private void Update()
    {
        mouseHit = GetMouseClickWorldPos();

        _currDirection = (mouseHit - transform.position).normalized;

        _directionOnFloor.transform.forward = _currDirection;
       
    }

    private Vector3 GetMouseClickWorldPos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _floorMask);
        return hit.point;
    }
}
