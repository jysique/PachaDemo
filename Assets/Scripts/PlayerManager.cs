using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    [SerializeField] private GameObject player;
    private Unit playerUnit;
    public Unit PlayerUnit { get { return playerUnit; } }

    private void Awake()
    {
        playerUnit = player.GetComponent<Unit>();
    }
    private void Start()
    {
        playerUnit.TileX = 0;
        playerUnit.TileY = 0;
    }
}
