using System;
using Controllers;
using Entities.Capacities;
using Entities.FogOfWar;
using GameStates;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;

namespace Entities.Champion
{
    public partial class Champion : Entity
    {
        public ChampionSO championSo;
        public Transform rotateParent;
        public Transform championMesh;
        private Vector3 respawnPos;

        private FogOfWarManager fowm;
        private CapacitySOCollectionManager capacityCollection;
        private UIManager uiManager;
        public Camera camera;
        public Rigidbody rb;

        public CollisionBlocker blocker;
        protected override void OnStart()
        {
            base.OnStart();
            fowm = FogOfWarManager.Instance;
            capacityCollection = CapacitySOCollectionManager.Instance;
            uiManager = UIManager.Instance;
            camera = Camera.main;
            uiManager = UIManager.Instance;
            agent = GetComponent<NavMeshAgent>();
            NavMeshObstacle obstacle = GetComponent<NavMeshObstacle>();
            if (isBattlerite)
            {
                obstacle.enabled = false;
                blocker.characterColliderBlocker.enabled = true;
                blocker.SetUpBlocker();
                agent.enabled = false;
            }
            else
            {
                blocker.characterColliderBlocker.enabled = false;
                obstacle.enabled = true;
                rb.isKinematic = true;
                agent.enabled = true;
            }
          
        }

        protected override void OnUpdate()
        {
            if(isBattlerite && GameStateMachine.Instance.GetPlayerChampion() == this)
            RotateMath();
            else
            {
            if (isFollowing) FollowEntity(); // Lol
            if (!photonView.IsMine) return;
            CheckMoveDistance();
            }
        }

        protected override void OnFixedUpdate()
        {
            if (!isBattlerite || GameStateMachine.Instance.GetPlayerChampion() != this) return;
            Move();
            Rotate();
        }

        public override void OnInstantiated()
        {
            base.OnInstantiated();
        }

        public override void OnInstantiatedFeedback()
        {
        }

        public void ApplyChampionSO(byte championSoIndex, Enums.Team newTeam)
        {
            var so = GameStateMachine.Instance.allChampionsSo[championSoIndex];
            championSo = so;
            maxHp = championSo.maxHp;
            currentHp = maxHp;
            maxResource = championSo.maxRessource;
            currentResource = championSo.maxRessource;
            viewRange = championSo.viewRange;
            referenceMoveSpeed = championSo.referenceMoveSpeed;
            currentMoveSpeed = referenceMoveSpeed;
            attackDamage = championSo.attackDamage;
            attackAbilityIndex = championSo.attackAbilityIndex;
            abilitiesIndexes = championSo.activeCapacitiesIndexes;
            ultimateAbilityIndex = championSo.ultimateAbilityIndex;
            var championMesh = Instantiate(championSo.championMeshPrefab, rotateParent.position,
                Quaternion.identity, rotateParent);
            championMesh.transform.localEulerAngles = Vector3.zero;

            team = newTeam;

            Transform pos = transform;
            switch (team)
            {
                case Enums.Team.Team1:
                {
                    for (int i = 0; i < MapLoaderManager.Instance.firstTeamBasePoint.Length; i++)
                    {
                        if (MapLoaderManager.Instance.firstTeamBasePoint[i].champion == null)
                        {
                            pos = MapLoaderManager.Instance.firstTeamBasePoint[i].position;
                            MapLoaderManager.Instance.firstTeamBasePoint[i].champion = this;
                            break;
                        }
                    }

                    break;
                }
                case Enums.Team.Team2:
                {
                    for (int i = 0; i < MapLoaderManager.Instance.secondTeamBasePoint.Length; i++)
                    {
                        if (MapLoaderManager.Instance.secondTeamBasePoint[i].champion == null)
                        {
                            pos = MapLoaderManager.Instance.secondTeamBasePoint[i].position;
                            MapLoaderManager.Instance.secondTeamBasePoint[i].champion = this;
                            break;
                        }
                    }

                    break;
                }
                default:
                    Debug.LogError("Team is not valid.");
                    pos = transform;
                    break;
            }

            if (GameStates.GameStateMachine.Instance.GetPlayerTeam() != team)
            {
                championMesh.SetActive(false);
            }

            respawnPos = transform.position = pos.position;
            if (!isBattlerite)
            {
                SetupNavMesh();
            }
            championMesh.GetComponent<ChampionMeshLinker>().LinkTeamColor(this.team);
            elementsToShow.Add(championMesh);

            uiManager = UIManager.Instance;

            if (uiManager != null)
            {
                uiManager.InstantiateHealthBarForEntity(entityIndex);
                uiManager.InstantiateResourceBarForEntity(entityIndex);
            }

            so.SetIndexes();
            for (int i = 0; i < so.passiveCapacitiesIndexes.Length; i++)
            {
                AddPassiveCapacityRPC(so.passiveCapacitiesIndexes[i]);
            }
            rb.velocity = Vector3.zero;
            RequestSetCanDie(true);
        }
    }
}