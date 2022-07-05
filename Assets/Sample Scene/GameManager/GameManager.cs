using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; //IMPORTANT

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    //Health
    public event Action playerTakeDamage,
        playerTakeHealth;

    //Killing
    public event Action playerKillScore;

    //ActionMap
    public event Action<string> changeActionMap;

    //PlayerEmotes
    public event Action<string> emotesReaction;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        // At Starting of game , GameManager will send the event to set the actionMap
        StartCoroutine(start());
    }

    IEnumerator start()
    {
        Debug.Log("Game Manager Start");
        yield return new WaitForEndOfFrame();
        ChangeActionMap(ActionMapManager.ActionMap.Land);
    }

    /// <summary>
    /// Health Event Action
    /// </summary>
    public void PlayerTakeDamage()
    {
        playerTakeDamage?.Invoke();
    }

    public void PlayerTakeHealth()
    {
        playerTakeHealth?.Invoke();
    }

    public void PlayerKillScore()
    {
        playerKillScore?.Invoke();
    }

    /// <summary>
    /// ActionMap Event Action
    /// </summary>
    /// <param name="actionMap"></param>

    public void ChangeActionMap(string actionMap)
    {
        changeActionMap?.Invoke(actionMap);
    }


    public void EmotesReaction(string emoteName)
    {
        emotesReaction?.Invoke(emoteName);
    }
}
