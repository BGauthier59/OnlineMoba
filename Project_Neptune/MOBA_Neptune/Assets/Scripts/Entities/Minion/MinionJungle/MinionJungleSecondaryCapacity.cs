using System.Collections;
using Entities;
using Entities.Champion;
using Entities.Minion.MinionJungle;
using UnityEngine;
using Photon.Pun;

public class MinionJungleSecondaryCapacity : NewActiveCapacity
{
    [Space] [Header("Capacity Stats")]
    [SerializeField] private float capacityRange;
    [SerializeField] private float capacityDuration;
    [SerializeField] private LayerMask capacityLayerMask;
    [SerializeField] private Vector3 casterPos;
    [SerializeField] private Vector3 targetPos;
    
    
    // Start is called before the first frame update
    void Start()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        capacityRange = GetComponent<MinionJungle>().stunRange;
        capacityDuration = GetComponent<MinionJungle>().stunDuration;
    }
    
    public override void RequestCastCapacity(int[] targetedEntities, Vector3[] targetedPositions)
    {
        photonView.RPC("CastSecondaryMinionJungleCapacityRPC", RpcTarget.MasterClient);
    }
    
    [PunRPC]
    public IEnumerator CastSecondaryMinionJungleCapacityRPC(int[] targetedEntities, Vector3[] targetedPositions)
    {
        photonView.RPC("SyncCastSecondaryMinionJungleCapacityRPC", RpcTarget.All);
        
        // Calcul de la capa
        var aimedZone = targetedPositions[0]; // Endroit où le joueur se situe
        var colliders = Physics.OverlapSphere(aimedZone, capacityRange, capacityLayerMask);
        
        foreach (var VARIABLE in colliders)
        {
            if (VARIABLE.GetComponent<Champion>())
            {
                VARIABLE.GetComponent<Champion>().RequestSetCanMove(false);
                VARIABLE.GetComponent<Champion>().RequestSetCanAttack(false);
                VARIABLE.GetComponent<Champion>().RequestSetCanCast(false);
            }
        }

        yield return new WaitForSeconds(capacityDuration);
        
        foreach (var VARIABLE in colliders)
        {
            if (VARIABLE.GetComponent<Champion>())
            {
                VARIABLE.GetComponent<Champion>().RequestSetCanMove(true);
                VARIABLE.GetComponent<Champion>().RequestSetCanAttack(true);
                VARIABLE.GetComponent<Champion>().RequestSetCanCast(true);
            }
        }
    }

    [PunRPC]
    public void SyncCastSecondaryMinionJungleCapacityRPC()
    {
        caster = GetComponent<Entity>();
        // TODO - Afficher FX
    }
    
    
    public override bool TryCast()
    {
        return true;
    }
    
    protected override void StartCooldown()
    {
        throw new System.NotImplementedException();
    }

    protected override void TimerCooldown()
    {
        throw new System.NotImplementedException();
    }
}
