using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering.UI;
using UnityEngine.Rendering;


public class ScreenShotManager : MonoBehaviour
{
    [SerializeField] private KeyCode screenShotKeyCode = KeyCode.A;
    private bool takeScreenShot;
    public GameObject uiGO;
    public string path;
    
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(screenShotKeyCode))
        {
            TakeScreenShot();
        }
    }

    private void TakeScreenShot()
    {
        path = Application.dataPath + "/Screenshot";
        takeScreenShot = true;
        uiGO.SetActive(false);
    }
    
    void RenderPipelineManagerEndCameraRendering(ScriptableRenderContext scriptableRenderContext, Camera camera)
    {
        if (takeScreenShot)
        {
            takeScreenShot = false;

            int width = camera.pixelWidth;
            int height = camera.pixelHeight;

            Texture2D screenShotTexture = new Texture2D(width, height, TextureFormat.ARGB32, false);

            Rect rect = new Rect(0, 0, width, height);
            
            screenShotTexture.ReadPixels(rect, 0,0);
            screenShotTexture.Apply();

            Byte[] bytes = screenShotTexture.EncodeToPNG();

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            
            File.WriteAllBytes(path + "/Screenshot " + width + "-" + height + " " + DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss") + " (" + Directory.GetFiles(path).Length +  ")" + ".png", bytes);
            
            uiGO.SetActive(true);
        }
    }

    private void OnEnable()
    {
        RenderPipelineManager.beginCameraRendering += RenderPipelineManagerEndCameraRendering;
    }

    private void OnDisable()
    {
        RenderPipelineManager.beginCameraRendering -= RenderPipelineManagerEndCameraRendering;
    }
}
