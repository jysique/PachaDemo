using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private GameObject player;

    private UIManager uiManager;
    private SpriteTileManager spriteTileManager;
    private Unit playerUnit;
    public Unit PlayerUnit { get { return playerUnit; } }
    public SpriteTileManager SpriteTileManager
    {
        get { return spriteTileManager; }
        set { spriteTileManager = value; }
    }
    public UIManager UIManager { get { return uiManager; } }

    private void Awake()
    {
        instance = this;
        playerUnit = player.GetComponent<Unit>();
        uiManager = this.GetComponent<UIManager>();
        spriteTileManager = this.GetComponent<SpriteTileManager>();
    }
    private void Update()
    {
        SetMoveBtn(playerUnit.GetIsMoving());
        SetInteractBtn(playerUnit.GetIsMoving());
    }

    #region STATES BUTTONS
    private void SetMoveBtn(bool isMoving)
    {
        uiManager.SetMoveBtn(isMoving);
    }
    private void SetInteractBtn(bool isMoving)
    {
        uiManager.SetInteractBtn(isMoving);
    }
    #endregion


}


