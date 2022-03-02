using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteTileManager : MonoBehaviour
{
    [SerializeField] private Transform tileSpriteContainer;
    [SerializeField] private GameObject cityPrefab;

    [Header("List Elements")]
    private List<Node> graph;
    private List<ClickeableSprite> clickeableSprites = new List<ClickeableSprite>();
    private List<ClickeableSprite> candidates = new List<ClickeableSprite>();

    [Header("Parameters")]
    private int max_childs = 4;
    private int max_add_tiers = 2;
    
    
    [Header("SpriteDetails")]
    public List<Material> materials;

    [Header("Debug")]
    [SerializeField] private bool withCost;
    
    private void Start()
    {
        GameManager.instance.PlayerUnit.TileX = 0;
        GameManager.instance.PlayerUnit.TileY = 0;
        GetAllClickeableSprite();
        InstantiateCityInClickeableSprite();
        GeneratePathFindingGraph();

    }

    private void GetAllClickeableSprite()
    {

        List<Transform> a = GetAllChildren(tileSpriteContainer);
        for (int i = 0; i < a.Count; i++)
        {
            ClickeableSprite cs = a[i].GetComponent<ClickeableSprite>();
            if (cs != null)
                clickeableSprites.Add(cs);
        }
        for (int i = 0; i < clickeableSprites.Count; i++)
        {
            clickeableSprites[i].GetNeighbours();
        }
        for (int i = 0; i < clickeableSprites.Count; i++)
        {
            candidates.Add(clickeableSprites[i]);
        }
    }
    public void Interact()
    {
        //print("nivel" + GameManager.instance.PlayerUnit.TileX);
        int _x = GameManager.instance.PlayerUnit.TileX;
        int _y = GameManager.instance.PlayerUnit.TileY;
        if (SearchSprite(_x,_y).IsFounded)
        {
            print("ya salio nodos hijos de aqui");
            return;
        }
        SearchSprite(_x,_y).IsFounded = true;
        int childs = Random.Range(2, max_childs);
        for (int i = 0; i < candidates.Count; i++)
        {
            if (candidates[i].PosX == _x + max_add_tiers && childs>0)
            {
                InstantiateCityInClickeableSprite(candidates[i]);
                candidates.RemoveAt(i);
                childs--;
            }
        }
    }



    #region PATH METHODS (DIJKSTRA)
    private bool UnitCanEnterTile(int x, int y)
    {
        return SearchSprite(x, y).Tile.IsWalkable;
    }
    Node SearchNode(int x, int y)
    {
        for (int i = 0; i < graph.Count; i++)
        {
            if (x == graph[i].X && y == graph[i].Y)
            {
                return graph[i];
            }
        }
        return null;
    }
    Node SearchNode(ClickeableSprite cs)
    {
        return SearchNode(cs.PosX, cs.PosY);
    }

    public void GeneratePathFindingGraph()
    {
        graph = new List<Node>();

        for (int i = 0; i < clickeableSprites.Count; i++)
        {
            Node node = new Node();
            node.X = clickeableSprites[i].PosX;
            node.Y = clickeableSprites[i].PosY;
            node.PosX = clickeableSprites[i].transform.position.x;
            node.PosY = clickeableSprites[i].transform.position.y;
            graph.Add(node);

        }

        for (int i = 0; i < clickeableSprites.Count; i++)
        {
            for (int j = 0; j < clickeableSprites[i].neighbours.Count; j++)
            {
                Node node = SearchNode(clickeableSprites[i].neighbours[j]);
                if (node != null)
                    graph[i].neighbours.Add(node);
            }

        }

    }
    public float CostToEnterTile(int sourceX, int sourceY, int targetX, int targetY)
    {
        Tile tt = SearchSprite(targetX, targetY).Tile;
        float cost = tt.MovementCost;

        if (!UnitCanEnterTile(targetX, targetY))
            return Mathf.Infinity;

        if (sourceX != targetX && sourceY != targetY)
        {
            //permite añadir un costo adicional al mover en diagional . cosmetic thing
            cost += 0.001f;
        }

        return cost;
    }
    public void DijkstraPathTo(int x, int y)
    {

        if (!UnitCanEnterTile(x, y))
        {
            return;
        }

        Unit unit = GameManager.instance.PlayerUnit;
        unit.SetCurrentPath(null);

        Dictionary<Node, float> dist = new Dictionary<Node, float>();
        Dictionary<Node, Node> prev = new Dictionary<Node, Node>();

        List<Node> unvisitednodes = new List<Node>();

        Node source = SearchNode(unit.TileX, unit.TileY);
        Node target = SearchNode(x, y);

        dist[source] = 0;
        prev[source] = null;

        foreach (Node v in graph)
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
                if (!withCost)
                {
                    alt = dist[u] + u.DistanceTo(v); //neighbours with only distance 
                }
                else
                {
                    alt = dist[u] + CostToEnterTile(u.X, u.Y, v.X, v.Y);// neighbours with cost per tile
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

        //  unit.currentPath = currentPath;
        unit.SetCurrentPath(currentPath);

    }
    #endregion


    #region INSTANTIATE METHODS
    private void InstantiateCityInClickeableSprite()
    {
        for (int i = 0; i < clickeableSprites.Count; i++)
        {
            if (clickeableSprites[i].City == true)
            {
                float x = clickeableSprites[i].transform.position.x;
                float y = clickeableSprites[i].transform.position.y;
                Instantiate(cityPrefab, new Vector3(x, y, 0.125f), Quaternion.identity, clickeableSprites[i].transform);
            }
        }
    }
    private void InstantiateCityInClickeableSprite(ClickeableSprite clickeableSprite)
    {
        float x = clickeableSprite.transform.position.x;
        float y = clickeableSprite.transform.position.y;
        Instantiate(cityPrefab, new Vector3(x, y, 0.125f), Quaternion.identity, clickeableSprite.transform);
        clickeableSprite.City = true;
    }
    #endregion



    #region SEARCH METHODS
    private List<Transform> GetAllChildren(Transform aTransform, List<Transform> aList = null)
    {
        if (aList == null)
            aList = new List<Transform>();
        int start = aList.Count;
        for (int n = 0; n < aTransform.childCount; n++)
            aList.Add(aTransform.GetChild(n));
        for (int i = start; i < aList.Count; i++)
        {
            var t = aList[i];
            for (int n = 0; n < t.childCount; n++)
                aList.Add(t.GetChild(n));
        }
        return aList;
    }
    public ClickeableSprite SearchSprite(string name)
    {
        for (int i = 0; i < clickeableSprites.Count; i++)
        {
            if (clickeableSprites[i].Name == name)
            {
                return clickeableSprites[i];
            }
        }
        //  print("no se encontro " + name );
        return null;
    }

    public ClickeableSprite SearchSprite(int x, int y)
    {
        for (int i = 0; i < clickeableSprites.Count; i++)
        {
            if (clickeableSprites[i].PosX == x && clickeableSprites[i].PosY == y)
            {
                return clickeableSprites[i];
            }
        }
        return null;
    }
    #endregion



}
