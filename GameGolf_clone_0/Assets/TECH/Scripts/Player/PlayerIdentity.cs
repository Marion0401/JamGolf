using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdentity : MonoBehaviour
{
    [SerializeField] private GameObject[] _ballPrefab = new GameObject[0];

    public Team _team;


    public void SetTeam(Team newTeam)
    {
        _team = newTeam;
    }
}

public enum Team
{
    green,
    red
}
