using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCircle : MonoBehaviour
{
    public static DamageCircle instance;
    [SerializeField] private Transform targetCircleTransform;

    private Transform circleTransform;
    private Transform topTransform;
    private Transform bottomTransform;
    private Transform leftTransform;
    private Transform rightTransform;


    private Vector3 circleSize;
    private Vector3 circlePosition;

    public float circleShrinkSpeed;

    private Vector3 targetCircleSize;
    private Vector3 targetCirclePosition;

    private float shrinkTimer;

    private Vector3 previousCirclePos;
    private Vector3 previousCircleSize;

    public float zoneSize= 2500;
    private void Awake()
    {
        instance = this;
        circleShrinkSpeed = 10f;
        circleTransform = transform.Find("circle");
        topTransform = transform.Find("top");
        bottomTransform = transform.Find("bottom");
        leftTransform = transform.Find("left");
        rightTransform = transform.Find("right");

        SetCircleSize(Vector3.up * 20,new Vector3(zoneSize, zoneSize, 1));
        previousCirclePos = Vector3.up * 20;
        previousCircleSize = new Vector3(2500, 2500, 1);

        //targetCircleSize = new Vector3(200, 200, 1);
        //targetCirclePosition = new Vector3(1000, 20, 0);
        zoneSize = zoneSize / 2;
        SetTargetCircle(new Vector3(200, 20, 1), new Vector3(zoneSize, zoneSize, 0),5f);  // after 5 second it will shrink
    }
    public float distanceOfTarget;

    private void Update()
    {
        shrinkTimer -= Time.deltaTime;
        if(shrinkTimer < 0)
        {
            Vector3 sizeChangeVector = (targetCircleSize - circleSize).normalized;  // gives you -ve normalized Vector
            Vector3 newCircleSize = circleSize + sizeChangeVector * Time.deltaTime * circleShrinkSpeed; // adding +ve with -ve the answer will be small

            Vector3 circleMoveDir = (targetCirclePosition - circlePosition).normalized;
            Vector3 newCirclePosition = circlePosition + circleMoveDir * Time.deltaTime * circleShrinkSpeed;

            SetCircleSize(newCirclePosition, newCircleSize);

            float distanceTestAmount = 0.1f;
            // checking when shrinking is completen;

            distanceOfTarget = Vector3.Distance(newCircleSize, targetCircleSize);
            //Debug.Log(distanceOfTarget);  // <-- successor of ZOneProgress

            if (Vector3.Distance(newCircleSize , targetCircleSize ) < distanceTestAmount && Vector3.Distance(newCirclePosition, targetCirclePosition) < distanceTestAmount)
            {
                previousCirclePos = circlePosition;
                previousCircleSize = circleSize;
                //SetTargetCircle(circlePosition, circleSize - new Vector3(200, 200, 0), 5);
                GenerateTargetCircle();
            }
        }

    }

    private void GenerateTargetCircle()
    {
        zoneSize = zoneSize / 2;
        float shrinkAmount = zoneSize;
        Vector3 generatedTargetCircleSize = circleSize - new Vector3(shrinkAmount, shrinkAmount);
        Vector3 generatedTargetCirclePosition = circlePosition + new Vector3(Random.Range(-shrinkAmount, shrinkAmount)
            , 20, Random.Range(-shrinkAmount, shrinkAmount)) ;

        shrinkTimer = 5;
        SetTargetCircle(generatedTargetCirclePosition, generatedTargetCircleSize ,shrinkTimer);
    }


    private void SetCircleSize(Vector3 position,Vector3 size)
    {

        circlePosition = position;
        circleSize = size;

        transform.position = position;

        int topDownblockSize = 5000;
        circleTransform.localScale = size;

        topTransform.localScale = new Vector3(topDownblockSize, topDownblockSize, 1);
        topTransform.localPosition = new Vector3(0, 0, topTransform.localScale.y * 0.5f + size.y * 0.5f);

        bottomTransform.localScale = new Vector3(topDownblockSize, topDownblockSize, 1);
        bottomTransform.localPosition = new Vector3(0, 0, -bottomTransform.localScale.y * 0.5f - size.y * 0.5f);

        leftTransform.localScale = new Vector3(topDownblockSize, size.y, 1);
        leftTransform.localPosition = new Vector3(-leftTransform.localScale.x * 0.5f - size.x * 0.5f, 0, 0);

        rightTransform.localScale = new Vector3(topDownblockSize, size.y, 1);
        rightTransform.localPosition = new Vector3(rightTransform.localScale.x * 0.5f + size.x * 0.5f, 0, 0);
    }

    private void SetTargetCircle(Vector3 position, Vector3 size , float _shrinkTImer)
    {
        shrinkTimer = _shrinkTImer;

        targetCircleTransform.position = position;
        targetCircleTransform.localScale = size;

        targetCirclePosition = position;
        targetCircleSize = size;
    }


    // pass player Position
    public bool isOutstide(Vector3 position)
    {
        float distance = Vector3.Distance(position, circlePosition);
        float circleRadius =  circleSize.x * 0.5f;

        return Vector3.Distance(position, circlePosition) > circleSize.x * 0.5f;  //radius : circleSize.x * 0.5f
    }

    public float distanceToTarget(Vector3 position)
    {
        return Vector3.Distance(position, targetCirclePosition) - targetCircleSize.x * 0.5f;
    }

    public Vector3 TargetPosition()
    {
        return targetCirclePosition;
    }

    public Vector3 TargetSize()
    {
        return targetCircleSize;
    }

    public Vector3 PreviousCirclePos()
    {
        return previousCirclePos;
    }

    public Vector3 PreviousCircleSize()
    {
        return previousCircleSize;
    }

    public float GetCurrentZoneProgress()
    {
        return distanceOfTarget;
    }

    public float Timer()
    {
        return shrinkTimer;
    }



}
