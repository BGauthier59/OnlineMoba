using System.Collections;
using System.Collections.Generic;
using Entities;
using Entities.Champion;
using Photon.Pun;
using UnityEngine;

public class SlowedCapacity : NewPassiveCapacity
{
    [SerializeField] private ParticleSystem slowedFx;
    [SerializeField] private float duration;
    private float timer;

    [SerializeField] private float speedModifier;
    private float initSpeed;

    public override void OnUpdateEffect()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        if (!isActive) return;

        CheckTimer();
    }

    // Se lance sur le Master
    public override void OnAddEffect(Entity giver = null, Vector3 position = default)
    {
        Debug.Log("slowed!");
        var moveable = entityUnderEffect.GetComponent<IMoveable>();
        if (moveable == null) return;

        photonView.RPC("GetSlowedFeedback", RpcTarget.All);

        var championUnderEffect = ((Champion)entityUnderEffect);
        initSpeed = championUnderEffect.currentMoveSpeed;
        moveable.SetCurrentMoveSpeedRPC(championUnderEffect.currentMoveSpeed / speedModifier);

        base.OnAddEffect(giver, position);
    }

    private void CheckTimer()
    {
        Debug.Log(timer);
        if (timer >= duration)
        {
            timer = 0f;
            OnRemoveEffect();
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

    public override void OnRemoveEffect()
    {
        Debug.Log("not slowed any more");
        photonView.RPC("CancelSlowFeedback", RpcTarget.All);
        var championUnderEffect = ((Champion)entityUnderEffect);
        championUnderEffect.SetCurrentMoveSpeedRPC(initSpeed);
        base.OnRemoveEffect();
    }

    [PunRPC]
    public void GetSlowedFeedback()
    {
        // Feedbacks
        slowedFx.Play();
    }

    [PunRPC]
    public void CancelSlowFeedback()
    {
        // Feedbacks
        slowedFx.Stop();
    }
}