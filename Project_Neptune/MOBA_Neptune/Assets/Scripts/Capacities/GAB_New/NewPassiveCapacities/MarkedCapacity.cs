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
    private ushort markCount;
    [SerializeField] private uint damage;
    [SerializeField] private TextMeshPro markDebugText;
    
    // Se lance sur le Master
    public override void OnAddEffect(Entity giver = null, Vector3 position = default)
    {
        markCount++;
        markDebugText.text = $"Marks: {markCount}";
        Debug.Log("got marked");
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
        markDebugText.text = $"Marks: {markCount}";

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
    
    public override void OnUpdateEffect()
    {
        
    }
}
