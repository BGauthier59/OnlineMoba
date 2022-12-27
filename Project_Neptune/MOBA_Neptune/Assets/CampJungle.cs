using System;
using UnityEngine;
using Photon.Pun;

namespace Entities.Minion.MinionJungle
{
    public class CampJungle : MonoBehaviour
    {
        [Space] [Header("Spawner Camp Settings")]
        public Entity minionPrefab;
        public int pointsCarriedAtStartByMinions = 4; // 4 de base
        public float repopCycleTime = 30; // 30 secondes de base
        public bool isInRepop;

        private float repopTimer = 0;
        
        private void Start()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            isInRepop = true;
        }

        private void Update()
        {
            if (!PhotonNetwork.IsMasterClient) return;

            if (isInRepop)
            {
                // Respawn de minion
                repopTimer += Time.deltaTime;
                if (repopTimer >= repopCycleTime)
                {
                    Respawn();
                    repopTimer = 0;
                    isInRepop = false;
                }
            }
        }
        
        private void Respawn()
        {
            Entity minionGO = PoolNetworkManager.Instance.PoolInstantiate(minionPrefab, transform.position,
                Quaternion.identity, transform.root);

            MinionJungle minionStreamScript = minionGO.GetComponent<MinionJungle>();
            minionStreamScript.currentPointCarried = pointsCarriedAtStartByMinions;
            minionStreamScript.myCamp = this;
        }
    }
}

