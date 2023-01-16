using System.Collections;
using Entities;
using Entities.Champion;
using Entities.Minion.MinionJungle;
using UnityEngine;
using Photon.Pun;

public class MinionJungleSecondaryCapacity : NewActiveCapacity
{
    [Space] [Header("Capacity Stats")] [SerializeField]
    private float capacityRange;

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
        photonView.RPC("CastSecondaryMinionJungleCapacityRPC", RpcTarget.MasterClient, targetedPositions);
    }

    private Collider[] colliders;

    [PunRPC]
    public void CastSecondaryMinionJungleCapacityRPC(Vector3[] targetedPositions)
    {
        photonView.RPC("SyncCastSecondaryMinionJungleCapacityRPC", RpcTarget.All);
        caster = GetComponent<Entity>();

        // Calcul de la capa
        var aimedZone = targetedPositions[0]; // Endroit où le joueur se situe
        colliders = Physics.OverlapSphere(aimedZone, capacityRange, capacityLayerMask);

        foreach (var entityStuned in colliders)
        {
            Champion thisChampion = entityStuned.GetComponent<Champion>();

            if (thisChampion && entityStuned.GetComponent<Entity>() != caster)
            {
                Debug.Log("STUN PLAYER");
                thisChampion.rb.velocity = Vector3.zero;
                thisChampion.RequestSetCanMove(false);
                thisChampion.RequestSetCanCast(false);
            }
        }

        StartCoroutine(EndStun());
    }

    private IEnumerator EndStun()
    {
        yield return new WaitForSeconds(capacityDuration);

        foreach (var entityStuned in colliders)
        {
            Champion thisChampion = entityStuned.GetComponent<Champion>();

            if (thisChampion)
            {
                Debug.Log("FINI");
                thisChampion.RequestSetCanMove(true);
                thisChampion.RequestSetCanCast(true);
            }
        }
    }


    [PunRPC]
    public void SyncCastSecondaryMinionJungleCapacityRPC()
    {
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

    public override void RequestSetPreview(bool active) { }

    public override void Update() { }

    public override void UpdatePreview() { }
}