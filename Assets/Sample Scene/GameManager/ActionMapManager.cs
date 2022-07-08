using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActionMapManager : MonoBehaviour
{
    public static class ActionMap
    {
        public static string Land = "Land";
        public static string Aeroplane = "Aeroplane";
        public static string Player = "Player";
        public static string Gliding = "Gliding";

    }

    public static PlayerInput playerInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>() ;
    }

    void Start()
    {
        GameManager.instance.changeActionMap += ChangeActionMap;
    }

    void ChangeActionMap(string actionName)
    {
        SwitchActionMap(actionName);
    }

    void SwitchActionMap( string switchName)
    {
        //currentActionMap = playerInput.actions.FindActionMap(switchName);
        playerInput.SwitchCurrentActionMap(switchName);
    }
}
