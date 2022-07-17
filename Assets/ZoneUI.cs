using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ZoneUI : MonoBehaviour
{

    public Slider zoneProgressToTarget;

    public Slider playerDistanceToTarget; // distance Totaget - radiusofTarget

    public Image warning;

    public TextMeshProUGUI startCounter;

    public TextMeshProUGUI distanceToTargetBelow;
    public TextMeshProUGUI distanceToTargetAbove;


    private void Start()
    {
        zoneProgressToTarget.value = 0;
        playerDistanceToTarget.value = 1;

        distanceToTargetAbove.text = "";
        distanceToTargetBelow.text = "";

        startCounter.text = "00:00";
        warning.color = Color.gray;
    }

    public void SetWarning(bool _warning)
    {
        if(_warning)
        {
            warning.color = Color.red;
        }
        else
        {
            warning.color = Color.gray;
        }
    }

    public void SetStartCounter(float time)
    {
        // format the time;
        int second = (int) time ;
        int minute = (int) second / 60;
        startCounter.text = minute.ToString("00")+":"+second.ToString("00");
        //Debug.Log(minute.ToString("00") + ":" + second.ToString("00"));
    }

    public void SetPlayerDistanceToTarget(float max, float distance)
    {
        float _range = Mathf.InverseLerp(max, 0, distance);
        //Debug.Log("max : " + max + " distane : " + distance + " _range : " + _range);
        playerDistanceToTarget.value = _range;
        if(distance > 0)
        {
            if (playerDistanceToTarget.value < 0.5f)
            {
                distanceToTargetAbove.text = distance.ToString("0") + "M";
                distanceToTargetBelow.text = "";
            }
            else
            {
                distanceToTargetBelow.text = distance.ToString("0") + "M";
                distanceToTargetAbove.text = "";
            }
        }else
        {
            distanceToTargetAbove.text = "";
            distanceToTargetBelow.text = "";
        }

    }

    public void ZoneProgressToTarget(float max ,float distance)
    {
        float _range = Mathf.InverseLerp(max, 0, distance);

        //Debug.Log("max : " + max + " distane : " + distance + " _range : " + _range);
        zoneProgressToTarget.value = _range;
    }


}
