using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public int x;
    public int y;
    public float posX;
    public float posY;

    public List<Node> neighbours;
    public Node()
    {
        neighbours = new List<Node>();

    }
    public float DistanceTo(Node n)
    {
        return Vector2.Distance(
            new Vector2(x, y),
            new Vector2(n.x, n.y)
            );
    }
}
