using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Tile
{
    [SerializeField] private TYPE name;
    [SerializeField] private bool isWalkable = true;
    [SerializeField] private float movementCost = 1;

    public TYPE Name { get { return name; } set { name = value; } }
    public bool IsWalkable { get { return isWalkable; } set { isWalkable = value; } }
    public float MovementCost { get { return movementCost; } set { movementCost = value; } }

    public enum TYPE
    {
        MOUNTAIN =0,
        GRASS =1,
        MUD = 2,
        SEA  = 3,
    }
}
