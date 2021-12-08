using UnityEngine;
using UnityEngine.UI;

public class BallMover : MonoBehaviour
{
    [SerializeField] private GameObject _directionOnFloor = null;
    [SerializeField] private LayerMask _floorMask = new LayerMask();
    [SerializeField] private float _maxPower = 3f;
    [SerializeField] private float _powerSpeed = 0.10f;
    [SerializeField] public Image _powerBarImg = null;
    
    private Rigidbody _rigidBody = null;

    private Vector3 _currDirection = new Vector3();

    private Vector3 mouseHit = new Vector3();

    private float _currPower = 0f;

    private bool _powerBack = false;

    private bool _isLaunched = false;

    public bool _canPlay = false;

    private Vector3 _lastPosition = new Vector3();

    #region OnEnable / OnDisable
    private void OnDisable()
    {
        _directionOnFloor.SetActive(false);
    }
    #endregion

    private void Awake()
    {
        GameObject[] otherBall = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject item in otherBall)
        {
            Physics.IgnoreCollision(this.gameObject.GetComponent<Collider>(), item.GetComponent<Collider>());
        }
    }

    private void Start()
    {
        _rigidBody = this.gameObject.GetComponent<Rigidbody>();

        _lastPosition = transform.position;        
    }

    private void Update()
    {
         mouseHit = GetMouseClickWorldPos();
        _currDirection = (mouseHit - transform.position).normalized;

        _currDirection.y = 0.5f;

        DirectionOnFloorUi();

        if (!_isLaunched && _canPlay)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _currPower = 0;
            }

            if (Input.GetMouseButton(0))
            {
                if (!_powerBack)
                {
                    if (_currPower < _maxPower)
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
                    if (_currPower > 0)
                    {
                        _currPower -= _powerSpeed;
                    }
                    else if (_currPower <= 0)
                    {
                        _powerBack = false;
                    }
                }
                _powerBarImg.fillAmount = _currPower / _maxPower;
            }
            else
            {
                if (_powerBarImg.fillAmount != 0) _powerBarImg.fillAmount = 0;
            }

            if (Input.GetMouseButtonUp(0))
            {
                MoveBall(_currDirection, _currPower);
                _isLaunched = true;
                GameManager.instance.MoveBallServer(_currDirection, _currPower);
            }
        }   
        
        if(_rigidBody.velocity == Vector3.zero && _isLaunched)
        {
            BallStop();   
        }
    }

    private void DirectionOnFloorUi()
    {

        Vector3 newDir = _currDirection;
        newDir.y = 0f;

        if (!_isLaunched && _canPlay)
        {
            if (!_directionOnFloor.activeInHierarchy)
            {
                _directionOnFloor.SetActive(true);
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _floorMask))
            {
                _directionOnFloor.transform.forward = newDir;
            }
        }
        else
        {
            if (_directionOnFloor.activeInHierarchy)
            {
                _directionOnFloor.SetActive(false);
            }
        }
    }

    public void ResetBallPosWhenOutOfMap()
    {
        _rigidBody.velocity = Vector3.zero;
        transform.position = _lastPosition;
        EndRound();
    }

    private void BallStop()
    {
        _isLaunched = false;
        _lastPosition = transform.position;
        GameManager.instance.AdjustBallPosServer(transform.position);
        EndRound();
    }

    public void MoveBallAtWorldPos(Vector3 newPos)
    {
        transform.position = newPos + new Vector3(0,0.01f,0f);
    }

    private void EndRound()
    {
        _canPlay = false;
        GameManager.instance.PlayerEndRound();
    }

    public void MoveBall(Vector3 newDir, float newPower)
    {
        _rigidBody.velocity = newDir * newPower;
    }

    private Vector3 GetMouseClickWorldPos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _floorMask);
        return hit.point;
    }
}
