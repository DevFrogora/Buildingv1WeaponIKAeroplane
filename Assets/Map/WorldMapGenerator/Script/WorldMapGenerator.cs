#define TESTING
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

// Polygon Pilgriamge
// Added Grid 
// WorldMapGenerator V2.0

public class WorldMapGenerator : MonoBehaviour
{
    [Header("Camera")]
    [Tooltip("Drag and drop your camera here")]
    public GameObject targetCamera;
    private Camera myCamera;

    [Tooltip("A height above the highest terrain point to clear buildings or other tall structures")]
    public float CameraHeight = 2472;

    [Header("Terrain")]
    [Tooltip("Drag and drop your terrain here")]
    public GameObject myTerrain;

    [Header("Settings")]
    [Tooltip("Choose the size of the final master map texture (in pixels")]
    public MasterTextureSizeEnum MasterTextureSize;

    public enum MasterTextureSizeEnum { x2048 = 2048, x4096 = 4096, x8192 = 8192}

    [Header("Outer Grid Image")]
    [Tooltip("Select the desired Outer Grid type")]
    public OuterGridEnum OuterGrid;

    public enum OuterGridEnum {OuterThick, OuterThin}

    private RawImage outerGridRaw;

    public InnerGridEnum InnerGrid;

    public enum InnerGridEnum {Dotted, Full, CenterDots}

    private RawImage innerGridRaw;

    [Header("Buildings Material")]
    [Tooltip("Drag and drop the material you want the buildings to be for the World Map rendering")]
    public Material BuildingsMaterial;

    private Material[][] originalMaterials;
    private GameObject[] WMBuildings;

    [ContextMenu("Generate World Map Image")]
    public void GenerateWorldMap()
    {
        //Swap Building Materials before we render the World Map
        BuildingMaterialSwap("start");
        RoadMaterialSwap("start");
        CityMaterialSwap("start");

        //Get a reference to the OuterGrid image
        outerGridRaw = GameObject.Find("Outer").GetComponent<RawImage>();

        //Get a reference to the InnerGrid image
        innerGridRaw = GameObject.Find("Inner").GetComponent<RawImage>();

        //Setup the grid images
        //SetUpGridImages();


        Vector3 MapSize = new Vector3();

         if(myTerrain !=null)
        {
            var terrainDataGrab = myTerrain.GetComponent<Terrain>().terrainData.size;
            MapSize = new Vector3((terrainDataGrab.x),(terrainDataGrab.y),(terrainDataGrab.z));
        }

        int width = (int)MasterTextureSize;
        int height = width;

        targetCamera.transform.position = new Vector3(0, (CameraHeight), 0);

        //Set Camera settings
        float OrthoSize = (MapSize.x /2);
        Camera.main.orthographic = true;
        Camera.main.orthographicSize = OrthoSize;

        //Rendering the image, and saving it
        myCamera = targetCamera.GetComponent<Camera>();
        RenderTexture rt = new RenderTexture(width, height, 16);
        myCamera.targetTexture = rt;
        Texture2D screenshot = new Texture2D(width, height, TextureFormat.RGB24, false);
        myCamera.Render();
        RenderTexture.active = rt;
        screenshot.ReadPixels(new Rect(0,0,width,height),0,0);
        myCamera.targetTexture = null;
        RenderTexture.active = null;
        DestroyImmediate(rt);
        byte[] bytes = screenshot.EncodeToTGA();
        System.IO.File.WriteAllBytes(Application.dataPath + "/Screenshots/" + "_" + "WorldMap" + ".tga", bytes);

        AssetDatabase.Refresh();
        BuildingMaterialSwap("end");
        RoadMaterialSwap("end");
        CityMaterialSwap("end");


        Debug.Log("World Map Generated!");
    }

    public void BuildingMaterialSwap(string mode)
    {
            //Get an array of all objects that have the layer assignment of Buildings
            WMBuildings = GameObject.FindGameObjectsWithTag("Buildings");

            //Number of buildings
            int numBuildings = WMBuildings.Length;

        if(mode =="start")
        {
            //Create a new Material Array that is numBuildings long
            originalMaterials = new Material[numBuildings][];

            //Loop through all of the buildings
            for(int i=0; i < numBuildings; i++)
            {
                //originalMaterials[i] = WMBuildings[i].GetComponent<Renderer>().sharedMaterials[0];

                //Find the number of materials on this building
                Renderer ren = WMBuildings[i].GetComponent<Renderer>();
                int numMaterials = ren.sharedMaterials.Length;

                originalMaterials[i] = new Material[numMaterials];

                //Loop through all the materials on each building, and save that material into the array
                for(int j=0; j < numMaterials; j++)
                {
                    originalMaterials[i][j] = ren.sharedMaterials[j];
                }
            }

            //Assign the BuildingsMaterial to the buildings
            for(int i=0; i < numBuildings; i++)
            {
                Renderer ren = WMBuildings[i].GetComponent<Renderer>();
                var mats = new Material[ren.sharedMaterials.Length];

                //Loop through all the materials on each building, and set it to the BuildingsMaterial
                for(int j=0; j < mats.Length; j++)
                {
                    mats[j] = BuildingsMaterial;
                }
                ren.sharedMaterials = mats;
            }
        }

        else if(mode =="end")
        {
            //Loop through all of the buildings
            for(int i=0; i < numBuildings; i++)
            {
                //Find the number of materials on this building
                Renderer ren = WMBuildings[i].GetComponent<Renderer>();
                int numMaterials = ren.sharedMaterials.Length;
                ren.sharedMaterials = originalMaterials[i];
            }
        }
        else
        {
            Debug.LogError("Mode was not set to either start or end");
        }
    }

    public Material StateRoadsMaterial;

    private Material[][] originalStateRoadMaterials;
    private GameObject[] WMStateRoads;
    public void RoadMaterialSwap(string mode)
    {
        //Get an array of all objects that have the layer assignment of Buildings
        WMStateRoads = GameObject.FindGameObjectsWithTag("StateRoad");

        //Number of buildings
        int numRoads = WMStateRoads.Length;

        if (mode == "start")
        {
            //Create a new Material Array that is numBuildings long
            originalStateRoadMaterials = new Material[numRoads][];

            //Loop through all of the buildings
            for (int i = 0; i < numRoads; i++)
            {
                //originalMaterials[i] = WMBuildings[i].GetComponent<Renderer>().sharedMaterials[0];

                //Find the number of materials on this building
                Renderer ren = WMStateRoads[i].GetComponent<Renderer>();
                int numMaterials = ren.sharedMaterials.Length;

                originalStateRoadMaterials[i] = new Material[numMaterials];

                //Loop through all the materials on each building, and save that material into the array
                for (int j = 0; j < numMaterials; j++)
                {
                    originalStateRoadMaterials[i][j] = ren.sharedMaterials[j];
                }
            }

            //Assign the BuildingsMaterial to the buildings
            for (int i = 0; i < numRoads; i++)
            {
                Renderer ren = WMStateRoads[i].GetComponent<Renderer>();
                var mats = new Material[ren.sharedMaterials.Length];

                //Loop through all the materials on each building, and set it to the BuildingsMaterial
                for (int j = 0; j < mats.Length; j++)
                {
                    mats[j] = StateRoadsMaterial;
                }
                ren.sharedMaterials = mats;
            }
        }

        else if (mode == "end")
        {
            //Loop through all of the buildings
            for (int i = 0; i < numRoads; i++)
            {
                //Find the number of materials on this building
                Renderer ren = WMStateRoads[i].GetComponent<Renderer>();
                int numMaterials = ren.sharedMaterials.Length;
                ren.sharedMaterials = originalStateRoadMaterials[i];
            }
        }
        else
        {
            Debug.LogError("Mode was not set to either start or end");
        }
    }



    public Material CityRoadsMaterial;

    private Material[][] originalCityRoadMaterials;
    private GameObject[] WMCityRoads;
    public void CityMaterialSwap(string mode)
    {
        //Get an array of all objects that have the layer assignment of Buildings
        WMCityRoads = GameObject.FindGameObjectsWithTag("CityRoad");

        //Number of buildings
        int numRoads = WMCityRoads.Length;

        if (mode == "start")
        {
            //Create a new Material Array that is numBuildings long
            originalCityRoadMaterials = new Material[numRoads][];

            //Loop through all of the buildings
            for (int i = 0; i < numRoads; i++)
            {
                //originalMaterials[i] = WMBuildings[i].GetComponent<Renderer>().sharedMaterials[0];

                //Find the number of materials on this building
                Renderer ren = WMCityRoads[i].GetComponent<Renderer>();
                int numMaterials = ren.sharedMaterials.Length;

                originalCityRoadMaterials[i] = new Material[numMaterials];

                //Loop through all the materials on each building, and save that material into the array
                for (int j = 0; j < numMaterials; j++)
                {
                    originalCityRoadMaterials[i][j] = ren.sharedMaterials[j];
                }
            }

            //Assign the BuildingsMaterial to the buildings
            for (int i = 0; i < numRoads; i++)
            {
                Renderer ren = WMCityRoads[i].GetComponent<Renderer>();
                var mats = new Material[ren.sharedMaterials.Length];

                //Loop through all the materials on each building, and set it to the BuildingsMaterial
                for (int j = 0; j < mats.Length; j++)
                {
                    mats[j] = CityRoadsMaterial;
                }
                ren.sharedMaterials = mats;
            }
        }

        else if (mode == "end")
        {
            //Loop through all of the buildings
            for (int i = 0; i < numRoads; i++)
            {
                //Find the number of materials on this building
                Renderer ren = WMCityRoads[i].GetComponent<Renderer>();
                int numMaterials = ren.sharedMaterials.Length;
                ren.sharedMaterials = originalCityRoadMaterials[i];
            }
        }
        else
        {
            Debug.LogError("Mode was not set to either start or end");
        }
    }



    string gridFolderPath = "Assets/Map/WorldMapGenerator";
    public void SetUpGridImages()
    {
        switch (OuterGrid)
        {
            case OuterGridEnum.OuterThick:
                outerGridRaw.texture = (Texture2D)AssetDatabase.LoadAssetAtPath(gridFolderPath + "/OuterGrids/8K_Outer_Thick.png", typeof(Texture2D));
                break;

            case OuterGridEnum.OuterThin:
                outerGridRaw.texture = (Texture2D)AssetDatabase.LoadAssetAtPath(gridFolderPath + "/OuterGrids/8K_Outer_Thin_2.png", typeof(Texture2D));
                break;
        }

        switch (InnerGrid)
        {
            case InnerGridEnum.Full:
                innerGridRaw.texture = (Texture2D)AssetDatabase.LoadAssetAtPath(gridFolderPath + "/InnerGrids/8K_Inner_Full.png", typeof(Texture2D));
                break;

            case InnerGridEnum.Dotted:
                innerGridRaw.texture = (Texture2D)AssetDatabase.LoadAssetAtPath(gridFolderPath + "/InnerGrids/8K_Inner_Dotted.png", typeof(Texture2D));
                break;

            case InnerGridEnum.CenterDots:
                innerGridRaw.texture = (Texture2D)AssetDatabase.LoadAssetAtPath(gridFolderPath+"/InnerGrids/8K_Inner_CenterDots.png", typeof(Texture2D));
                break;
        }
    }
}