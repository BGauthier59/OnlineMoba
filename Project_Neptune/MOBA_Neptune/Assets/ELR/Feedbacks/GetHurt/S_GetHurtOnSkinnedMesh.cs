using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_GetHurtOnSkinnedMesh : MonoBehaviour
{
    public SkinnedMeshRenderer Renderer;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayFeedback();
        }
        
        //Debug.Log (Time.time + " ||| " + Renderer.material.GetFloat("_HitTime") + " ||| " + (Time.time - Renderer.material.GetFloat("_HitTime")< Renderer.material.GetFloat("_Duration")));
    }

    public void PlayFeedback()
    {
        Renderer.material.SetFloat("_HitTime", Time.time);
    }
}
