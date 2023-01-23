using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyHover : MonoBehaviour
{
    [SerializeField] private Image image;
    private RectTransform rt;
    [SerializeField] private float scaleMax;
    [SerializeField] private float scaleMin;
    private bool isHovering;
    private bool needUpdate;

    private void Start()
    {
        rt = image.rectTransform;
    }

    private void Update()
    {
        if (isHovering) IncreaseSize();
        else DecreaseSize();
        
    }

    private void IncreaseSize()
    {
        if (!needUpdate) return;
        if (rt.localScale.x > scaleMax)
        {
            rt.localScale = Vector3.one * scaleMax;
            needUpdate = false;
            return;
        }
        
        rt.localScale += Vector3.one * (Time.deltaTime * 2);
    }
    
    private void DecreaseSize()
    {
        if (!needUpdate) return;
        if (rt.localScale.x < scaleMin)
        {
            rt.localScale = Vector3.one * scaleMin;
            needUpdate = false;
            return;
        }
        
        rt.localScale -= Vector3.one * (Time.deltaTime * 2);
    }
    
    public void SetHover(bool b)
    {
        isHovering = b;
        needUpdate = true;
    }
}
