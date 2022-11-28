using Photon.Pun;

namespace Entities.Champion
{
    public partial class Champion : IResourceable
    {
        public float maxResource;
        public float currentResource;

        public float GetMaxResource()
        {
            return maxResource;
        }

        public float GetCurrentResource()
        {
            return currentResource;
        }

        public float GetCurrentResourcePercent()
        {
            return currentResource / maxResource * 100;
        }

        public void RequestSetMaxResource(float value)
        {
            photonView.RPC("SetMaxResourceRPC", RpcTarget.MasterClient, value);
        }

        [PunRPC]
        public void SyncSetMaxResourceRPC(float value)
        {
            maxHp = value;
            OnSetMaxResourceFeedback?.Invoke(value);
        }

        [PunRPC]
        public void SetMaxResourceRPC(float value)
        {
            maxResource = value; 
            OnSetMaxResource?.Invoke(value);
            photonView.RPC("SyncSetMaxResourceRPC", RpcTarget.All, value);
        }

        public event GlobalDelegates.FloatDelegate OnSetMaxResource;
        public event GlobalDelegates.FloatDelegate OnSetMaxResourceFeedback;

        public void RequestIncreaseMaxResource(float amount)
        {
            photonView.RPC("IncreaseMaxResourceRPC", RpcTarget.MasterClient, amount);
        }

        [PunRPC]
        public void SyncIncreaseMaxResourceRPC(float amount)
        {
            maxResource = amount;
            OnIncreaseMaxResourceFeedback?.Invoke(amount);
        }

        [PunRPC]
        public void IncreaseMaxResourceRPC(float amount)
        {
            maxResource += amount;
            OnIncreaseMaxResource?.Invoke(amount);
            photonView.RPC("SyncIncreaseMaxResourceRPC", RpcTarget.All, amount);
        }

        public event GlobalDelegates.FloatDelegate OnIncreaseMaxResource;
        public event GlobalDelegates.FloatDelegate OnIncreaseMaxResourceFeedback;

        public void RequestDecreaseMaxResource(float amount)
        {
            photonView.RPC("DecreaseMaxResourceRPC", RpcTarget.MasterClient, amount);
        }

        [PunRPC]
        public void SyncDecreaseMaxResourceRPC(float amount)
        {
            maxResource = amount;
            OnDecreaseMaxResourceFeedback?.Invoke(amount);
        }

        [PunRPC]
        public void DecreaseMaxResourceRPC(float amount)
        {
            maxResource -= amount;
            OnDecreaseMaxResource?.Invoke(amount);
            photonView.RPC("SyncDecreaseMaxResourceRPC", RpcTarget.All, amount);
        }

        public event GlobalDelegates.FloatDelegate OnDecreaseMaxResource;
        public event GlobalDelegates.FloatDelegate OnDecreaseMaxResourceFeedback;

        public void RequestSetCurrentResource(float value)
        {
            photonView.RPC("SetCurrentResourceRPC", RpcTarget.MasterClient, value);
        }

        [PunRPC]
        public void SyncSetCurrentResourceRPC(float value)
        {
            currentResource = value;
            OnSetCurrentResourceFeedback?.Invoke(value);
        }

        [PunRPC]
        public void SetCurrentResourceRPC(float value)
        {
            currentResource = value;
            OnSetCurrentResource?.Invoke(value);
            photonView.RPC("SyncSetCurrentResourceRPC", RpcTarget.All, value);
        }

        public event GlobalDelegates.FloatDelegate OnSetCurrentResource;
        public event GlobalDelegates.FloatDelegate OnSetCurrentResourceFeedback;

        public void RequestSetCurrentResourcePercent(float value)
        {
            photonView.RPC("SetCurrentResourcePercentRPC", RpcTarget.MasterClient, value);
        }

        [PunRPC]
        public void SyncSetCurrentResourcePercentRPC(float value)
        {
            currentResource = value;
            OnSetCurrentResourcePercentFeedback?.Invoke(value);
        }

        [PunRPC]
        public void SetCurrentResourcePercentRPC(float value)
        {
            currentResource = (value * 100)/maxResource;
            OnSetCurrentResourcePercent?.Invoke(value);
            photonView.RPC("SyncSetCurrentResourcePercentRPC", RpcTarget.All, value);
        }

        public event GlobalDelegates.FloatDelegate OnSetCurrentResourcePercent;
        public event GlobalDelegates.FloatDelegate OnSetCurrentResourcePercentFeedback;

        public void RequestIncreaseCurrentResource(float amount)
        {
            photonView.RPC("IncreaseCurrentResourceRPC", RpcTarget.MasterClient, amount);
        }

        [PunRPC]
        public void SyncIncreaseCurrentResourceRPC(float amount)
        {
            currentResource = amount;
            OnIncreaseCurrentResourceFeedback?.Invoke(amount);
        }

        [PunRPC]
        public void IncreaseCurrentResourceRPC(float amount)
        {
            currentResource += amount;
            OnIncreaseCurrentResource?.Invoke(amount);
            photonView.RPC("SyncIncreaseCurrentResourceRPC", RpcTarget.All, amount);
        }

        public event GlobalDelegates.FloatDelegate OnIncreaseCurrentResource;
        public event GlobalDelegates.FloatDelegate OnIncreaseCurrentResourceFeedback;

        public void RequestDecreaseCurrentResource(float amount)
        {
            photonView.RPC("DecreaseCurrentResourceRPC", RpcTarget.MasterClient, amount);
        }

        [PunRPC]
        public void SyncDecreaseCurrentResourceRPC(float amount)
        {
            currentResource = amount;
            OnDecreaseCurrentResourceFeedback?.Invoke(amount);
        }

        [PunRPC]
        public void DecreaseCurrentResourceRPC(float amount)
        {
            currentResource -= amount;
            OnDecreaseCurrentResource?.Invoke(amount);
            photonView.RPC("SyncDecreaseCurrentResourceRPC", RpcTarget.All, amount);
        }

        public event GlobalDelegates.FloatDelegate OnDecreaseCurrentResource;
        public event GlobalDelegates.FloatDelegate OnDecreaseCurrentResourceFeedback;
    }
}