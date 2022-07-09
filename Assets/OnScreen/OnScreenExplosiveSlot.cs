using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class OnScreenExplosiveSlot : MonoBehaviour
{

    public List<Sprite> sprites = new List<Sprite>();

    private void Start()
    {
        var dropdown = transform.GetComponent<TMP_Dropdown>();
        dropdown.options.Clear();

        //List<string> items = new List<string>();
        //items.Add("item 1");
        //items.Add("item 2");

        foreach (Sprite sprite in sprites)
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData() { image = sprite });
        }
    }


}
