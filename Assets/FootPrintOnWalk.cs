using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FootPrintOnWalk : MonoBehaviour
{
    InputActionMap landActionMap;
    InputAction move, jump;
    PlayerDetails playerDetails;

    //public GameObject footPrint;

    //Transform parentOfFootPrint;
    void Start()
    {
        playerDetails = GetComponent<PlayerDetails>();
        landActionMap = ActionMapManager.playerInput.actions.FindActionMap(ActionMapManager.ActionMap.Land);

        GameManager.instance.changeActionMap += ChangeActionMap;
        RegisterAction();
        //parentOfFootPrint = footPrint.transform.parent;
        //footPrint.SetActive(false);
    }

    void ChangeActionMap(string actionMap)
    {
        if (actionMap == ActionMapManager.ActionMap.Land)
        {
            RegisterAction();
        }
        else
        {
            UnRegisterActionMap();
        }
    }

    void RegisterAction()
    {
        move = landActionMap["Move"];
        jump = landActionMap["Jump"];

    }
    bool iswalking = false;

    bool timeout = true;
    void Update()
    {
        if(move.IsPressed() || jump.IsPressed() || iswalking)
        {
            playerDetails.iswalking= true;

        }
        else
        {
            playerDetails.iswalking = false;
        }
    }

    IEnumerator FootPrintVisible()
    {
        timeout = false;
        // make foot visible;
        //footPrint.SetActive(true);
        //footPrint.transform.SetParent(null);
        yield return new WaitForSeconds(2);
        // make foot disable;
        //footPrint.transform.SetParent(parentOfFootPrint);
        //footPrint.transform.localPosition = Vector3.zero;
        //footPrint.SetActive(false);

        timeout = true;
    }

    void UnRegisterActionMap()
    {
        landActionMap.Disable();
    }
}
