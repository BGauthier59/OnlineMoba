using System.Collections;
using System.Collections.Generic;
using Entities;
using Photon.Pun;
using UnityEngine;

public class MarkedCapacity : NewPassiveCapacity
{
    private ushort markCount;
    
    // Se lance sur le Master
    public override void OnAddEffect(Entity giver = null, Vector3 position = default)
    {
        markCount++;
        photonView.RPC("GetMarkedFeedbackRPC", RpcTarget.All);
        base.OnAddEffect(giver, position);

        if (markCount >= 3)
        {
            GetBlownUp();
        }
    }

    private void GetBlownUp()
    {
        markCount = 0;
        
        // Explosion
        Debug.Log("Get blown up");
        
        
        
        OnRemoveEffect();
    }

    [PunRPC]
    public void GetMarkedFeedbackRPC()
    {
        // Feedbacks
    }
    
    public override void OnUpdateEffect()
    {
        
    }
}
