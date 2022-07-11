using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldMapIcons : MonoBehaviour
{

    public Canvas canvas;

    public Terrain terrain;

    public Vector2 TerrainDimensions;

    public GameObject player;

    public Vector2 playerMapPos;

    public Vector2 playerMapIconPos;

    public Image playerIcon;

    public Image worldMapImage;

    public Vector2 worldMapImageSize;

    // Start is called before the first frame update
    void Start()
    {
        worldMapImageSize.x = worldMapImage.rectTransform.rect.width;
        worldMapImageSize.y = worldMapImage.rectTransform.rect.height;
        
        TerrainDimensions.x = terrain.terrainData.size.x;
        TerrainDimensions.y = terrain.terrainData.size.z;

    }

    // Update is called once per frame
    void Update()
    {
        //Get the player's map position (x and z only)
        playerMapPos.x = player.transform.position.x;
        playerMapPos.y = player.transform.position.z;

        playerMapIconPos.x = worldMapImageSize.x * (GetNormalizedValue(playerMapPos.x, new Vector2((-(TerrainDimensions.x/2)),(TerrainDimensions.x/2))));
        //Debug.Log("x is: " + playerMapIconPos.x);

        playerMapIconPos.y = worldMapImageSize.y * (GetNormalizedValue(playerMapPos.y, new Vector2((-(TerrainDimensions.y/2)),(TerrainDimensions.y/2))));
        
        //playerIcon.transform.position = new Vector2(playerMapIconPos.x,playerMapIconPos.y);
        //Get screen size offset
        //Our base image is 1920 x 1080.  This is a nice 1080P resolution, however our map is square and it scaled to be
        //as tall as the screen (1080) but not as wide, since it is a square.  So this means that we need to find the
        //difference between the width of the screen and the height (in this case 1920-1080 = 840), then get half that
        //value as our offset is taking into consideration a world center and screen center 0,0
        //So (1920-1080) / 2 = 420
        //Now we get that value and add it to our x, since our x is calculated to be within the bounds of the world map
        //image, but that image is in the middle of the screen, so we still need to account for moving the icon from
        //the far left edge of the screen, not just from the left edge of the world map image

        float screenOffset = (canvas.GetComponent<RectTransform>().rect.width - canvas.GetComponent<RectTransform>().rect.height) / 2;
        Debug.Log(screenOffset);
        playerIcon.transform.position = new Vector2(screenOffset + playerMapIconPos.x,playerMapIconPos.y);

    }

    public float GetNormalizedValue(float raw, Vector2 minMax)
    {
        //scaledValue = (rawValue - min) / (max - min);
        var returnValue = ((raw - minMax.x) / (minMax.y - minMax.x));
        return returnValue;
    }
}
