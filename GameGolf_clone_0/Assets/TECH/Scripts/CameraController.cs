using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject _currBall = null;

    private Vector3 _offsetCamera = new Vector3();

    private void Start()
    {
        _offsetCamera = transform.position;
    }

    private void Update()
    {
        if(_currBall == null) { return; }
        transform.position = new Vector3(_offsetCamera.x + _currBall.transform.position.x, _offsetCamera.y, _offsetCamera.z + _currBall.transform.position.z);
    }

    public void SetBallForCamera(GameObject newBall)
    {
        _currBall = newBall;
    }
}
