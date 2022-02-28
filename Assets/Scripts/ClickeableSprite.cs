using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class ClickeableSprite : MonoBehaviour
{
    
    public string nameSprite;
   
    public int posX;
    public int posY;

    public List<ClickeableSprite> neighbours;
    public bool isFounded= false;
    public TYPE city;
    public enum TYPE
    {
        CITY,
        NO_CITY,
    }

    public Tile tile;

    private void Awake()
    {
        nameSprite = transform.name;
    }
    private void Start()
    {
        PaintSprite();
    }
    void PaintSprite()
    {
        switch (tile.name)
        {
            case Tile.TYPE.MOUNTAIN:
                transform.GetComponent<SpriteRenderer>().color = SpriteTileManager.instance.materials[1].color;
                break;
            case Tile.TYPE.GRASS:
                transform.GetComponent<SpriteRenderer>().color = SpriteTileManager.instance.materials[0].color;
                break;
            case Tile.TYPE.MUD:
                transform.GetComponent<SpriteRenderer>().color = SpriteTileManager.instance.materials[2].color;
                break;
            case Tile.TYPE.SEA:
                transform.GetComponent<SpriteRenderer>().color = SpriteTileManager.instance.materials[3].color;
                break;
            default:
                transform.GetComponent<SpriteRenderer>().color = SpriteTileManager.instance.materials[0].color;
                break;
        }
    }
    public void GetNeighbours()
    {
        for (int i = 0; i < ng.Count; i++)
        {
            if (SpriteTileManager.instance.SearchSprite(ng[i]) != null)
                neighbours.Add(SpriteTileManager.instance.SearchSprite(ng[i]));
        }
    }

    public GameObject GetCity()
    {
        return transform.GetChild(0).gameObject;
    }

    private void OnMouseUp()
    {
        if(SpriteTileManager.instance!=null)
            SpriteTileManager.instance.player.GetComponent<Unit>().isPathDefined = true;
    }
    private void OnMouseOver()
    {
        if (city == TYPE.NO_CITY)
        {
            return;
        }
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        if (!SpriteTileManager.instance.player.GetComponent<Unit>().isPathDefined)
        {
            SpriteTileManager.instance.GeneratePathFindingGraph();
            SpriteTileManager.instance.DijkstraPathTo(posX, posY);
        }

    }
    public List<string> ng = new List<string>();
}
