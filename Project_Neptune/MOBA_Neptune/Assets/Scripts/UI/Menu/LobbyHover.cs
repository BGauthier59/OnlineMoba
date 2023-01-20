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
        if (rt.localScale.x > scaleMax)
        {
            rt.localScale = Vector3.one * scaleMax;
            return;
        }
        
        rt.localScale += Vector3.one * (Time.deltaTime * 1);
    }
    
    private void DecreaseSize()
    {
        if (rt.localScale.x < scaleMin)
        {
            rt.localScale = Vector3.one * scaleMin;
            return;
        }
        
        rt.localScale -= Vector3.one * (Time.deltaTime * 1);
    }
    
    public void SetHover(bool b)
    {
        isHovering = b;
    }
}
