using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<BallMover>(out BallMover ballMover))
        {
            if(ballMover == GameManager.instance.myBall.GetComponent<BallMover>())
            {
                ballMover.ResetBallPosWhenOutOfMap();
            }
        }
    }
}
