using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirDropPlane : MonoBehaviour
{
    public Transform LocationToDrop;
    public float totalDistance;

    public GameObject[] propellers;
    public float speed = 0;
    public float distanceTravelled;
    public void SomeoneFired(Transform _transform)
    {
        LocationToDrop.position = _transform.position;
    }
    public GameObject routeSource;
    public GameObject routeDestination;

    public GameObject airDrop;

    public GameObject seat;


    private void Start()
    {
        airDrop = Instantiate(airDrop);
        airDrop.gameObject.SetActive(false);
        speed = 70;
        airDrop.transform.SetParent(seat.transform);
        airDrop.transform.localPosition = Vector3.zero;
        routeSource.transform.localPosition = Vector3.zero;
        routeDestination.transform.localPosition = new Vector3(0, 0, 2530);
        routeDestination.transform.SetParent(null);
        routeSource.transform.SetParent(null);
        totalDistance = Vector3.Distance(routeSource.transform.position, routeDestination.transform.position);
        airDropDistance = Vector3.Distance(routeSource.transform.position, new Vector3(LocationToDrop.position.x,transform.position.y, LocationToDrop.position.z));
    }
    public float airDropDistance;
    bool isPayloadDrop;
    private void Update()
    {
        

        foreach (var propeller in propellers)
        {
            propeller.transform.Rotate(0, 0, 30f);
        }
        distanceTravelled += speed * Time.deltaTime;
        if (distanceTravelled < totalDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, routeDestination.transform.position, speed * Time.deltaTime);
            if(!isPayloadDrop)
            {
                if (distanceTravelled  > airDropDistance )
                {
                    isPayloadDrop = true;
                    airDrop.gameObject.SetActive(true);
                    airDrop.gameObject.transform.SetParent(null);
                }
            }

        }
        else
        {
            //drop all the player;
            //Destroy(this.gameObject);
        }
    }


}
