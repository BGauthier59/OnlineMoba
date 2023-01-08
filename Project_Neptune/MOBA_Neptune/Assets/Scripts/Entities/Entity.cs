using System;
using System.Collections.Generic;
using System.Linq;
using Entities.Capacities;
using Entities.FogOfWar;
using Photon.Pun;
using UnityEngine;

namespace Entities
{
    [RequireComponent(typeof(PhotonView))]
    public abstract partial class Entity : MonoBehaviourPun, ITeamable
    {
        /// <summary>
        /// The viewID of the photonView of the entity.
        /// </summary>
        public int entityIndex;

        [SerializeField] private bool canAddPassiveCapacity = true;
        [SerializeField] private bool canRemovePassiveCapacity = true;
        
        public GrabbedCapacity grabbed;
        public MarkedCapacity marked;
        public SlowedCapacity slowed;

        /// <summary>
        /// The current amount of point currently carried by the entity
        /// </summary>
        public int currentPointCarried = 0;

        /// <summary>
        /// The lastest entity who attacked this entity
        /// </summary>
        public int lastEntityWhoAttackedMeIndex;

        public Transform uiTransform;
        public Vector3 guiOffset = new Vector3(0, 2f, 0);

        public Rigidbody rb;
        public Animator animator;

        private void Start()
        {
            entityIndex = photonView.ViewID;
            EntityCollectionManager.AddEntity(this);
            OnStart();
        }

        /// <summary>
        /// Replaces the Start() method.
        /// </summary>
        protected virtual void OnStart()
        {
            if (meshFilterFoV == null) return;
            if (FogOfWarManager.Instance == null) return;
            FogOfWarManager.Instance.AddFOWViewable(this);
        }

        private void Update()
        {
            OnUpdate();
        }

        private void FixedUpdate()
        {
            OnFixedUpdate();
        }

        /// <summary>
        /// Replaces the Update() method.
        /// </summary>
        protected virtual void OnUpdate()
        {
        }

        protected virtual void OnFixedUpdate()
        {
        }

        #region MasterMethods

        public void SendSyncInstantiate(Vector3 position, Quaternion rotation)
        {
            photonView.RPC("SyncInstantiateRPC", RpcTarget.All, position, rotation);
            OnInstantiated();
        }

        public virtual void OnInstantiated()
        {
        }

        [PunRPC]
        public void SyncInstantiateRPC(Vector3 position, Quaternion rotation)
        {
            transform.position = position;
            transform.rotation = rotation;
            OnInstantiatedFeedback();
        }

        public virtual void OnInstantiatedFeedback()
        {
        }

        [PunRPC]
        private void SyncSetCanAddPassiveCapacityRPC(bool value)
        {
            photonView.RPC("SetCanAddPassiveCapacityRPC", RpcTarget.All, value);
        }


        [PunRPC]
        public void SetCanAddPassiveCapacityRPC(bool value)
        {
            canAddPassiveCapacity = value;
        }

        [PunRPC]
        private void SyncSetCanRemovePassiveCapacityRPC(bool value)
        {
            photonView.RPC("SetCanRemovePassiveCapacityRPC", RpcTarget.All, value);
        }

        [PunRPC]
        private void SetCanRemovePassiveCapacityRPC(bool value)
        {
            canRemovePassiveCapacity = value;
        }

        /*
        [PunRPC]
        public void AddPassiveCapacityRPC(byte index, int giverIndex = default, Vector3 pos = default)
        {
            if (!canAddPassiveCapacity) return;
            photonView.RPC("SyncAddPassiveCapacityRPC", RpcTarget.All, index, giverIndex, pos);
        }

        [PunRPC]
        public void SyncAddPassiveCapacityRPC(byte capacityIndex, int giverIndex, Vector3 pos)
        {
            var capacity = CapacitySOCollectionManager.Instance.CreatePassiveCapacity(capacityIndex, this);
            if (capacity == null)
            {
                Debug.LogWarning($"No capacity found! Index is {capacityIndex}.");
                return;
            }

            Entity giver = null;
            if (giverIndex != default)
            {
                Debug.Log($"Index is not null. Finding an entity that matches with index {giverIndex}.");
                giver = EntityCollectionManager.GetEntityByIndex(giverIndex);
                Debug.Log(giver.name);
            }

            if (!passiveCapacitiesList.Contains(capacity)) passiveCapacitiesList.Add(capacity);

            if (PhotonNetwork.IsMasterClient)
            {
                capacity.OnAdded(this, giver, pos);
                OnPassiveCapacityAdded?.Invoke(capacityIndex);
            }

            capacity.OnAddedFeedback(this, giver, pos);
            OnPassiveCapacityAddedFeedback?.Invoke(capacityIndex);
        }
        */

        public event GlobalDelegates.ByteDelegate OnPassiveCapacityAdded;
        public event GlobalDelegates.ByteDelegate OnPassiveCapacityAddedFeedback;

        /*
        public void RemovePassiveCapacityByIndex(byte index)
        {
            photonView.RPC("SyncRemovePassiveCapacityRPC", RpcTarget.All, index);
        }
        
    
        [PunRPC]
        public void SyncRemovePassiveCapacityRPC(byte index)
        {
            if (index >= passiveCapacitiesList.Count) return;
            var capacity = passiveCapacitiesList[index];
            passiveCapacitiesList.Remove(capacity);
            if (PhotonNetwork.IsMasterClient)
            {
                capacity.OnRemoved(this);
                OnPassiveCapacityRemoved?.Invoke(index);
            }

            capacity.OnRemovedFeedback(this);
            OnPassiveCapacityRemovedFeedback?.Invoke(index);
        }
        */

        public event GlobalDelegates.ByteDelegate OnPassiveCapacityRemoved;
        public event GlobalDelegates.ByteDelegate OnPassiveCapacityRemovedFeedback;

        #endregion
    }
}