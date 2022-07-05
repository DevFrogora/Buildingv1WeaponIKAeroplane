using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    public static AnimatorManager instance;
    public enum AnimatorLayer
    { 
        Default,
        Land, 
        Aeroplane,
        Gliding,
        Swimming,
        ParaGliding,
        Emote,
    }

    private void Awake()
    {
        instance = this;
    }

}