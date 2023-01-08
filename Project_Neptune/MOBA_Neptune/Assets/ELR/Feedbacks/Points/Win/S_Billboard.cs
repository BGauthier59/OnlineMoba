using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Billboard : MonoBehaviour
{
    public Transform textMeshTransform;
    void Update () {
        textMeshTransform.transform.rotation = Camera.main.transform.rotation;
    }
}
