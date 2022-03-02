using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    [SerializeField] private Button moveBtn;
    [SerializeField] private Button interactBtn;
    #region STATES BUTTONS
    public void SetMoveBtn(bool isMoving)
    {
        moveBtn.interactable = !isMoving;
    }
    public void SetInteractBtn(bool isMoving)
    {
        interactBtn.interactable = !isMoving;
    }
    #endregion

}
