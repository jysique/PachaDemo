using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private UIManager uiManager;
    private SpriteTileManager spriteTileManager;
    private PlayerManager playerManager;

    public SpriteTileManager SpriteTileManager { get { return spriteTileManager; } set { spriteTileManager = value; } }
    public UIManager UIManager { get { return uiManager; } }
    public PlayerManager PlayerManager { get { return playerManager; } }

    private void Awake()
    {
        instance = this;
        uiManager = this.GetComponent<UIManager>();
        spriteTileManager = this.GetComponent<SpriteTileManager>();
        playerManager = this.GetComponent <PlayerManager>();
    }
    private void Update()
    {
        SetMoveBtn(playerManager.PlayerUnit.GetIsMoving());
        SetInteractBtn(playerManager.PlayerUnit.GetIsMoving());
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


