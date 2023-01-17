using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurto : MonoBehaviour
{
    public SpriteRenderer Renderer;
  

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayFeedback();
            
            
        }

    
        //Debug.Log (Time.time + " ||| " + Renderer.material.GetFloat("_HitTime") + " ||| " + (Time.time - Renderer.material.GetFloat("_HitTime")< Renderer.material.GetFloat("_Duration")));
    }

    void PlayFeedback()
    {
        Renderer.material.SetFloat("_HitTime", Time.time);
   
      

    }


    
}
