using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnqueuePoolLocal : MonoBehaviour
{
    [SerializeField] private float duration;
    [SerializeField]
    private GameObject prefab;
    private void OnEnable()
    {
        StartCoroutine(WaitforEnqueued());
    }

    IEnumerator WaitforEnqueued()
    {
        yield return new WaitForSeconds(duration);
        PoolLocalManager.Instance.EnqueuePool(prefab, this.gameObject);
    }

}
