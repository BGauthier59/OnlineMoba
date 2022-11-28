using System;
using System.Collections;
using System.Collections.Generic;
using Entities;
using Photon.Pun;
using UnityEngine;

public class PoolNetworkManager : MonoBehaviour
{
    [Serializable]
    public class ElementData
    {
        public Entity Element;
        public uint amount;
    }

    private bool isSetup;

    public static PoolNetworkManager Instance;

    [SerializeField] private List<ElementData> poolElements;
    
    public static Dictionary<Entity, Queue<Entity>> queuesDictionary;

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }
        Instance = this;
        SetupDictionary();
    }

    private void SetupDictionary()
    {
        queuesDictionary = new Dictionary<Entity, Queue<Entity>>();
        foreach (var elementData in poolElements)
        {
            Queue<Entity> newQueue = new Queue<Entity>();
            for (int i = 0; i < elementData.amount; i++)
            {
                Entity entity = PhotonNetwork.Instantiate(elementData.Element.gameObject.name, transform.position, Quaternion.identity).GetComponent<Entity>();
                entity.gameObject.SetActive(false);
                newQueue.Enqueue(entity);
            }
            queuesDictionary.Add(elementData.Element, newQueue);
        }
    }

    public Entity PoolInstantiate(byte index, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        if (index >= poolElements.Count) return null;
        var entityRef = poolElements[index].Element;
        return PoolInstantiate(entityRef, position, rotation, parent);
    }

    public Entity PoolInstantiate(Entity entityRef, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        if(!isSetup)SetupDictionary();
        //Debug.Log(entityRef);
        Entity entity;
        if (parent == null) parent = transform;
        if (queuesDictionary.ContainsKey(entityRef))
        {
            var queue = queuesDictionary[entityRef];
            if (queue.Count == 0)
            {
                entity = PhotonNetwork.Instantiate(entityRef.gameObject.name, position, rotation).GetComponent<Entity>();
                entity.OnInstantiated();
                entity.OnInstantiatedFeedback();
            }
            else
            {
                entity = queue.Dequeue();
                entity.OnInstantiated();
                entity.SendSyncInstantiate(position,rotation);
            }
        }
        else
        {
            //Debug.Log("New pool of " + entityRef.gameObject.name);
            queuesDictionary.Add(entityRef, new Queue<Entity>());
            
            entity = PhotonNetwork.Instantiate(entityRef.gameObject.name, position, rotation).GetComponent<Entity>();
            //entity.OnInstantiated();
            //entity.OnInstantiatedFeedback();
            
        }

        return entity;
    }

    public void PoolRequeue(Entity entity)
    {
        
    }
}
