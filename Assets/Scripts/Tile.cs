using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Tile
{
    public TYPE name;
    public GameObject prefab;
    public bool isWalkable = true;
    public float movementCost = 1 ;
    
    public enum TYPE
    {
        MOUNTAIN,
        GRASS,
        MUD,
        SEA
    }
}
