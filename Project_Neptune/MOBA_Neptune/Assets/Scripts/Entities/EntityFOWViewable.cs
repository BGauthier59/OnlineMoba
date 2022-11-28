using System;
using System.Collections.Generic;
using Entities.FogOfWar;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Rendering;

namespace Entities
{
    public abstract partial class Entity : IFOWViewable
    {
        public bool canChangeTeam;
        public Enums.Team team;

        [Header("Field Of View Settings")] public float baseViewRange;
        public float viewRange;
        [Range(0, 360)] public float viewAngle;
        public bool canView;
        public List<IFOWShowable> seenShowables = new List<IFOWShowable>();
        public MeshFilter meshFilterFoV;
       public Transform fogOfWarStartDetection;
        
        public Enums.Team GetTeam()
        {
            return team;
        }

        public List<Enums.Team> GetEnemyTeams()
        {
            return Enum.GetValues(typeof(Enums.Team)).Cast<Enums.Team>().Where(someTeam => someTeam != team)
                .ToList(); //returns all teams that are not 'team'
        }

        public bool CanChangeTeam()
        {
            return canChangeTeam;
        }

        public void RequestChangeTeam(Enums.Team team)
        {
            photonView.RPC("ChangeTeamRPC", RpcTarget.MasterClient, (byte)team);
        }

        [PunRPC]
        public void SyncChangeTeamRPC(byte team)
        {
            this.team = (Enums.Team)team;
        }

        [PunRPC]
        public void ChangeTeamRPC(byte team)
        {
            photonView.RPC("SyncChangeTeamRPC", RpcTarget.All, team);
        }

        public event GlobalDelegates.BoolDelegate OnChangeTeam;
        public event GlobalDelegates.BoolDelegate OnChangeTeamFeedback;

        public bool CanView() => canView;
        public float GetFOWViewRange() => viewRange;
        public float GetFOWBaseViewRange() => baseViewRange;

        public List<IFOWShowable> SeenShowables() => seenShowables;

        public void RequestSetCanView(bool value)
        {
            photonView.RPC("SetCanViewRPC", RpcTarget.MasterClient, value);
        }

        [PunRPC]
        public void SyncSetCanViewRPC(bool value)
        {
            canView = value;
            OnSetCanViewFeedback?.Invoke(value);
        }

        [PunRPC]
        public void SetCanViewRPC(bool value)
        {
            canView = value;
            OnSetCanView?.Invoke(value);
            photonView.RPC("SetCanViewRPC", RpcTarget.All, value);
        }

        public event GlobalDelegates.BoolDelegate OnSetCanView;
        public event GlobalDelegates.BoolDelegate OnSetCanViewFeedback;

        public void RequestSetViewRange(float value)
        {
            photonView.RPC("SyncSetViewRangeRPC", RpcTarget.MasterClient, value);
        }

        [PunRPC]
        public void SyncSetViewRangeRPC(float value)
        {
            viewRange = value;
            OnSetViewRangeFeedback?.Invoke(value);
        }

        [PunRPC]
        public void SetViewRangeRPC(float value)
        {
            viewRange = value;
            OnSetViewRange?.Invoke(value);
            photonView.RPC("SyncSetViewRangeRPC", RpcTarget.All, value);
        }

        public event GlobalDelegates.FloatDelegate OnSetViewRange;
        public event GlobalDelegates.FloatDelegate OnSetViewRangeFeedback;

        public void RequestSetViewAngle(float value)
        {
            photonView.RPC("SyncSetViewAngleRPC", RpcTarget.MasterClient, value);
        }

        public void SyncSetViewAngleRPC(float value)
        {
            viewAngle = value;
            OnSetViewAngleFeedback?.Invoke(value);
        }

        public void SetViewAngleRPC(float value)
        {
            viewAngle = value;
            OnSetViewAngle?.Invoke(value);
            photonView.RPC("SyncSetViewAngleRPC", RpcTarget.All, value);
        }

        public event GlobalDelegates.FloatDelegate OnSetViewAngle;
        public event GlobalDelegates.FloatDelegate OnSetViewAngleFeedback;

        public void RequestSetBaseViewRange(float value)
        {
            photonView.RPC("SetBaseViewRangeRPC", RpcTarget.MasterClient, value);
        }

        [PunRPC]
        public void SyncSetBaseViewRangeRPC(float value)
        {
            baseViewRange = value;
            OnSetBaseViewRangeFeedback?.Invoke(value);
        }

        [PunRPC]
        public void SetBaseViewRangeRPC(float value)
        {
            baseViewRange = value;
            OnSetBaseViewRange?.Invoke(value);
            photonView.RPC("SyncSetBaseViewRangeRPC", RpcTarget.All, value);
        }

        public event GlobalDelegates.FloatDelegate OnSetBaseViewRange;
        public event GlobalDelegates.FloatDelegate OnSetBaseViewRangeFeedback;

        public void AddShowable(int seenEntityIndex)
        {
            var entity = EntityCollectionManager.GetEntityByIndex(seenEntityIndex);
            if (entity == null) return;

            var showable = entity.GetComponent<IFOWShowable>();
            if (showable == null) return;

            AddShowable(showable);
        }

        public void AddShowable(IFOWShowable showable)
        {
            if (seenShowables.Contains(showable)) return;

            seenShowables.Add(showable);
            //Debug.Log("seen Showable Add");
            showable.TryAddFOWViewable(this);
            //Debug.Log("Try add This FowViewable");
            var seenEntityIndex = ((Entity)showable).entityIndex;
            //Debug.Log("Entity index : " + seenEntityIndex);
            OnAddShowableFeedback?.Invoke(seenEntityIndex);

            
         //   if (!PhotonNetwork.IsMasterClient) return;
           // OnAddShowable?.Invoke(seenEntityIndex);
            //photonView.RPC("SyncAddShowableRPC", RpcTarget.All, seenEntityIndex);
        }

        [PunRPC]
        public void SyncAddShowableRPC(int seenEntityIndex)
        {
            var entity = EntityCollectionManager.GetEntityByIndex(seenEntityIndex);
            if (entity == null) return;

            var showable = entity.GetComponent<IFOWShowable>();
            if (showable == null) return;
            if (seenShowables.Contains(showable)) return;

            seenShowables.Add(showable);
            OnAddShowableFeedback?.Invoke(seenEntityIndex);
         //    if (!PhotonNetwork.IsMasterClient) showable.TryAddFOWViewable(this);
        }

        public event GlobalDelegates.IntDelegate OnAddShowable;
        public event GlobalDelegates.IntDelegate OnAddShowableFeedback;

        public void RemoveShowable(int seenEntityIndex)
        {
            var entity = EntityCollectionManager.GetEntityByIndex(seenEntityIndex);
            if (entity == null) return;

            var showable = entity.GetComponent<IFOWShowable>();
            if (showable == null) return;

            RemoveShowable(showable);
        }

        public void RemoveShowable(IFOWShowable showable)
        {
            if (!seenShowables.Contains(showable)) return;

            seenShowables.Remove(showable);
            //Debug.Log("Remove Showable");
            showable.TryRemoveFOWViewable(this);
            //Debug.Log("TryRemoveFOWViewable");
            
            var seenEntityIndex = ((Entity)showable).entityIndex;
            //Debug.Log("Entity Index : " + ((Entity)showable).entityIndex);
            OnRemoveShowableFeedback?.Invoke(seenEntityIndex);

            //    if (!PhotonNetwork.IsMasterClient) return;
            //     OnRemoveShowable?.Invoke(seenEntityIndex);
            //     photonView.RPC("SyncRemoveShowableRPC", RpcTarget.All, seenEntityIndex);
        }

        [PunRPC]
        public void SyncRemoveShowableRPC(int seenEntityIndex)
        {
            var entity = EntityCollectionManager.GetEntityByIndex(seenEntityIndex);
            if (entity == null) return;

            var showable = entity.GetComponent<IFOWShowable>();
            if (showable == null) return;
            if (!seenShowables.Contains(showable)) return;

            seenShowables.Remove(showable);
            OnAddShowableFeedback?.Invoke(seenEntityIndex);
            //     if (!PhotonNetwork.IsMasterClient) showable.TryRemoveFOWViewable(this);
        }

        public event GlobalDelegates.IntDelegate OnRemoveShowable;
        public event GlobalDelegates.IntDelegate OnRemoveShowableFeedback;

  
    }
}