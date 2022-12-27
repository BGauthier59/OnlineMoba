using Entities.Minion;
using Entities.Minion.MinionStream;
using Photon.Pun;
using UnityEngine;
using MinionStreamBehaviour = Entities.Minion.MinionStream.MinionStreamBehaviour;

public class TriggerToCashier : MonoBehaviour
{
    public Enums.Team teamToGoCashier;
    public Transform cashierPos;

    private void OnTriggerEnter(Collider other)
    {
        if(!PhotonNetwork.IsMasterClient) return;
        
        MinionStreamBehaviour m_behavior = other.gameObject.GetComponent<MinionStreamBehaviour>();

        if (!m_behavior) return;

        if (m_behavior.team == teamToGoCashier)
            MinionReorientation(m_behavior);
    }

    void MinionReorientation(MinionStreamBehaviour mB)
    {
        mB.myStreamController.currentState = MinionStreamController.MinionState.LookingForPathing;
        mB.myWayPoint = cashierPos;
    }
}
