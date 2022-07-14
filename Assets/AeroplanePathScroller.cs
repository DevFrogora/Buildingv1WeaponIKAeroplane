using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AeroplanePathScroller : MonoBehaviour
{
    public float ScrollXpos;
    public float ScrollYpos;

    Material pathRendererMat;
    // Start is called before the first frame update
    void Start()
    {
        pathRendererMat = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        float UpdatedXPos = Time.time * ScrollXpos;
        float UpdatedYPos = Time.time * ScrollYpos;
        pathRendererMat.mainTextureOffset = new Vector2(UpdatedXPos, UpdatedYPos);

    }
}
