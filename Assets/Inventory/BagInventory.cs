using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagInventory : MonoBehaviour
{
    // we have to make it scriptableObject
    public static BagInventory instance;

    private void Awake()
    {
        instance = this;
    }

    public int MaxSize;
    public int currentSize;
    public Transform itemParent;
    public Transform activeWeaponParent;


    [System.Serializable]
    public class AssultSlot
    {
        public GameObject assultPrefab;
        public GameObject extendedMag;
        public GameObject supressor;
        public GameObject sight;

    }

    public class PistolSlot
    {
        public GameObject pistolPrefab;
        public GameObject extendedMag;
        public GameObject sight;

    }

    public AssultSlot slot1;
    public AssultSlot slot2;
    public PistolSlot slot3;

    public bool activeSlot1;
    public bool activeSlot2;
    public bool activeSlot3;



    public void ActiveSlot1(bool _activeSlot1)
    {
        BagUIBroadcast.instance.ActiveSlot(1,_activeSlot1);
        activeSlot1 = _activeSlot1;
        if (_activeSlot1)
        {
            slot1.assultPrefab.gameObject.transform.SetParent(activeWeaponParent, false);
            ActiveSlot2(false);
            //activeSlot3 = false; Change it to ActiveSlot3(false)
        }
        else
        {
            if(slot1.assultPrefab != null)
            slot1.assultPrefab.gameObject.transform.SetParent(itemParent, false);
        }

    }



    public void ActiveSlot2(bool _activeSlot2)
    {
        BagUIBroadcast.instance.ActiveSlot(2, _activeSlot2);
        activeSlot2 = _activeSlot2;
        if (_activeSlot2)
        {
            slot2.assultPrefab.gameObject.transform.SetParent(activeWeaponParent, false);
            ActiveSlot1(false);
            //activeSlot3 = false; Change it to ActiveSlot3(false)
        }
        else
        {
            if(slot2.assultPrefab != null)
            slot2.assultPrefab.gameObject.transform.SetParent(itemParent, false);
        }

    }


    public List<GameObject> mixItem = new List<GameObject>();

    public void SetSlot1Assult(GameObject weapon)
    {
      
        //if(slot1.assultPrefab != null)
        //{
        //    //Drop The item If destroy
        //    Destroy(slot1.assultPrefab.gameObject);
        //}
        if (weapon == null)
        {
            slot1.assultPrefab = null;
            BagUIBroadcast.instance.Slot1AssultAdded(weapon);
        }
        else
        {
            if (weapon.GetComponent<m416>())
            {
                weapon.gameObject.transform.SetParent(itemParent, false);
                //weapon.gameObject.transform.SetParent(activeWeaponParent, false);
                weapon.gameObject.transform.localPosition = new Vector3(0,0, 0.303f); //was  vecto3.zero 
                slot1.assultPrefab = weapon;
                BagUIBroadcast.instance.Slot1AssultAdded(weapon);
                BagUIBroadcast.instance.ActiveSlot(1,true);
                ActiveSlot1(true);


            }
        }
    }

    public void SetSlot1Sight(GameObject sight)
    {
        if (sight == null)
        {
            slot1.sight = null;
            BagUIBroadcast.instance.Slot1SightAdded(sight);
        }
        else
        {
            if (sight.GetComponent<RedDotSight>())
            {
                sight.gameObject.transform.SetParent(itemParent, false);
                sight.gameObject.transform.localPosition = Vector3.zero;
                slot1.sight = sight;
                mixItem.Remove(sight);
                BagUIBroadcast.instance.Slot1SightAdded(sight);
            }
        }
    }



    //public void SetSlot1ExtendedMag(GameObject mag)
    //{
    //    //if (slot1.extendedMag != null)
    //    //{
    //    //    //Drop The item If destroy
    //    //    Destroy(slot1.extendedMag.gameObject);
    //    //}
    //    if (mag.GetComponent<ExtendedMag>())
    //    {
    //        slot1.extendedMag = mag;
    //    }
    //}

    //public void SetSlot1SightMag(GameObject sight)
    //{
    //    //if (slot1.extendedMag != null)
    //    //{
    //    //    //Drop The item If destroy
    //    //    Destroy(slot1.extendedMag.gameObject);
    //    //}
    //    if (sight.GetComponent<ExtendedMag>())
    //    {
    //        slot1.extendedMag = sight;
    //    }
    //}


    public void SetSlot2Assult(GameObject weapon)
    {
        if (weapon == null)
        {
            slot2.assultPrefab = null;
            BagUIBroadcast.instance.Slot2AssultAdded(weapon);
        }
        else
        {
            if (weapon.GetComponent<m416>())
            {
                weapon.gameObject.transform.SetParent(itemParent, false);
                weapon.gameObject.transform.localPosition = Vector3.zero;
                slot2.assultPrefab = weapon;
                BagUIBroadcast.instance.Slot2AssultAdded(weapon);
            }
        }

    }

    public void AddInMixItem(GameObject item)
    {
        item.gameObject.transform.SetParent(itemParent, false);
        item.gameObject.transform.localPosition = Vector3.zero;
        mixItem.Add(item);
        BagUIBroadcast.instance.SlotMixItemAdded(item);
    }
}
