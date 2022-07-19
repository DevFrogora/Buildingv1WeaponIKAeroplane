using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirDrop : MonoBehaviour
{
    public GameObject groundDetection;
    public GameObject canopy;
    public ParticleSystem Smoke;

    private Rigidbody airDropRb;
    private bool Landed = false;

    // Start is called before the first frame update
    void Start()
    {
        airDropRb = transform.GetComponent<Rigidbody>();
        Smoke.gameObject.SetActive(false);
        StartCoroutine(Updater());
        airDropRb.drag = 0.87f;
    }

    IEnumerator Updater()
    {
        yield return new WaitForEndOfFrame();
        RaycastHit raycastHit;

        if(Physics.Raycast(groundDetection.transform.position,Vector3.down,out raycastHit,1f))
        {
            Debug.Log("is i hitted ground");
            if(raycastHit.collider.tag != "Player")
            {
                Landed = true;
                Debug.Log("Landed : " + Landed);

            }
        }

        if(Landed)
        {
            DropHasLaned();
            Landed = false;
        }
        else
        {
            Debug.Log("Landed : " + Landed);

            StartCoroutine(Updater());
        }
    }

    void DropHasLaned()
    {
        Smoke.gameObject.SetActive(true);
        //StartCoroutine(SmokeTimer());
        StartCoroutine(SmokeTimeout());
        Destroy(canopy.gameObject);
    }

    float smokeTime = 60;
    //IEnumerator SmokeTimer()
    //{
        

    //    Smoke.Emit(10);
    //    while (smokeTime > 0 )
    //    {
    //        smokeTime -= 1;
    //        yield return new WaitForSeconds(0.5f);
    //        StartCoroutine(SmokeTimer());
    //    }
    //}

    IEnumerator SmokeTimeout()
    {
        yield return new WaitForSeconds(15);
        Destroy(airDropRb);
        Smoke.gameObject.SetActive(false);
    }



}
