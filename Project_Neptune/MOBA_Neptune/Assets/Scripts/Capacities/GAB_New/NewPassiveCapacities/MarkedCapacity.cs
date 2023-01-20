using System.Collections;
using System.Collections.Generic;
using Entities;
using Entities.Champion;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class MarkedCapacity : NewPassiveCapacity
{
    [SerializeField] private ParticleSystem iceExplosionFx;
    private int markCount;
    [SerializeField] private uint damage;
    [SerializeField] private TextMeshPro markDebugText;
    
    // Se lance sur le Master
    public override void OnAddEffect(Entity giver = null, Vector3 position = default)
    {
        markCount++;
        photonView.RPC("SyncMarkCountFeedback", RpcTarget.All, markCount);
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
        photonView.RPC("SyncMarkCountFeedback", RpcTarget.All, markCount);

        // Explosion
        Debug.Log("Get blown up");
        
        var damageable = entityUnderEffect.GetComponent<IDamageable>();
        damageable?.DecreaseCurrentHpRPC(damage, default);
        
        entityUnderEffect.slowed.OnAddEffect();
        OnRemoveEffect();
    }

    [PunRPC]
    public void GetMarkedFeedbackRPC()
    {
        // Feedbacks
        iceExplosionFx.Play();
    }

    [PunRPC]
    private void SyncMarkCountFeedback(int count)
    {
        markDebugText.text = $"Marks: {count} / 3";
    }
    
    public override void OnUpdateEffect()
    {
        
    }
}
