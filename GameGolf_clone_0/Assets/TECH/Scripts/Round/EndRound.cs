using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndRound : MonoBehaviour
{
    public static event Action<PlayerIdentity> BallInHole;

    float clock = 0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerIdentity>() != GameManager.instance.myBall.GetComponent<PlayerIdentity>()) { return; }
        clock = 1f;      
    }

    private void OnTriggerStay(Collider other)
    {
        PlayerIdentity currPlayer = other.gameObject.GetComponent<PlayerIdentity>();

        if (currPlayer != GameManager.instance.myBall.GetComponent<PlayerIdentity>()) { return; }

        if(clock != -1)
        {
            if (clock > 0)
            {
                clock -= Time.deltaTime;
            }
            else
            {
                BallInHole?.Invoke(currPlayer);
                clock = -1;
            }
        }

    }
}
