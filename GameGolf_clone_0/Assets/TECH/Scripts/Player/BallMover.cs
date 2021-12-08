using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallMover : MonoBehaviour
{
    [SerializeField] private LayerMask _floorMask = new LayerMask();
    [SerializeField] private GameObject _directionOnFloor = null;
    [SerializeField] private float _maxPower = 3f;
    [SerializeField] private float _powerSpeed = 0.10f;
    [SerializeField] private Image _powerBarImg = null;
    
    private Rigidbody _rigidBody = null;

    private Vector3 _currDirection = new Vector3();

    private Vector3 mouseHit = new Vector3();

    private float _currPower = 0f;
    private bool _powerBack = false;

    private void Start()
    {
        _rigidBody = this.gameObject.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        mouseHit = GetMouseClickWorldPos();
        _currDirection = (mouseHit - transform.position).normalized;
        _currDirection.y = 0;
        _directionOnFloor.transform.forward = _currDirection;

        if (Input.GetMouseButtonDown(0))
        {
            _currPower = 0;
        }

        if (Input.GetMouseButton(0))
        {
            if (!_powerBack)
            {
                if(_currPower < _maxPower)
                {
                    _currPower += _powerSpeed;
                }
                else if (_currPower >= _maxPower)
                {
                    _powerBack = true;
                }
            }
            else
            {
                if(_currPower > 0)
                {
                    _currPower -= _powerSpeed;
                }
                else if(_currPower <= 0)
                {
                    _powerBack = false;
                }
            }

            _powerBarImg.fillAmount = _currPower / _maxPower;
        }
        else
        {
            if(_powerBarImg.fillAmount !=0) _powerBarImg.fillAmount = 0;
        }

        if (Input.GetMouseButtonUp(0))
        {
            _rigidBody.AddForce(_currDirection * _currPower, ForceMode.Impulse);
        }
    }

    private Vector3 GetMouseClickWorldPos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _floorMask);
        return hit.point;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, _currDirection * 5f);
    }
}
