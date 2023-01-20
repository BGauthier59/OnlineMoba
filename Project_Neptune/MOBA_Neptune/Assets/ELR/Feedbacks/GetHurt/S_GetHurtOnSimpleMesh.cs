using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_GetHurtOnSimpleMesh : MonoBehaviour
{
    public MeshRenderer Renderer;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayFeedback();
        }
    }

    public void PlayFeedback()
    {
        Renderer.material.SetFloat("_HitTime", Time.deltaTime * 50);
    }
}
