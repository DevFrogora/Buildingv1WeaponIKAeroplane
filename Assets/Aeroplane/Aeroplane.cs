using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
public class Aeroplane : MonoBehaviour
{
    public GameObject parachuteBag;
    public GameObject routeSource;
    public GameObject routeDestination;

    public float totalDistance;

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

    Vector3 routeSourceEarly;
    Vector3 routeDestinationEarly;
    public float farDistance;
    private void Start()
    {
        StartCoroutine(LightBlinker());
        StartCoroutine(TimerToStartPuttinPlayerOnPlane());

        routeSourceEarly = routeSource.transform.position - (Vector3.forward *farDistance);
        routeDestinationEarly = routeDestination.transform.position + Vector3.forward * farDistance;
        //totalDistance = Vector3.Distance(routeSource.position, routeDestination.position);
        totalDistance = Vector3.Distance(routeSourceEarly, routeDestinationEarly);
        PathOnWorldSpace();
        transform.position = routeSourceEarly;
    }
    public GameObject aeroPath;

    void PathOnWorldSpace()
    {
        float pathLength = Vector3.Distance(routeSource.transform.position, routeDestination.transform.position);
        Debug.Log(pathLength);
        float pathWide = 30;
        routeSource.transform.SetParent(null);
        routeDestination.transform.SetParent(null);
        aeroPath.transform.SetParent(null);
        aeroPath.transform.localScale= new Vector3(pathWide,pathLength,1);
        aeroPath.transform.position = new Vector3(transform.position.x, transform.position.y, -(0.23f * pathLength));
        
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

    public float distanceTravelled;
    void Update()
    {
        foreach(var propeller in propellers)
        {
            propeller.transform.Rotate(0, 0, 30f);
        }
        distanceTravelled += speed * Time.deltaTime;
        if(distanceTravelled < totalDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, routeDestinationEarly, speed * Time.deltaTime);
        }
        else
        {
            //drop all the player;
        }
    }

    public void onPlayerEnter(GameObject player)
    {
        player.transform.SetParent(seat.transform);
        player.transform.localPosition = Vector3.zero;
        //player.GetComponent<LinkToAeroPlane>().enabled = true; // its is enabled by scriptManager
        GameManager.instance.ChangeActionMap("Aeroplane");
        player.GetComponent<PlayerUtils>().JoinedPlaneSetting(Instantiate(parachuteBag));
        players.Add(player);
        TotalPeople(players.Count);
    }

    public void onPlayerExit(GameObject player)
    {
        player.transform.SetParent(null);
        player.transform.position = (seat.transform.position);
        player.GetComponent<PlayerUtils>().ExitPlaneSetting();
        //player.GetComponent<PlayerUtils>().EnterGlidingSetting();
        GameManager.instance.ChangeActionMap("Gliding");
        players.Remove(player);
        TotalPeople(players.Count);
    }


}
