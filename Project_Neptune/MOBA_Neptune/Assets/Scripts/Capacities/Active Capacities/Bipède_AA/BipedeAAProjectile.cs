using Capacities.Active_Capacities;
using Entities;
using JetBrains.Annotations;
using Photon.Pun;
using UnityEngine;

public class BipedeAAProjectile : MonoBehaviourPun
{
    public Entity caster;
    
    [SerializeField] private BipedeAACapacitySO bipedeAASO;
    [SerializeField] private float shootForce;
    [SerializeField] private float lifeDuration;
    private float timerDuration;
    private bool isLaunching;

    public void SendBipedeAA(Entity caster)
    {
        photonView.RPC("SyncLaunchAARPC", RpcTarget.All, caster.entityIndex);
    }

    [PunRPC] [UsedImplicitly]
    public void SyncLaunchAARPC(int casterIndex)
    {
        caster = EntityCollectionManager.GetEntityByIndex(casterIndex);
        LauchAA();
    }
    
    private void LauchAA()
    {
        isLaunching = true;
    }

    private void FixedUpdate()
    {
        if (!isLaunching) return;
        
        transform.position += transform.forward * (shootForce * Time.fixedDeltaTime);

        timerDuration += Time.deltaTime;
        if (timerDuration > lifeDuration)
        {
            gameObject.SetActive(false);
        }
    }  
    
    private void OnTriggerEnter(Collider other)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        var damageable = other.gameObject.GetComponent<IDamageable>();
        if (damageable == null) return;

        if (other.gameObject.GetComponent<Entity>().team != caster.team)
        {
            Debug.Log($"Entity hit : {other.gameObject.name}");

            if (other.gameObject != null)
            {
                damageable?.RequestDecreaseCurrentHp(bipedeAASO.capacityDamages, caster);
            }
            
        }
        
        gameObject.SetActive(false);
    }
}