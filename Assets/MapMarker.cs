using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapMarker : MonoBehaviour, IPointerClickHandler
{
    public Camera worldMapCam;
    public GameObject markerLocation;

    public RawImage MapRawImage;

    LineRenderer locationLineRender;

    TextMeshProUGUI locationDistanceText;

    private GameObject player;
    public int id;

    private void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player");
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (var tempplayer in players)
        {
            int tempId = tempplayer.GetComponent<PlayerDetails>().id;
            if (tempId == id)
            {
                Debug.Log(tempId);
                player = tempplayer;
            }
        }

        //MapRawImage = MapRawImage.GetComponent<RawImage>();
        locationLineRender = markerLocation.GetComponent<LineRenderer>();
        locationDistanceText = markerLocation.GetComponentInChildren<TextMeshProUGUI>();
        RemoveMarker();
        AddMarker();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerCurrentRaycast.gameObject.name);
        Vector2 curosr = new Vector2(0, 0);

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(MapRawImage.rectTransform,
            eventData.pressPosition, eventData.pressEventCamera, out curosr))
        {

            Texture texture = MapRawImage.texture;
            Rect rect = MapRawImage.rectTransform.rect;

            float coordX = Mathf.Clamp(0, (((curosr.x - rect.x) * texture.width) / rect.width), texture.width);
            float coordY = Mathf.Clamp(0, (((curosr.y - rect.y) * texture.height) / rect.height), texture.height);

            float calX = coordX / texture.width;
            float calY = coordY / texture.height;


            curosr = new Vector2(calX, calY);

            CastRayToWorld(curosr);
        }


    }

    private void CastRayToWorld(Vector2 vec)
    {
        Ray MapRay = worldMapCam.ScreenPointToRay(new Vector2(vec.x * worldMapCam.pixelWidth,
            vec.y * worldMapCam.pixelHeight));

        RaycastHit miniMapHit;

        if (Physics.Raycast(MapRay, out miniMapHit, Mathf.Infinity))
        {
            Debug.Log("miniMapHit: " + miniMapHit.collider.gameObject);
            if(locationMarkerBool)
            {
                markerLocation.transform.position = new Vector3(miniMapHit.point.x, 1000, miniMapHit.point.z);
                markerLocation.transform.localScale = Vector3.one;
                markerLocation.SetActive(locationMarkerBool);

            }

        }

    }

    private void LateUpdate()
    {
        if(locationMarkerBool)
        {
            Vector3 dest = new Vector3(player.transform.position.x, 1000, player.transform.position.z);
            locationLineRender.SetPosition(0, markerLocation.transform.position);
            locationLineRender.SetPosition(1, dest);
            locationDistanceText.text =((int) Vector3.Distance(markerLocation.transform.position, dest)).ToString() + "M";

        }

    }

    bool locationMarkerBool;
    public void RemoveMarker()
    {
        locationMarkerBool = false;
        markerLocation.SetActive(locationMarkerBool);
        markerLocation.transform.localScale = Vector3.zero;
        locationLineRender.SetPosition(0, markerLocation.transform.position);
        locationLineRender.SetPosition(1, markerLocation.transform.position);
    }

    public void AddMarker()
    {
        locationMarkerBool = true;
    }
}