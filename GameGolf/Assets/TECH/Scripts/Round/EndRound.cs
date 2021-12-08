using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndRound : MonoBehaviour
{
    public static event Action BallInHole;

    private void OnTriggerEnter(Collider other)
    {
        BallInHole?.Invoke();
    }
}
