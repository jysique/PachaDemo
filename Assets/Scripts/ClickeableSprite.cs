using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using System;

public class ClickeableSprite : MonoBehaviour
{
    
    [SerializeField] string nameSprite;
    [SerializeField] private int posX;
    [SerializeField] private int posY;
    [SerializeField] Tile tile;
    [SerializeField] private bool city;
    private bool isFounded = false;

    public List<ClickeableSprite> neighbours;
    public List<string> ng = new List<string>();

    public bool City { get { return city; } set { city = value; } }
    public Tile Tile { get { return tile; } }
    public string Name { get { return nameSprite; } }
    public int PosX { get { return posX; } }
    public int PosY { get { return posY; } }
    public bool IsFounded { get { return isFounded; } set { isFounded = value; } }

    private void Awake()
    {
        nameSprite = transform.name;
    }
    private void Start()
    {
        PaintSprite();
    }
    #region INIT METHODS
    void PaintSprite()
    {
        for (int i = 0; i < Enum.GetNames(typeof(Tile.TYPE)).Length; i++)
        {
            if (tile.Name.ToString() == Enum.GetNames(typeof(Tile.TYPE))[i])
            {
                transform.GetComponent<SpriteRenderer>().color = GameManager.instance.SpriteTileManager.materials[i].color;
            }
        }
    }
    public void GetNeighbours()
    {
        for (int i = 0; i < ng.Count; i++)
        {
            if (GameManager.instance.SpriteTileManager.SearchSprite(ng[i]) != null)
                neighbours.Add(GameManager.instance.SpriteTileManager.SearchSprite(ng[i]));
            //neighbours.Add(SpriteTileManager.instance.SearchSprite(ng[i]));
        }
    }

    #endregion

    #region STATE MOUSE
    private void OnMouseUp()
    {

    }
    private void OnMouseOver()
    {
        if (city == false)
            return;
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        if (GameManager.instance.PlayerUnit.GetIsMoving())
            return;
        if (GameManager.instance.PlayerUnit.IsPathDefined)
            return;
        GameManager.instance.SpriteTileManager.GeneratePathFindingGraph();
        GameManager.instance.SpriteTileManager.DijkstraPathTo(posX, posY);
    }
    private void OnMouseDown()
    {
        if (city == false)
            return;
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        if (GameManager.instance.PlayerUnit.GetIsMoving())
            return;
        GameManager.instance.PlayerUnit.IsPathDefined = true;
        GameManager.instance.SpriteTileManager.DijkstraPathTo(posX, posY);

    }

    #endregion


}
