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
        if((int)newTeam == 0)
        {
            _ballPrefab[0].SetActive(true);
            _ballPrefab[1].SetActive(false);
        }
        else if((int)newTeam == 1)
        {
            _ballPrefab[0].SetActive(false);
            _ballPrefab[1].SetActive(true);
        }
    }
}

public enum Team
{
    green,
    red
}
