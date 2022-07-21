using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScriptManager : MonoBehaviour
{
    //Handling State
    //string previousScript;
    //string currentScript;
    //bool isFirst;
    CharacterLocomotion characterLandController;
    LinkToAeroPlane linkToAeroPlane;
    LinkToGliding linkToGliding;
    Swimming swimming;

    void Start()
    {
        GameManager.instance.changeActionMap += ChangeActionMap;

        characterLandController = GetComponent<CharacterLocomotion>();
        linkToAeroPlane = GetComponent<LinkToAeroPlane>();
        linkToGliding = GetComponent<LinkToGliding>();
        swimming = GetComponent<Swimming>();

    }

    void ChangeActionMap(string actionName)

    {
        if (actionName == ActionMapManager.ActionMap.Land)
        {
            characterLandController.enabled = true;
        }
        else
        {
            characterLandController.enabled = false;
        }

        if (actionName == ActionMapManager.ActionMap.Aeroplane)
        {
            linkToAeroPlane.enabled = true; //
        }
        else
        {
            linkToAeroPlane.enabled = false;
        }

        if (actionName == ActionMapManager.ActionMap.Gliding)
        {
            linkToGliding.enabled = true; //
        }
        else
        {
            linkToGliding.enabled = false;
        }

        if (actionName == ActionMapManager.ActionMap.Swimming)
        {
            swimming.enabled = true; //
        }
        else
        {
            swimming.enabled = false;
        }
    }
}
