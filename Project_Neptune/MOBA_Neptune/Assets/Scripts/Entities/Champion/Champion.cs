using System;
using Capacities.Passive_Capacities;
using Controllers;
using Controllers.Inputs;
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
        public ChampionInputController controller;
        public Transform rotateParent;
        private Vector3 respawnPos;

        private UIManager uiManager;
        public Camera camera;

        public CollisionBlocker blocker;
        
        public LineRenderer grabLine;
        private bool isLinked;

        [SerializeField] private MeshRenderer teamConeRenderer;

        protected override void OnStart()
        {
            base.OnStart();
            uiManager = UIManager.Instance;
            camera = Camera.main;

            blocker.characterColliderBlocker.enabled = true;
            blocker.SetUpBlocker();
        }

        protected override void OnUpdate()
        {
            if (!isLinked) return;
            
            if (GameStateMachine.Instance.GetPlayerChampion() == this)
                RotateMath();
        }

        protected override void OnFixedUpdate()
        {
            if (!isLinked) return;

            if (GameStateMachine.Instance.GetPlayerChampion() != this) return;
            Move();
            Rotate();
        }

        public override void OnInstantiated()
        {
            base.OnInstantiated();
        }

        public override void OnInstantiatedFeedback() { }

        public void ApplyChampionSO(byte championSoIndex, Enums.Team newTeam)
        {
            team = newTeam;

            foreach (var tc in GameStateMachine.Instance.teamColors)
            {
                if (tc.team == team)
                {
                    teamConeRenderer.material.SetColor("_EmissionColor", tc.color);
                    break;
                }
            }

            var pos = transform;
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

            respawnPos = transform.position = pos.position;

            uiManager = UIManager.Instance;

            if (uiManager != null)
            {
                uiManager.InstantiateHealthBarForEntity(entityIndex);
                uiManager.InstantiateResourceBarForEntity(entityIndex);
            }

            rb.velocity = Vector3.zero;
            RequestSetCanDie(true);
            RequestSetCanMove(true);
            
            isLinked = true;
        }
        
        private void OnGUI()
        {
            if (!GameStateMachine.Instance.GetPlayerChampion()) return;
                
            GUILayout.BeginArea(new Rect(200,200,200,500));
            GUILayout.BeginVertical();

            foreach (var players in GameStateMachine.Instance.debugList)
            {
                if (players.championPhotonViewId != 0)
                {
                    GUILayout.Label($"Player {players.championPhotonViewId} - points : {EntityCollectionManager.GetEntityByIndex(players.championPhotonViewId).currentPointCarried}");
                }
            }
        
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }
    }
}