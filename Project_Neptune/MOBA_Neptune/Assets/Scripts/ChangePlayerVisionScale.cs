using System;
using System.Collections;
using System.Collections.Generic;
using Entities.Champion;
using UnityEngine;

public class ChangePlayerVisionScale : MonoBehaviour
{
    public bool isTopEntry;
    public int newViewRange;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Champion>() == null) return;

        var newChampCollide = other.GetComponent<Champion>();
        
        if (isTopEntry)
        {
            if (other.transform.position.z > transform.position.z) // Entre dans la jungle par le haut 
            {
                newChampCollide.RequestSetViewRange(newViewRange);
            }
            else // Sort de la jungle
            {
                newChampCollide.RequestSetViewRange(newChampCollide.baseViewRange);
            }
        }
        else
        {
            if (other.transform.position.z < transform.position.z) // Trigger collide par le haut 
            {
                newChampCollide.RequestSetViewRange(newViewRange);
            }
            else // Sort de la jungle par le bas 
            {
                newChampCollide.RequestSetViewRange(newChampCollide.baseViewRange);
            }
        }
    }
}
