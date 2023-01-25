using Photon.Pun;
using UnityEngine;

public class DestroyItem : MonoBehaviourPun
{
    [SerializeField] private PhotonView photonView;

    void Start()
    {
        photonView = PhotonView.Get(gameObject);
    }

    public void RequestDestroyItem()
    {
        photonView.RPC("DestroyItemRPC", RpcTarget.MasterClient);
    }

    [PunRPC]
    private void DestroyItemRPC()
    {
        Destroy(gameObject);
        photonView.RPC("SyncDestroyItemRPC", RpcTarget.All);
    }

    [PunRPC]
    private void SyncDestroyItemRPC()
    {
        if (gameObject) Destroy(gameObject);
    }
}