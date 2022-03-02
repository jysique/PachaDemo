using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Timers;
public class Unit : MonoBehaviour
{
    private int tileX;
    private int tileY;
    [SerializeField] private bool isPathDefined;
    [SerializeField] private bool isMoving;
    private List<Node> currentPath = null;

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

    public int TileX { get { return tileX; } set { tileX = value; } }
    public int TileY { get { return tileY; } set { tileY = value; } }
    public bool IsPathDefined { get { return isPathDefined; } set { isPathDefined = value; } }

    public void SetCurrentPath(List<Node> newPath) { currentPath = newPath; }
    public bool GetIsMoving() { return isMoving; }
    private void DrawLineRender()
    {
        if (currentPath != null)
        {
            int current = 0;
            lineRenderer.positionCount = currentPath.Count;
            while (current < currentPath.Count - 1)
            {

                lineRenderer.SetPosition(current, TileCoordToWorldCoord(currentPath[current].PosX, currentPath[current].PosY, -1f));
                lineRenderer.SetPosition(current + 1, TileCoordToWorldCoord(currentPath[current + 1].PosX, currentPath[current + 1].PosY, -1f));
                current++;
            }
        }
    }

    #region MOVE METHODS
    public void MoveAllPath()
    {
        if (currentPath == null)
            return;

        for (int i = 0; i < currentPath.Count; i++)
        {
            GameManager.instance.SpriteTileManager.SearchSprite(currentPath[i].X, currentPath[i].Y).Tile.MovementCost = 1;
        }

        //  StartCoroutine(MoveNextTileDirect());
        MoveNextTileDirect();
    }
    void MoveNextTileDirect()
    {
        //Update unit position
        tileX = currentPath[1].X;
        tileY = currentPath[1].Y;

        //Moves to the next tile on the path
        if (GameManager.instance.SpriteTileManager != null)
        {
            float time = 0;
            while (time < 2f)
            {
                isMoving = true;
                transform.position = Vector3.Lerp(TileCoordToWorldCoord(currentPath[0].PosX, currentPath[0].PosY), TileCoordToWorldCoord(currentPath[1].PosX, currentPath[1].PosY), time);
                time += Time.deltaTime;
                //  time += 0.001f;
                //  Timer _timer = new Timer(0.1f, () => { return; });
                //  TimersManager.SetTimer(this, _timer, true);
            }
        }
        //Remove the last square from the path
        currentPath.RemoveAt(0);

        if (currentPath.Count > 1)
        {
            Timer _timer = new Timer(0.1f, () => MoveNextTileDirect());
            TimersManager.SetTimer(this, _timer, true);
        }
        //when is the target square
        if (currentPath != null && currentPath.Count == 1)
        {
            ResetPath();
        }
    }
    private void ResetPath()
    {
        isPathDefined = false;
        isMoving = false;
        lineRenderer.positionCount = 0;
        currentPath = null;
    }
    #endregion


    private Vector3 TileCoordToWorldCoord(float x, float y,float z=0)
    {
        return new Vector3(x, y, z);
    }
}
