using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLocation : MonoBehaviour
{
    //public int areaSize;
    public List<GameObject> Location;


    [System.Serializable]
    public class Item
    {
        // for debug :
        public string Name;

        public GameObject Prefab;
        [Range(0f, 100f)] public float Chance = 100f;

        [HideInInspector] public double _weight;
    }

    [SerializeField] private Item[] items;
    private double accumulatedWeights;
    private System.Random rand = new System.Random();

    private void Awake()
    {
        CalculateWeights();
    }

    public Item SpawnRandomItem(Vector3 position)
    {
        Item randomItem = items[GetRandomItemIndex()];

        //Instantiate(randomItem.Prefab, position, Quaternion.identity, transform);

        // This line is not required (debug) :
        return randomItem;
    }

    private int GetRandomItemIndex()
    {
        double r = rand.NextDouble() * accumulatedWeights;

        for (int i = 0; i < items.Length; i++)
            if (items[i]._weight >= r)
                return i;

        return 0;
    }

    private void CalculateWeights()
    {
        accumulatedWeights = 0f;
        foreach (Item item in items)
        {
            accumulatedWeights += item.Chance;
            item._weight = accumulatedWeights;
        }
    }

}
