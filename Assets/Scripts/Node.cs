using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    private int x;
    private int y;
    private float posX;
    private float posY;

    public int X { get { return x; } set { x = value; } }
    public int Y { get { return y; } set { y = value; } }

    public float PosX { get { return posX; } set { posX = value; } }
    public float PosY { get { return posY;} set { posY = value; } }

    public List<Node> neighbours;
    


    public Node()
    {
        neighbours = new List<Node>();

    }
    #region METHODS
    public float DistanceTo(Node n)
    {
        return Vector2.Distance(
            new Vector2(x, y),
            new Vector2(n.x, n.y)
            );
    }
    #endregion

}
