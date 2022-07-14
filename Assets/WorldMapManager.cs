using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WorldMapManager : MonoBehaviour
{
    InputActionMap playerActionMap;
    InputAction M;

    public GameObject WorldMap;

    // Start is called before the first frame update
    void Start()
    {
        playerActionMap = ActionMapManager.playerInput.actions.FindActionMap(ActionMapManager.ActionMap.Player);
        playerActionMap.Enable();
        M = playerActionMap["M"];
        M.performed += M_performed;
    }

    private void M_performed(InputAction.CallbackContext obj)
    {
        toggleMap();
    }


    public void toggleMap()
    {
        WorldMap.SetActive(!WorldMap.activeInHierarchy);
    }

}
