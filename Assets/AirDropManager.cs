using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirDropManager : MonoBehaviour
{

    public static AirDropManager instance;
    //List<Transform> airDropLocations = new List<Transform>();
    public GameObject airDropPlane;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }


    public void CallDropLocation(Transform location)
    {
        StartCoroutine(SpawnAirDropPlane(location));
    }


    IEnumerator SpawnAirDropPlane(Transform location)
    {
        yield return new WaitForSeconds(3);
        GameObject _airDropPlane = Instantiate(airDropPlane);
        AirDropPlane _airDropSRef = _airDropPlane.GetComponent<AirDropPlane>();
        if(transform.eulerAngles.y >= 0 && transform.eulerAngles.y <= 180)
        {
                    //600 terrain max height + add more if needed;
            _airDropSRef.transform.position =  new Vector3(-1265f,150,0);
            _airDropSRef.transform.LookAt(location, Vector3.up);
            _airDropSRef.transform.eulerAngles = new Vector3(0, _airDropSRef.transform.eulerAngles.y, 0);

        }
        else
        {
            _airDropSRef.transform.position = new Vector3(1265f, 1000, 0);
            _airDropSRef.transform.LookAt(location, Vector3.up);
            _airDropSRef.transform.eulerAngles = new Vector3(0, _airDropSRef.transform.eulerAngles.y, 0);

        }
        _airDropSRef.LocationToDrop = location;

    }

}
