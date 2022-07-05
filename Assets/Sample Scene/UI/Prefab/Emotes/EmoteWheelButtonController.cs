using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EmoteWheelButtonController : MonoBehaviour
{
    public EmoteScriptObject emote;
    Image emoteImage;
    void Start()
    {
        emoteImage = GetComponentsInChildren<Image>()[1];
        emoteImage.sprite = emote.emoteImage;
        if(emote.emoteImage == null)
        {
            emoteImage.color = new Color(0, 0, 0, 0);
            GetComponent<Button>().interactable = false;
        }
    }

    public void EmoteReact()
    {
        if (emote.emoteImage != null)
        {
            // Here Emote. is the base layer of state
            GameManager.instance.EmotesReaction("Emote."+emote.emoteName);
            Debug.Log(emote.emoteName);
        }
    }


}
