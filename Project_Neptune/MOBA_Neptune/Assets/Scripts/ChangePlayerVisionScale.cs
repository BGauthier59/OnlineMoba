using Entities.Champion;
using Photon.Pun;
using UnityEngine;

public class ChangePlayerVisionScale : MonoBehaviourPun
{
    public bool isTopEntry;
    public float newViewRange;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Champion>() == null) return;

        var newChampCollide = other.GetComponent<Champion>();
        
        if (isTopEntry)
        {
            if (other.transform.position.z < transform.position.z) // Entre dans la jungle par le haut 
            {
                newChampCollide.photonView.RPC("SetViewRangeRPC", RpcTarget.MasterClient, newChampCollide.entityIndex, newChampCollide.baseViewRange);
            }
            else // Sort de la jungle
            {
                newChampCollide.photonView.RPC("SetViewRangeRPC", RpcTarget.MasterClient, newChampCollide.entityIndex, newViewRange);
            }
        }
        else
        {
            if (other.transform.position.z > transform.position.z) // Trigger collide par le haut 
            {
                newChampCollide.photonView.RPC("SetViewRangeRPC", RpcTarget.MasterClient, newChampCollide.entityIndex, newChampCollide.baseViewRange);
            }
            else // Sort de la jungle
            {
                newChampCollide.photonView.RPC("SetViewRangeRPC", RpcTarget.MasterClient, newChampCollide.entityIndex, newViewRange);
            }
        }
    }
}
