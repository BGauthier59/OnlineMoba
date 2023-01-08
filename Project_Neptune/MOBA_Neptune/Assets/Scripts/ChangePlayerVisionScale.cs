using Entities;
using Photon.Pun;
using UnityEngine;

public class ChangePlayerVisionScale : MonoBehaviourPun
{
    public bool isTopEntry;
    public float newViewRange;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Entity>() == null) return;

        var newChampCollide = other.GetComponent<Entity>();
        
        if (isTopEntry)
        {
            if (other.transform.position.z > transform.position.z) // Entre dans la jungle par le haut 
            {
                Debug.Log("Entered Jungle");
                newChampCollide.photonView.RPC("SetViewRangeRPC", RpcTarget.MasterClient, newChampCollide.entityIndex, 3.5f);
            }
            else // Sort de la jungle
            {
                Debug.Log("Exit Jungle");
                newChampCollide.photonView.RPC("SetViewRangeRPC", RpcTarget.MasterClient, newChampCollide.entityIndex, 1);
            }
        }
        else
        {
            if (other.transform.position.z < transform.position.z) // Trigger collide par le haut 
            {
                Debug.Log("Entered Jungle");
                newChampCollide.photonView.RPC("SetViewRangeRPC", RpcTarget.MasterClient, newChampCollide.entityIndex, 3.5f);
            }
            else // Sort de la jungle
            {
                Debug.Log("Exit Jungle");
                newChampCollide.photonView.RPC("SetViewRangeRPC", RpcTarget.MasterClient, newChampCollide.entityIndex, 1);
            }
        }
    }
}
