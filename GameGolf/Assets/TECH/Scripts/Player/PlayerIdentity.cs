using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdentity : MonoBehaviour
{
    [SerializeField] private GameObject[] _ballPrefab = new GameObject[0];

    private void Start()
    {
        SetTeam(Team.red);
    }

    public Team _team;

    public void SetTeam(Team newTeam)
    {
        _team = newTeam;
        _ballPrefab[(int)_team].SetActive(true);
    }
}

public enum Team
{
    green,
    red
}
