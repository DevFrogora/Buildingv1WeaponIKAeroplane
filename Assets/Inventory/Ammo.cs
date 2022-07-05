using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    public ParticleSystem hitEffect;
    public ParticleSystem hitEffectPrefab;
    public AudioClip hitImpactSound;
    public enum AmmoType
    {
        FivePointFive,
        SevenPointSeven,
        NinePointNine
    };
    public AmmoType ammoType;

    public float time;
    public Vector3 initalPosition;
    public Vector3 initialVelocity;
    public float bulletSpeed = 1000f;
    public float bulletDrop = 0f;

    float maxLifeTime = 3f;


    Vector3 GetPosition()
    {
        // p + v*t + 0.5* g *t *t
        Vector3 gravity = Vector3.down * bulletDrop;
        return (initalPosition) + (initialVelocity * time) + (0.5f * gravity * time * time);
    }
    AudioSource audioSource;
    int i;
    private void Start()
    {
        i++;
        audioSource = GetComponent<AudioSource>();
        Debug.Log("i called this times"+ i);
        
    }
    bool isFired = false;

    public void CreateBullet(Transform muzzlePoint)
    {
        hitted = false;
        isFired = true;
        initalPosition = muzzlePoint.position;
        initialVelocity = muzzlePoint.forward.normalized * bulletSpeed;
        hitEffect = Instantiate(hitEffectPrefab);
        time = 0f;
    }

    void Updatebullet(float deltaTime)
    {
        SimulateBullets(deltaTime);
        DestroyBullet();
    }

    private void DestroyBullet()
    {
        if (time > maxLifeTime)
        {
            isFired = false;
            StartCoroutine(DisableAfterHitEffect());
            //gameObject.SetActive(false);

        }
    }

    void SimulateBullets(float deltatime)
    {
        Vector3 p0 = GetPosition();
        time += deltatime;
        Vector3 p1 = GetPosition();
        RaycastSegment(p0, p1);
    }

    Ray bulletRay;
    RaycastHit bulletHitInfo;
    bool hitted = false;
    void RaycastSegment(Vector3 start, Vector3 end)
    {
        Vector3 direction = end - start;
        float distance = direction.magnitude;
        bulletRay.origin = start;
        bulletRay.direction = direction;

        if(!hitted && time < maxLifeTime)
        {
            if (Physics.Raycast(bulletRay, out bulletHitInfo, distance))
            {
                hitted = true;
                time = maxLifeTime;
                //Debug.Log("we hit the collider no else part");
            }
        }


        if (time >= maxLifeTime && hitted)
        {
            hitted = false;
            hitEffect.transform.position = bulletHitInfo.point;
            hitEffect.transform.forward = bulletHitInfo.normal;
            //Instantiate(hitEffect, bulletHitInfo.point, bulletHitInfo.normal, null);
            transform.position = bulletHitInfo.point;
            hitEffect.Emit(1);
            audioSource.PlayOneShot(hitImpactSound);
            Debug.DrawLine(start, end, Color.red, 5);
            //StartCoroutine(DisableAfterHitEffect());


        }
        else if(!hitted && time < maxLifeTime)
        {
            Debug.DrawLine(start, end, Color.red, 5);
            transform.position = end;
            //Debug.Log(" else part");
        }

    }

    IEnumerator DisableAfterHitEffect()
    {
        yield return new WaitForSeconds(2f);
        Debug.Log("Destroying hit effect");
        Destroy(hitEffect.gameObject);
        gameObject.SetActive(false);
    }


    private void LateUpdate()
    {
        if(isFired)
        {
            Updatebullet(Time.deltaTime);
        }
    }



}
