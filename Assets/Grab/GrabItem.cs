using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GrabItem : MonoBehaviour 
{
    [SerializeField] private Transform _pickerPoint1;

    [SerializeField] private float _pickerPointRadius = 0.5f;
    [SerializeField] private LayerMask _pickerLayerMask;

    [SerializeField] private Collider[] _colliders1 = new Collider[10]; // 3 collider (item ) we are checking

    [SerializeField] private int _numFound1; // number of collider(item found)


    //template For Picker UI // rename it later
    public Image pickerUIPrefab;
    public GameObject pickerUIContainer;

    public Dictionary<int, Image> itemsUIlist = new Dictionary<int, Image>();

    int previousItemCount1;


    private void Update()
    {
        _numFound1 = Physics.OverlapSphereNonAlloc(_pickerPoint1.position, _pickerPointRadius,
            _colliders1, _pickerLayerMask);

        if (_numFound1 > 0 )
        {
            if (previousItemCount1 != _numFound1)
            {
                Sphere(_numFound1, _colliders1);
                previousItemCount1 = _numFound1;
            }
        }
        else
        {
            clearCollider(ref previousItemCount1, _colliders1);
        }
    }


    void Sphere(int _numFound1, Collider[] _colliders)
    {
        for (int i = 0; i < _numFound1; i++)
        {
            IGrab foundItem = _colliders[i].GetComponent<IGrab>();
            if (foundItem == null) return;
            if (itemsUIlist.ContainsKey(foundItem.ItemId))
            {

            }
            else
            {
                var itemUiForPickup = Instantiate(pickerUIPrefab);
                GrabPickUI pickedItemData = itemUiForPickup.GetComponent<GrabPickUI>();
                itemUiForPickup.gameObject.transform.SetParent(pickerUIContainer.transform);
                itemUiForPickup.rectTransform.localScale = pickerUIPrefab.transform.localScale;
                itemUiForPickup.rectTransform.rotation = Quaternion.identity;
                pickedItemData.image.sprite = foundItem.spriteImage;
                pickedItemData.itemName.text = foundItem.Name;
                pickedItemData.itemPrefab = _colliders[i].gameObject;
                pickedItemData.itemId = pickedItemData.itemPrefab.GetComponent<IGrab>().ItemId;

                itemsUIlist.Add(pickedItemData.itemId, itemUiForPickup);
            }
        }
    }



    void clearCollider(ref int previousItemCount, Collider[] colliders)
    {
        for (int i = 0; i < previousItemCount; i++)
        {
            var foundItem = colliders[i].GetComponent<IGrab>();
            if (foundItem == null) {
                colliders[i] = null;
                return;
            }
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
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_pickerPoint1.position, _pickerPointRadius);
    }
}
