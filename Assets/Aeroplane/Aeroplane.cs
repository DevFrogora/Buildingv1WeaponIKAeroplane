using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
public class Aeroplane : MonoBehaviour
{

    public GameObject[] propellers;
    public float speed;
    public GameObject seat;
    public Light middleLight;

    public List<GameObject> players = new List<GameObject>();

    public event Action<int> totalPeople;
    public event Action<int> countDownCounter;

    public static Aeroplane instance;

    public TextMeshProUGUI startTimer;
    private void Awake()
    {
        instance = this;
    }

    void TotalPeople(int count)
    {
        totalPeople?.Invoke(count);
    }

    void CountDownCounter(int count)
    {
        startTimer.text = count.ToString();
        countDownCounter?.Invoke(count);
    }

    private void Start()
    {
        StartCoroutine(LightBlinker());
        StartCoroutine(TimerToStartPuttinPlayerOnPlane());
    }

    public int countdownTime;

    IEnumerator TimerToStartPuttinPlayerOnPlane()
    {
        while (countdownTime > 0)
        {
            CountDownCounter(countdownTime);
            yield return new WaitForSeconds(1f);
            countdownTime--;
        }

        CountDownCounter(countdownTime);
        yield return new WaitForSeconds(1f);
        afterTimer();


    }
    public AllPlayers allPlayers;
    void afterTimer()
    {
        startTimer.gameObject.SetActive(false);
        //pickUpALl the player
        foreach (GameObject player in allPlayers.players)
        {
            onPlayerEnter(player);
        }

    }


    IEnumerator LightBlinker()
    {
        yield return new WaitForSeconds(2);
        StartCoroutine(LightBlinker());
        middleLight.enabled = !middleLight.enabled;
    }

    void Update()
    {
        foreach(var propeller in propellers)
        {
            propeller.transform.Rotate(0, 0, 30f);
        }
        //transform.Translate(transform.forward * speed *Time.deltaTime,Space.World);
    }

    public void onPlayerEnter(GameObject player)
    {
        player.transform.SetParent(seat.transform);
        player.transform.localPosition = Vector3.zero;
        //player.GetComponent<LinkToAeroPlane>().enabled = true;
        GameManager.instance.ChangeActionMap("Aeroplane");
        player.GetComponent<PlayerUtils>().JoinedPlaneSetting();
        players.Add(player);
        TotalPeople(players.Count);
    }

    public void onPlayerExit(GameObject player)
    {
        player.transform.position = (seat.transform.position);
        players.Remove(player);
        TotalPeople(players.Count);
    }


}
