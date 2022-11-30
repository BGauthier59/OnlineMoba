using System.Collections;
using Entities;
using Photon.Pun;
using UnityEngine;

namespace Entities.Minion
{
    public class MinionSpawner : Building
    {
        [Space]
        [Header("Spawner Seetings")]
        public Transform spawnPointForMinion;
        public Entity minionPrefab;
        public int spawnMinionAmount = 5;
        public float spawnMinionInterval = 1.7f;
        public float spawnCycleTime = 30;
        private readonly float spawnSpeed = 30;
        public Color minionColor;
        public Transform goToPointBeforeStream;
        public string unitTag;

        private void Update()
        {
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

            MinionBehaviour minionScript = minionGO.GetComponent<MinionBehaviour>();
            minionScript.myWayPoint = goToPointBeforeStream;
            minionScript.tag = unitTag;
            minionGO.GetComponent<MeshRenderer>().material.color = minionColor;
        }
    }
}