using System.Collections;
using Entities;
using GameStates;
using Photon.Pun;
using UnityEngine;

namespace Entities.Minion.MinionStream
{
    public class MinionSpawner : Building
    {
        [Space] [Header("Spawner Settings")] public Transform spawnPointForMinion;
        public Entity minionPrefab;
        public int spawnMinionAmount = 5;
        public float spawnMinionInterval = 1.7f;
        public float spawnCycleTime = 30;
        private readonly float spawnSpeed = 30;
        public Transform goToPointBeforeStream;
        public int pointsCarriedAtStartByMinions = 2;
        public Material teamMat;

        private void Update()
        {
            if (!PhotonNetwork.IsMasterClient) return;

            // Spawn de minion tous les spawnCycleTime secondes
            spawnCycleTime += Time.deltaTime;
            if (spawnCycleTime >= spawnSpeed)
            {
                StartCoroutine(SpawnMinionCo());
                spawnCycleTime = 0;
            }
        }

        private IEnumerator SpawnMinionCo()
        {
            for (int i = 0; i < spawnMinionAmount; i++)
            {
                yield return new WaitForSeconds(spawnMinionInterval);
                SpawnMinion();
            }
        }

        private void SpawnMinion()
        {
            Entity minionGO = PoolNetworkManager.Instance.PoolInstantiate(minionPrefab, spawnPointForMinion.position,
                Quaternion.identity, transform.root);

            MinionStream.MinionStreamBehaviour minionStreamScript = minionGO.GetComponent<MinionStream.MinionStreamBehaviour>();
            minionStreamScript.myWayPoint = goToPointBeforeStream;
            minionStreamScript.ChangeTeamRPC((byte)team);
            minionStreamScript.currentPointCarried = pointsCarriedAtStartByMinions;
            //minionScript.myMeshRenderer.materials[1] = teamMat;
        }
    }
}