using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEmoteManager : MonoBehaviour
{
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        GameManager.instance.emotesReaction += EmotesReaction;
        GameManager.instance.changeActionMap += ChangeActionMap;

    }
    void ChangeActionMap(string actionMap)
    {
        if (actionMap == ActionMapManager.ActionMap.Land)
        {
            animator.SetLayerWeight((int)AnimatorManager.AnimatorLayer.Emote, 1);
        }
        else
        {
            animator.SetLayerWeight((int)AnimatorManager.AnimatorLayer.Emote, 0);
        }
    }

    void EmotesReaction(string emoteName)
    {
        animator.Play(emoteName,(int) AnimatorManager.AnimatorLayer.Emote);
    }
}
