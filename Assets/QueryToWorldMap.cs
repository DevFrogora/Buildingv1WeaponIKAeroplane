using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueryToWorldMap : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerDetails playerDetails;

    private ZoneUI zoneUI;

    public LineRenderer playerToTargetnLineRender;

    void Start()
    {
        StartCoroutine(UpdateTheZone());
        zoneUI = GetComponent<ZoneUI>();
    }


    IEnumerator UpdateTheZone()
    {
        yield return new WaitForSeconds(0.3f);
        ZoneUpdateMethod();
    }
    Vector3 playerPos ;

    void ZoneUpdateMethod()
    {
        playerPos = playerDetails.player.transform.position;
        if (DamageCircle.instance.isOutstide(playerPos))
        {
            // damange the player;
        }
        else
        {

        }
        SetPlayerPos();
        SetPlayerDistanceToTarget();
        if(zoneTimer > 0)
        {
            zoneUI.SetWarning(false);
            zoneUI.SetStartCounter(zoneTimer);
            zoneUI.ZoneProgressToTarget(1, 1); // max to max , 1, 1 just a max to max;

        }
        else
        {
            zoneUI.SetWarning(true);
            zoneUI.SetStartCounter(0);
            SetZoneProgressToTarget();
        }



        StartCoroutine(UpdateTheZone());
    }
    Vector3 targetCircleSize;
    Vector3 targetCirclePosition;
    Vector3 previousCirclePosition;
    Vector3 previousCircleSize;
    public float zoneTimer;
    void SetPlayerPos()
    {
         targetCircleSize = DamageCircle.instance.TargetSize();
         targetCirclePosition = DamageCircle.instance.TargetPosition();
         previousCirclePosition = DamageCircle.instance.PreviousCirclePos();
         previousCircleSize = DamageCircle.instance.PreviousCircleSize();

         zoneTimer = DamageCircle.instance.Timer();
    }



    void SetPlayerDistanceToTarget()
    {
        float maxDist = Mathf.Abs(previousCircleSize.x - targetCircleSize.x);
        float distToTarget = Vector3.Distance(playerPos, targetCirclePosition) - targetCircleSize.x * 0.5f; 

        zoneUI.SetPlayerDistanceToTarget(maxDist, distToTarget);
        if(distToTarget >0)
        {
            playerToTargetnLineRender.SetPosition(0, playerDetails.transform.position);
            playerToTargetnLineRender.SetPosition(1, targetCirclePosition);
        }
        else
        {
            playerToTargetnLineRender.SetPosition(0, playerDetails.transform.position);
            playerToTargetnLineRender.SetPosition(1, playerDetails.transform.position);
        }
    }

    void SetZoneProgressToTarget( )
    {
        float maxDist = Vector3.Distance(previousCircleSize, targetCircleSize);
        float distToTarget = DamageCircle.instance.GetCurrentZoneProgress();

        zoneUI.ZoneProgressToTarget(maxDist, distToTarget);
    }

}
