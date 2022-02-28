using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class TileMapManager : MonoBehaviour
{
    public static TileMapManager instance;
    public TYPE typeMovement;
    public bool withCost;
    public DijkstraAlgorithm algorithm;
    public enum TYPE
    {
        four,
        eight
    }

    public List<Tile> tileTypes;
    public List<List<int>> tiles;
    
    public Transform tileMap;
    public GameObject player;


    int mapSizeX = 10;  
    int mapSizeY = 10;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        player.GetComponent<Unit>().tileX = (int)player.transform.position.x;
        player.GetComponent<Unit>().tileY = (int)player.transform.position.y;
        tiles = new List<List<int>>();
        GenerateMapData();
       //  PrintMapData();
        GenerateVisualMap();
        //    GeneratePathFindingGraph();
        algorithm = new DijkstraAlgorithm(player);
        algorithm.GeneratePathFindingGraph(mapSizeX,mapSizeY);
    }
    void GenerateMapData()
    {
        for (int x = 0; x < mapSizeX; x++)
        {
            List<int> _row = new List<int>();
            for (int y = 0; y < mapSizeY; y++)
            {
                _row.Add(0);
            }
            tiles.Add(_row);
        }

        for (int i = 4; i < 9; i++)
        {
            tiles[2][i] = 1;
        }


        for (int i = 4; i < 9; i++)
        {
            tiles[i][4] = 2;
        }
        
        tiles[4][5] = 2;
        tiles[4][6] = 2;
        tiles[8][5] = 2;
        tiles[8][6] = 2;
    }
    void GenerateVisualMap()
    {
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                Tile t = tileTypes[tiles[x][y]];
                GameObject go = Instantiate(t.prefab, new Vector3(x, y, 0.125f), Quaternion.identity, tileMap);
                ClickeableTile clickeableTile = go.GetComponent<ClickeableTile>();
                clickeableTile.posX = x;
                clickeableTile.posY = y;
            }
        }
    }

    void PrintMapData()
    {
        for (int i = 0; i < mapSizeX; i++)
        {
            for (int j = 0; j < mapSizeY; j++)
            {
                print(i + "," + j + " - " + tiles[i][j]);
            }
        }
    }

    public float costToEnterTile(int sourceX, int sourceY, int targetX, int targetY)
    {

        Tile tt = tileTypes[tiles[targetX][targetY]];
        float cost = tt.movementCost;

        if (!UnitCanEnterTile(targetX,targetY))
            return Mathf.Infinity;

        if (sourceX != targetX && sourceY != targetY)
        {
            //permite añadir un costo adicional al mover en diagional . cosmetic thing
            cost += 0.001f;
        }

        return cost;
    }

  //  Node[,] graph;

    public void GeneratePathFindingGraph()
    {
        algorithm.GeneratePathFindingGraph(mapSizeX,mapSizeY);
    }
  /*
    public void GeneratePathFindingGraph()
    {
        graph = new Node[mapSizeX,mapSizeY];

        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                graph[x, y] = new Node();
                graph[x, y].x = x;
                graph[x, y].y = y;
            }
        }
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                switch (typeMovement)
                {
                    case TYPE.four:
                        if (x > 0)
                            graph[x, y].neighbours.Add(graph[x - 1, y]);
                        if (x < mapSizeX - 1)
                            graph[x, y].neighbours.Add(graph[x + 1, y]);
                        if (y > 0)
                            graph[x, y].neighbours.Add(graph[x, y - 1]);
                        if (y < mapSizeY - 1)
                            graph[x, y].neighbours.Add(graph[x, y + 1]);
                        break;
                    case TYPE.eight:
                        if (x > 0)
                        {
                            graph[x, y].neighbours.Add(graph[x - 1, y]);
                            if (y > 0)
                                graph[x, y].neighbours.Add(graph[x - 1, y - 1]);
                            if (y < mapSizeY - 1)
                                graph[x, y].neighbours.Add(graph[x - 1, y + 1]);
                        }
                        // RIGHT
                        if (x < mapSizeX - 1)
                        {
                            graph[x, y].neighbours.Add(graph[x + 1, y]);
                            if (y > 0)
                                graph[x, y].neighbours.Add(graph[x + 1, y - 1]);
                            if (y < mapSizeY - 1)
                                graph[x, y].neighbours.Add(graph[x + 1, y + 1]);
                        }
                        // UP - DOWN

                        if (y > 0)
                            graph[x, y].neighbours.Add(graph[x, y - 1]);
                        if (y < mapSizeY - 1)
                            graph[x, y].neighbours.Add(graph[x, y + 1]);
                        break;
                    default:
                        break;
                }
                // 4-way connection version:
                /*
                if (x > 0)
                    graph[x, y].neighbours.Add(graph[x - 1, y]);
                if (x < mapSizeX - 1)
                    graph[x, y].neighbours.Add(graph[x + 1, y]);
                if (y > 0)
                    graph[x, y].neighbours.Add(graph[x, y - 1]);
                if (y < mapSizeY - 1)
                    graph[x, y].neighbours.Add(graph[x, y + 1]);
                */
    // 8-way connection version:
    // LEFT
    /*
    if (x > 0)
    {
        graph[x, y].neighbours.Add(graph[x - 1, y]);
        if (y > 0)
            graph[x, y].neighbours.Add(graph[x-1, y - 1]);
        if (y < mapSizeY - 1)
            graph[x, y].neighbours.Add(graph[x-1, y + 1]);
    }
    // RIGHT
    if (x < mapSizeX - 1)
    {
        graph[x, y].neighbours.Add(graph[x + 1, y]);
        if (y > 0)
            graph[x, y].neighbours.Add(graph[x + 1, y - 1]);
        if (y < mapSizeY - 1)
            graph[x, y].neighbours.Add(graph[x + 1, y + 1]);
    }
    // UP - DOWN

    if (y > 0)
        graph[x, y].neighbours.Add(graph[x, y - 1]);
    if (y < mapSizeY - 1)
        graph[x, y].neighbours.Add(graph[x, y + 1]);
    */
    /*
}
}
}
*/

    public Vector3 TileCoordToWorldCoord(int x,int y)
    {
        return new Vector3(x,y,0);
    }

    public bool UnitCanEnterTile(int x, int y)
    {

        return tileTypes[tiles[x][y]].isWalkable;
    }
    /*
    public void GeneratePathTo(int x, int y)
    {
        //player.transform.position = TileCoordToWorldCoord(x, y);
        
        if (!UnitCanEnterTile(x,y))
        {
            return;
        }

        Unit unit = player.GetComponent<Unit>();
        unit.currentPath = null;

        Dictionary<Node,float> dist = new Dictionary<Node,float>();
        Dictionary<Node,Node> prev = new Dictionary<Node,Node>();

        List<Node> unvisitednodes = new List<Node>();

        Node source = graph[unit.tileX, unit.tileY];
        Node target = graph[x, y];

        dist[source] = 0;
        prev[source] = null;

        foreach (Node v in graph)
        {
            if(v != source)
            {
                dist[v] = Mathf.Infinity;
                prev[v] = null;
            }
            unvisitednodes.Add(v);
        }
        while (unvisitednodes.Count >0)
        {
            //Node u = unvisitednodes.OrderBy(n => dist[n]).First();
            Node u = null;

            foreach (Node possibleU in unvisitednodes)
            {
                if(u == null || dist[possibleU] < dist[u])
                {
                    u = possibleU;
                }
            }

            if(u == target)
            {
                break;
            }

            unvisitednodes.Remove(u);
            foreach (Node v in u.neighbours)
            {
                float alt = 0f;
                if (!withCost)
                {
                    alt = dist[u] + u.DistanceTo(v);
                }
                else
                {
                    alt = dist[u] + costToEnterTile(u.x, u.y, v.x, v.y);
                }
                //float alt = dist[u] + u.DistanceTo(v);
                //float alt = dist[u] + costToEnterTile(u.x , u.y , v.x,v.y);
                if (alt < dist[v])
                {
                    dist[v] = alt;
                    prev[v] = u;
                }

            }
        }
        if(prev[target] == null)
        {
            return;
        }

        List<Node>currentPath = new List<Node>();


        Node curr = target;
        while (curr != null)
        {
            currentPath.Add(curr);
            curr = prev[curr];
        }

        currentPath.Reverse();

        unit.currentPath = currentPath;

    }
    */
}
