using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleEmoteWheel : MonoBehaviour
{
    public GameObject emoteWheelUI;
    bool isClick = false;

    private void Start()
    {
        GameManager.instance.emotesReaction += emoteClicked;
        emoteWheelUI.SetActive(isClick);
    }

    public void OnClick()
    {
        isClick = !isClick;
        emoteWheelUI.SetActive(isClick);
    }

    void emoteClicked(string emoteName)
    {
        isClick = !isClick;
        emoteWheelUI.SetActive(isClick);
    }
}
