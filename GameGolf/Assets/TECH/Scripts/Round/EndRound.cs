using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndRound : MonoBehaviour
{
    public static event Action<PlayerIdentity> BallInHole;

    private void OnTriggerEnter(Collider other)
    {
        PlayerIdentity currPlayer = other.gameObject.GetComponent<PlayerIdentity>();
        BallInHole?.Invoke(currPlayer);
    }
}
