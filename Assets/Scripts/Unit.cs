using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public int tileX;
    public int tileY;
    int moveSpeed = 2;
    public bool isPathDefined = false;
    public List<Node> currentPath = null;
    public bool isMoving;
    private LineRenderer lineRenderer;
    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        DrawLineRender();
      //  print("isMoving " + isMoving);
    }
    void DrawLineRender()
    {
        if (currentPath != null)
        {
            int current = 0;
            lineRenderer.positionCount = currentPath.Count;
            while (current < currentPath.Count - 1)
            {
                if (TileMapManager.instance != null)
                {
                    lineRenderer.SetPosition(current, TileMapManager.instance.TileCoordToWorldCoord(currentPath[current].x, currentPath[current].y) +
                    new Vector3(0, 0, -1f));
                    lineRenderer.SetPosition(current + 1, TileMapManager.instance.TileCoordToWorldCoord(currentPath[current + 1].x, currentPath[current + 1].y) +
                        new Vector3(0, 0, -1f));
                }


                if (SpriteTileManager.instance != null)
                {
                    lineRenderer.SetPosition(current, SpriteTileManager.instance.TileCoordToWorldCoord(currentPath[current].posX, currentPath[current].posY) +new Vector3(0, 0, -1f));
                    lineRenderer.SetPosition(current + 1, SpriteTileManager.instance.TileCoordToWorldCoord(currentPath[current + 1].posX, currentPath[current + 1].posY) + new Vector3(0, 0, -1f));
                }

                //Debug.DrawLine(start, end,Color.red);

                current++;
            }
        }
    }
    public void MoveNextTileWithSpeed()
    {
        float remainMovement = moveSpeed;
        while (remainMovement>0)
        {
            if (currentPath == null)
                return;

            //Cost of the next tile
            if(TileMapManager.instance !=null)
                remainMovement -= TileMapManager.instance.costToEnterTile(currentPath[0].x, currentPath[0].y, currentPath[1].x, currentPath[1].y);

            if (SpriteTileManager.instance != null)
                remainMovement -= SpriteTileManager.instance.costToEnterTile(currentPath[0].x, currentPath[0].y, currentPath[1].x, currentPath[1].y);

            //Update unit position
            tileX = currentPath[1].x;
            tileY = currentPath[1].y;


            //Moves to the next tile on the path
            if (TileMapManager.instance != null)
                transform.position = TileMapManager.instance.TileCoordToWorldCoord(currentPath[1].x, currentPath[1].y);

            if (SpriteTileManager.instance != null)
                StartCoroutine(LerpMove(SpriteTileManager.instance.TileCoordToWorldCoord(currentPath[0].posX, currentPath[0].posY), SpriteTileManager.instance.TileCoordToWorldCoord(currentPath[1].posX, currentPath[1].posY),1f));
                //transform.position = SpriteTileManager.instance.TileCoordToWorldCoord(currentPath[1].posX, currentPath[1].posY);


            //Remove the last square from the path
            currentPath.RemoveAt(0);

            //when is the target square
            if (currentPath.Count == 1)
            {
                RedefinePath();
            }
        }
    }

    IEnumerator MoveNextTileDirect()
    {
        //Update unit position
        tileX = currentPath[1].x;
        tileY = currentPath[1].y;



        //Moves to the next tile on the path
        if (SpriteTileManager.instance != null)
        {
            float time = 0;
            while (time < 1f)
            {
                isMoving = true;
                transform.position = Vector3.Lerp(SpriteTileManager.instance.TileCoordToWorldCoord(currentPath[0].posX, currentPath[0].posY), SpriteTileManager.instance.TileCoordToWorldCoord(currentPath[1].posX, currentPath[1].posY), time);
                time += Time.deltaTime;
                yield return null;
            }
        }

        //Remove the last square from the path
        currentPath.RemoveAt(0);

        if (currentPath.Count > 1)
        {

            yield return new WaitForSeconds(0.1f);
            StartCoroutine(MoveNextTileDirect());

        }

        //when is the target square
        if (currentPath!= null && currentPath.Count == 1)
        {
            RedefinePath();
        }

    }
    public void MoveAllPath()
    {
        if (currentPath == null)
            return;

        for (int i = 0; i < currentPath.Count; i++)
        {
            SpriteTileManager.instance.SearchSprite(currentPath[i].x, currentPath[i].y).tile.movementCost = 1;
        }

        StartCoroutine(MoveNextTileDirect());

    }

    public void RedefinePath()
    {
        
        isMoving = false;
        lineRenderer.positionCount = 0;
        currentPath = null;
        isPathDefined = false;

    }
    IEnumerator LerpMove(Vector3 startPos, Vector3 endPos,float duration)
    {
        float time = 0;
        while (time < duration)
        {
            isMoving = true;
            transform.position = Vector3.Lerp(startPos, endPos, time);
            time += Time.deltaTime;
            yield return null;
        }
    }
}
