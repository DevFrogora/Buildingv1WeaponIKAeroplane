using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GlidingUiControl : MonoBehaviour
{
    public TextMeshProUGUI altitudeText;
    public TextMeshProUGUI speedText;

    public Slider altitudeSlider;
    public Slider speedSlider;

    private void OnEnable()
    {
        altitudeText.gameObject.SetActive(true);
        speedText.gameObject.SetActive(true);

        altitudeSlider.gameObject.SetActive(true);
        speedSlider.gameObject.SetActive(true);
    }

    public void SetAltitude(int altitudeHeight,float remainingHeight)
    {
        altitudeText.text = altitudeHeight.ToString() + " M";
        altitudeSlider.value = remainingHeight; // normalised height , ( TotalHeight = StartingHeight) , (DistanceItCover = remainingHeight/TotalHeight)
    }

    public void SetSpeed(int dopwnSpeedOFPlayer, float normalizedSpeed)
    {
        speedText.text = dopwnSpeedOFPlayer.ToString() + " M/s";
        speedSlider.value = normalizedSpeed; // normalised speed , ( MaxSpeed ) , (normalizedSpeed = dopwnSpeedOFPlayer/MaxSpeed)
    }

    private void OnDisable()
    {
        altitudeText.gameObject.SetActive(false);
        speedText.gameObject.SetActive(false);

        altitudeSlider.gameObject.SetActive(false);
        speedSlider.gameObject.SetActive(false);
    }
}
