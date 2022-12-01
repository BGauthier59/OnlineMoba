using Entities.Minion;
using UnityEngine;

public class TriggerToCashier : MonoBehaviour
{
    public Enums.Team teamToGoCashier;
    public Transform cashierPos;

    private void OnTriggerEnter(Collider other)
    {
        MinionBehaviour m_behavior = other.gameObject.GetComponent<MinionBehaviour>();

        if (!m_behavior) return;

        if (m_behavior.team == teamToGoCashier)
            MinionReorientation(m_behavior);
    }

    void MinionReorientation(MinionBehaviour mB)
    {
        mB.myController.currentState = MinionController.MinionState.LookingForPathing;
        mB.myWayPoint = cashierPos;
    }
}
