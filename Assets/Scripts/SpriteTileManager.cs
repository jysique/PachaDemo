using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteTileManager : MonoBehaviour
{
    public static SpriteTileManager instance;
    public TYPE typeMovement;
    public bool withCost;
    public List<Material> materials;

    public List<List<int>> tiles;

    public Transform tileSprite;
    public GameObject player;
    public GameObject cityPrefab;
    public Button move;
    public Button toInteract;
    List<Node> graph;

    public List<ClickeableSprite> clickeableSprites = new List<ClickeableSprite>();

    public enum TYPE
    {
        four,
        eight
    }
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        GetAllClickeableSprite();
        InstantiateCityInClickeableSprite();
        GeneratePathFindingGraph();
        for (int i = 0; i < clickeableSprites.Count; i++)
        {
            candidates.Add(clickeableSprites[i]);
        }
    }
    private void Update()
    {
        move.interactable = !player.GetComponent<Unit>().isMoving;
        toInteract.interactable = !player.GetComponent<Unit>().isMoving;
    }
    int max_childs = 4
        ;
    public List<ClickeableSprite> candidates = new List<ClickeableSprite>();
    public void Interact()
    {
        print("nivel" + player.GetComponent<Unit>().tileX);

        if (SearchSprite(player.GetComponent<Unit>().tileX, player.GetComponent<Unit>().tileY).isFounded)
        {
            print("ya salio nodos hijos de aqui");
            return;
        }
        SearchSprite(player.GetComponent<Unit>().tileX, player.GetComponent<Unit>().tileY).isFounded = true;
        int childs = Random.Range(2, max_childs);
        for (int i = 0; i < candidates.Count; i++)
        {
            if (candidates[i].posX == player.GetComponent<Unit>().tileX + 2 && childs>0)
            {
                print(candidates[i].nameSprite);
                InstantiateCityInClickeableSprite(candidates[i]);
                candidates.RemoveAt(i);
                childs--;
            }
        }
    }
    public Node tree;

    void GetAllClickeableSprite()
    {

        List<Transform> a = GetAllChildren(tileSprite);
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
        player.GetComponent<Unit>().tileX = 0;
        player.GetComponent<Unit>().tileY = 0;
    }


    void InstantiateCityInClickeableSprite()
    {
        for (int i = 0; i < clickeableSprites.Count; i++)
        {
            if (clickeableSprites[i].city == ClickeableSprite.TYPE.CITY)
            {
                float x = clickeableSprites[i].transform.position.x;
                float y = clickeableSprites[i].transform.position.y;
                Instantiate(cityPrefab, new Vector3(x, y, 0.125f), Quaternion.identity, clickeableSprites[i].transform);
            }
        }
    }
    void InstantiateCityInClickeableSprite(ClickeableSprite clickeableSprite)
    {
        float x = clickeableSprite.transform.position.x;
        float y = clickeableSprite.transform.position.y;
        Instantiate(cityPrefab, new Vector3(x, y, 0.125f), Quaternion.identity, clickeableSprite.transform);
        clickeableSprite.city = ClickeableSprite.TYPE.CITY;
    }

    public float costToEnterTile(int sourceX, int sourceY, int targetX, int targetY)
    {
        Tile tt = SearchSprite(targetX, targetY).tile;
        float cost =tt.movementCost;

        if (!UnitCanEnterTile(targetX, targetY))
            return Mathf.Infinity;

        if (sourceX != targetX && sourceY != targetY)
        {
            //permite añadir un costo adicional al mover en diagional . cosmetic thing
            cost += 0.001f;
        }

        return cost;
    }
    public bool UnitCanEnterTile(int x, int y)
    {
        return SearchSprite(x, y).tile.isWalkable;
    }
    public Vector3 TileCoordToWorldCoord(float x, float y)
    {
        return new Vector3(x, y, 0);
    }
    

    public List<Transform> GetAllChildren(Transform aTransform, List<Transform> aList = null)
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
            if (clickeableSprites[i].nameSprite == name)
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
            if (clickeableSprites[i].posX == x && clickeableSprites[i].posY == y)
            {
                return clickeableSprites[i];
            }
        }
        return null;
    }

    Node SearchNode(ClickeableSprite cs)
    {
        for (int i = 0; i < graph.Count; i++)
        {
            if (cs.posX == graph[i].x && cs.posY == graph[i].y)
            {
                return graph[i];
            }
        }
        return null;
    }

    Node SearchNode(int x, int y)
    {
        for (int i = 0; i < graph.Count; i++)
        {
            if (x == graph[i].x && y == graph[i].y)
            {
                return graph[i];
            }
        }
        return null;
    }

    public void GeneratePathFindingGraph()
    {
        graph = new List<Node>();

        for (int i = 0; i < clickeableSprites.Count; i++)
        {
            Node node = new Node();
            node.x = clickeableSprites[i].posX;
            node.y = clickeableSprites[i].posY;
            node.posX = clickeableSprites[i].transform.position.x;
            node.posY = clickeableSprites[i].transform.position.y;
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

    public void DijkstraPathTo(int x, int y)
    {

        if (!UnitCanEnterTile(x, y))
        {
            return;
        }

        Unit unit = player.GetComponent<Unit>();
        unit.currentPath = null;

        Dictionary<Node, float> dist = new Dictionary<Node, float>();
        Dictionary<Node, Node> prev = new Dictionary<Node, Node>();

        List<Node> unvisitednodes = new List<Node>();

        Node source = SearchNode(unit.tileX, unit.tileY);
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
                    alt = dist[u] + costToEnterTile(u.x, u.y, v.x, v.y);// neighbours with cost per tile
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
