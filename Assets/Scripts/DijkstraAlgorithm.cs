using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DijkstraAlgorithm
{
    GameObject _player;
    Node[,] _graph;

    public DijkstraAlgorithm(GameObject player)
    {
        _player = player;
    }

    

    public void GeneratePathFindingGraph(int mapSizeX, int mapSizeY)
    {
        _graph = new Node[mapSizeX, mapSizeY];

        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                _graph[x, y] = new Node();
                _graph[x, y].x = x;
                _graph[x, y].y = y;
            }
        }
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                switch (TileMapManager.instance.typeMovement)
                {
                    case TileMapManager.TYPE.four:
                        if (x > 0)
                            _graph[x, y].neighbours.Add(_graph[x - 1, y]);
                        if (x < mapSizeX - 1)
                            _graph[x, y].neighbours.Add(_graph[x + 1, y]);
                        if (y > 0)
                            _graph[x, y].neighbours.Add(_graph[x, y - 1]);
                        if (y < mapSizeY - 1)
                            _graph[x, y].neighbours.Add(_graph[x, y + 1]);
                        break;
                    case TileMapManager.TYPE.eight:
                        if (x > 0)
                        {
                            _graph[x, y].neighbours.Add(_graph[x - 1, y]);
                            if (y > 0)
                                _graph[x, y].neighbours.Add(_graph[x - 1, y - 1]);
                            if (y < mapSizeY - 1)
                                _graph[x, y].neighbours.Add(_graph[x - 1, y + 1]);
                        }
                        // RIGHT
                        if (x < mapSizeX - 1)
                        {
                            _graph[x, y].neighbours.Add(_graph[x + 1, y]);
                            if (y > 0)
                                _graph[x, y].neighbours.Add(_graph[x + 1, y - 1]);
                            if (y < mapSizeY - 1)
                                _graph[x, y].neighbours.Add(_graph[x + 1, y + 1]);
                        }
                        // UP - DOWN

                        if (y > 0)
                            _graph[x, y].neighbours.Add(_graph[x, y - 1]);
                        if (y < mapSizeY - 1)
                            _graph[x, y].neighbours.Add(_graph[x, y + 1]);
                        break;
                    default:
                        break;
                }
               
            }
        }

        for (int i = 0; i < mapSizeX; i++)
        {
            for (int j = 0; j < mapSizeY; j++)
            {
                Debug.Log("("+_graph[i, j].x + "," + _graph[i, j].y + ") ->");
                for (int k = 0; k < _graph[i, j].neighbours.Count; k++)
                {
                    Debug.Log("(" + _graph[i, j].neighbours[k].x + "," + _graph[i, j].neighbours[k].y + ")");
                }
            }
        }

    }

    public void DijkstraPathTo(int x, int y)
    {
        if (!TileMapManager.instance.UnitCanEnterTile(x, y))
        {
            return;
        }

        Unit unit = _player.GetComponent<Unit>();
        unit.currentPath = null;

        Dictionary<Node, float> dist = new Dictionary<Node, float>();
        Dictionary<Node, Node> prev = new Dictionary<Node, Node>();

        List<Node> unvisitednodes = new List<Node>();

        Node source = _graph[unit.tileX, unit.tileY];
        Node target = _graph[x, y];

        dist[source] = 0;
        prev[source] = null;

        foreach (Node v in _graph)
        {
            if (v != source)
            {
                dist[v] = Mathf.Infinity;
                prev[v] = null;
            }
            unvisitednodes.Add(v);
        }
        while (unvisitednodes.Count > 0)
        {
            Node u = null;

            foreach (Node possibleU in unvisitednodes)
            {
                if (u == null || dist[possibleU] < dist[u])
                {
                    u = possibleU;
                }
            }

            if (u == target)
            {
                break;
            }

            unvisitednodes.Remove(u);
            foreach (Node v in u.neighbours)
            {
                float alt = 0f;
                if (!TileMapManager.instance.withCost)
                {
                    alt = dist[u] + u.DistanceTo(v); //neighbours with only distance 
                }
                else
                {
                    alt = dist[u] + TileMapManager.instance.costToEnterTile(u.x, u.y, v.x, v.y);// neighbours with cost per tile
                }
                if (alt < dist[v])
                {
                    dist[v] = alt;
                    prev[v] = u;
                }

            }
        }
        if (prev[target] == null)
        {
            return;
        }

        List<Node> currentPath = new List<Node>();


        Node curr = target;
        while (curr != null)
        {
            currentPath.Add(curr);
            curr = prev[curr];
        }

        currentPath.Reverse();

        unit.currentPath = currentPath;

    }
}
