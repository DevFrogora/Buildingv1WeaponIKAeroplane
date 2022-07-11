using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class WorldMapGeneratorv1 : MonoBehaviour
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

    public enum MasterTextureSizeEnum { x2048 = 2048, x4096 = 4096, x8192 = 8192 }


    [ContextMenu("Generate World Map Image")]
    public void GenerateWorldMap()
    {

        Vector3 MapSize = new Vector3();

        if (myTerrain != null)
        {
            var terrainDataGrab = myTerrain.GetComponent<Terrain>().terrainData.size;
            MapSize = new Vector3((terrainDataGrab.x), (terrainDataGrab.y), (terrainDataGrab.z));
        }

        int width = (int)MasterTextureSize;
        int height = width;

        targetCamera.transform.position = new Vector3(0, (CameraHeight), 0);

        myCamera = targetCamera.GetComponent<Camera>();

        //Set Camera settings
        float OrthoSize = (MapSize.x / 2);
        myCamera.orthographic = true;
        myCamera.orthographicSize = OrthoSize;

        RenderTexture rt = new RenderTexture(width, height, 16);
        myCamera.targetTexture = rt;
        Texture2D screenshot = new Texture2D(width, height, TextureFormat.RGB24, false);
        myCamera.Render();
        RenderTexture.active = rt;
        screenshot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        myCamera.targetTexture = null;
        RenderTexture.active = null;
        DestroyImmediate(rt);
        byte[] bytes = screenshot.EncodeToTGA();
        System.IO.File.WriteAllBytes(Application.dataPath + "/Screenshots/" + "_" + "WorldMap" + ".tga", bytes);

        AssetDatabase.Refresh();
        Debug.Log("World Map Generated!");
    }

}