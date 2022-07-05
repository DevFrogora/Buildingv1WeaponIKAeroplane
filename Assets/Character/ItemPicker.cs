using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemPicker : MonoBehaviour
{
    [SerializeField] private Transform _pickerPoint1;
    [SerializeField] private Transform _pickerPoint2;
    [SerializeField] private Transform _pickerPoint3;
    [SerializeField] private Transform _pickerPoint4;
    [SerializeField] private float _pickerPointRadius = 0.5f;
    [SerializeField] private LayerMask _pickerLayerMask;

    [SerializeField] private  Collider[] _colliders1 = new Collider[10]; // 3 collider (item ) we are checking
    [SerializeField] private  Collider[] _colliders2 = new Collider[10]; // 3 collider (item ) we are checking
    [SerializeField] private  Collider[] _colliders3 = new Collider[10]; // 3 collider (item ) we are checking
    [SerializeField] private  Collider[] _colliders4 = new Collider[10]; // 3 collider (item ) we are checking
    [SerializeField] private int _numFound1; // number of collider(item found)
    [SerializeField] private int _numFound2; // number of collider(item found)
    [SerializeField] private int _numFound3; // number of collider(item found)
    [SerializeField] private int _numFound4; // number of collider(item found)

    //template For Picker UI // rename it later
    public Image pickerUIPrefab;
    public GameObject pickerUIContainer;

    public Dictionary<int, Image> itemsUIlist = new Dictionary<int, Image>();

    int previousItemCount1;
    int previousItemCount2;
    int previousItemCount3;
    int previousItemCount4;

    private void Update()
    {
        _numFound1 = Physics.OverlapSphereNonAlloc(_pickerPoint1.position, _pickerPointRadius,
            _colliders1, _pickerLayerMask);
        _numFound2 = Physics.OverlapSphereNonAlloc(_pickerPoint2.position, _pickerPointRadius,
            _colliders2, _pickerLayerMask);
        _numFound3 = Physics.OverlapSphereNonAlloc(_pickerPoint3.position, _pickerPointRadius,
            _colliders3, _pickerLayerMask);
        _numFound4 = Physics.OverlapSphereNonAlloc(_pickerPoint4.position, _pickerPointRadius,
            _colliders4, _pickerLayerMask);

        if (_numFound1 > 0)
        {
            if (previousItemCount1 != _numFound1)
            {
                Sphere(_numFound1, _colliders1);
                previousItemCount1 = _numFound1;
            }
        }
        else
        {
            clearCollider(ref previousItemCount1,_colliders1);
        }

        if (_numFound2 > 0)
        {
            if (previousItemCount2 != _numFound2)
            {
                Sphere(_numFound2,_colliders2);
                previousItemCount2 = _numFound2;
            }

        }
        else
        {
            clearCollider(ref previousItemCount2,_colliders2);
        }

        if (_numFound3 > 0)
        {
            if (previousItemCount3 != _numFound3)
            {
                Sphere(_numFound3, _colliders3);
                previousItemCount3 = _numFound3;
            }

        }
        else
        {
            clearCollider(ref previousItemCount3, _colliders3);
        }

        if (_numFound4 > 0)
        {
            if (previousItemCount4 != _numFound4)
            {
                Sphere(_numFound4, _colliders4);
                previousItemCount4 = _numFound4;
            }

        }
        else
        {
            clearCollider(ref previousItemCount4, _colliders4);
        }



    }

    void Sphere(int _numFound1, Collider[] _colliders)
    {
        for (int i = 0; i < _numFound1; i++)
        {
            var foundItem = _colliders[i].GetComponent<IInventoryItem>();
            if (itemsUIlist.ContainsKey(foundItem.ItemId))
            {

            }
            else
            {
                var itemUiForPickup = Instantiate(pickerUIPrefab);
                PickerItemUI pickedItemData = itemUiForPickup.GetComponent<PickerItemUI>();
                itemUiForPickup.gameObject.transform.SetParent(pickerUIContainer.transform);
                itemUiForPickup.rectTransform.localScale = pickerUIPrefab.transform.localScale;
                itemUiForPickup.rectTransform.rotation = Quaternion.identity;
                pickedItemData.image.sprite = foundItem.spriteImage;
                pickedItemData.itemName.text = foundItem.Name;
                pickedItemData.itemPrefab = _colliders[i].gameObject;
                pickedItemData.itemId = pickedItemData.itemPrefab.GetComponent<IInventoryItem>().ItemId;

                itemsUIlist.Add(pickedItemData.itemId, itemUiForPickup);
            }
        }
    }



    void clearCollider(ref int previousItemCount , Collider[] colliders)
    {
        for(int i = 0; i < previousItemCount; i ++)
        {
            var foundItem = colliders[i].GetComponent<IInventoryItem>();
            if (itemsUIlist.ContainsKey(foundItem.ItemId))
            {
                Image imageToDelete;
                itemsUIlist.TryGetValue(foundItem.ItemId, out imageToDelete);
                Destroy(imageToDelete.gameObject);
                itemsUIlist.Remove(foundItem.ItemId);
            }
            colliders[i] = null;
        }
        previousItemCount = 0;
    }



    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(_pickerPoint1.position, _pickerPointRadius);
        //Gizmos.color = Color.green;
        //Gizmos.DrawWireSphere(_pickerPoint2.position, _pickerPointRadius);
        //Gizmos.color = Color.yellow;
        //Gizmos.DrawWireSphere(_pickerPoint3.position, _pickerPointRadius);
        //Gizmos.color = Color.blue;
        //Gizmos.DrawWireSphere(_pickerPoint4.position, _pickerPointRadius);
    }
}

