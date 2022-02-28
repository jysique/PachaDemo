using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ClickeableTile : MonoBehaviour
{
    public int posX;
    public int posY;
    
    private void OnMouseUp()
    {
        print("click in pos X: " + posX + "posY: " + posY);
        TileMapManager.instance.player.GetComponent<Unit>().isPathDefined = true;
        /*
        TileMapManager.instance.GeneratePathFindingGraph();
        print("Click!!");
        TileMapManager.instance.GeneratePathTo(posX, posY);
        */
    }
    private void OnMouseOver()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        
        if (!TileMapManager.instance.player.GetComponent<Unit>().isPathDefined)
        {

            TileMapManager.instance.GeneratePathFindingGraph();
            TileMapManager.instance.algorithm.DijkstraPathTo(posX, posY);

            // TileMapManager.instance.GeneratePathFindingGraph();
            // TileMapManager.instance.GeneratePathTo(posX, posY);
        }
    }
}
