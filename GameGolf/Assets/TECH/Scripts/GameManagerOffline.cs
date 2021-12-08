using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerOffline : MonoBehaviour
{
    #region OnEnable / Disable

    private void OnEnable()
    {
        EndRound.BallInHole += IsEndRound;
    }

    private void OnDisable()
    {        
        EndRound.BallInHole -= IsEndRound;
    }

    #endregion

    private void IsEndRound(PlayerIdentity currPlayer)
    {
        Debug.Log(currPlayer);
    }


}