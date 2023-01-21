using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_GetHurtOnSimpleMesh : MonoBehaviour
{
    public MeshRenderer Renderer;


    private void Update()
    {
        
    }

    public void PlayFeedback()
    {
        Renderer.material.SetFloat("_HitTime", Time.time);
    }
}
