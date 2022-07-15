using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearbyFootprint : MonoBehaviour
{
    Camera minimapCamera;
    public List<GameObject> playersFound= new List<GameObject>();

    public List<PlayerFootPrintDetail> playerFootPrintDetails = new List<PlayerFootPrintDetail>();
    public int id;

    public GameObject MyPlayer;
    public GameObject footPrintPrefab;

    void Awake()
    {
        minimapCamera = GetComponent<Camera>(); 
        playersFound.AddRange(GameObject.FindGameObjectsWithTag("Player")); 
        foreach(var player in playersFound)
        {
            Debug.Log(player.name);

            PlayerFootPrintDetail playerFootPrint = new PlayerFootPrintDetail();
            playerFootPrint.player = player;
            playerFootPrint.playerDetail = player.GetComponent<PlayerDetails>();
            playerFootPrint.renderer = player.GetComponentInChildren<Renderer>();
            playerFootPrint.footPrint = InstanceFootPrint(transform);
            playerFootPrint.footPrintScript = playerFootPrint.footPrint.GetComponent<FootPrint>();
            playerFootPrintDetails.Add(playerFootPrint);
            if(playerFootPrint.playerDetail.id == id)
            {
                MyPlayer = player;
            }
        }
        //renderers = playersFound.GetComponentsInChildren<Renderer>();
    }

    private void Start()
    {

    }


    public GameObject InstanceFootPrint(Transform origin)
    {
        GameObject footprint = Instantiate(
            footPrintPrefab,
            (origin.position + Vector3.up*3),
            origin.rotation,
            null
            );
        footprint.SetActive(false);
        return footprint;
    }

    bool timeout = true;



    void Update()
    {
        OutputVisibleRenderers(playerFootPrintDetails);
    }

    void OutputVisibleRenderers(List<PlayerFootPrintDetail> playerFootPrints)
    {
        foreach (var playerFootPrint in playerFootPrints)
        {
            // output only the visible renderers' name
            if (IsVisible(playerFootPrint.renderer))
            {
                if(playerFootPrint.playerDetail.id != id)
                {
                    if(playerFootPrint.playerDetail.iswalking)
                    {
                        float dist = Vector3.Distance(MyPlayer.transform.position, playerFootPrint.player.transform.position);
                        //Debug.Log(dist);
                        if (dist < 10f)
                        {
                            if (!playerFootPrint.footPrint.activeInHierarchy)
                            {
                                playerFootPrint.footPrint.SetActive(true);
                                playerFootPrint.footPrintScript.CallWhenGameobjectaActive(playerFootPrint.player.transform);
                            }
                        }
                        

                    }
                }
            }
        }

    }

    private bool IsVisible(Renderer renderer)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(minimapCamera);

        if (GeometryUtility.TestPlanesAABB(planes, renderer.bounds))
            return true;
        else
            return false;
    }
}
[System.Serializable]
public class PlayerFootPrintDetail
{
    public Renderer renderer;
    public PlayerDetails playerDetail;
    public GameObject player;
    public GameObject footPrint;
    public FootPrint footPrintScript;

}