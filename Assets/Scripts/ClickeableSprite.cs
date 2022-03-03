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
    [SerializeField] private Tile tile;
    [SerializeField] private bool isCity;

    private bool isFounded = false;

    public List<ClickeableSprite> neighbours;
    public List<string> ng = new List<string>();

    public string Name { get { return nameSprite; } }
    public int PosX { get { return posX; } }
    public int PosY { get { return posY; } }
    public Tile Tile { get { return tile; } }
    public bool IsCity { get { return isCity; } set { isCity = value; } }
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
    private void PaintSprite()
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
        }
    }
    public void InstantiateCity()
    {
        float x = this.transform.position.x;
        float y = this.transform.position.y;
        Instantiate(GameManager.instance.SpriteTileManager.CityPrefab, new Vector3(x, y, 0.125f), Quaternion.identity, this.transform);
    }
    #endregion

    #region STATE MOUSE METHODS
    private void OnMouseOver()
    {
        if (isCity == false)
            return;
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        if (GameManager.instance.PlayerManager.PlayerUnit.GetIsMoving())
            return;
        if (GameManager.instance.PlayerManager.PlayerUnit.IsPathDefined)
            return;
        GameManager.instance.SpriteTileManager.GeneratePathFindingGraph();
        GameManager.instance.SpriteTileManager.DijkstraPathTo(posX, posY);
    }
    private void OnMouseDown()
    {
        if (isCity == false)
            return;
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        if (GameManager.instance.PlayerManager.PlayerUnit.GetIsMoving())
            return;
        GameManager.instance.PlayerManager.PlayerUnit.IsPathDefined = true;
        GameManager.instance.SpriteTileManager.DijkstraPathTo(posX, posY);

    }

    #endregion


}
