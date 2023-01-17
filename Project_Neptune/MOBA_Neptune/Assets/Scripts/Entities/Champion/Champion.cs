using System.Collections.Generic;
using Controllers.Inputs;
using GameStates;
using JetBrains.Annotations;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Entities.Champion
{
    public partial class Champion : Entity
    {
        public string championName;
        public ChampionInputController controller;
        public Transform rotateParent;
        private Vector3 respawnPos;
        private UIManager uiManager;
        public Camera camera;
        public CollisionBlocker blocker;
        public LineRenderer grabLine;
        public List<TargetIndicator> targetIndicators;
        private bool isLinked;
        public TextMeshPro pointsText;
    
        [SerializeField] private MeshRenderer teamConeRenderer;

        [SerializeField] private Renderer[] meshes;
        [SerializeField] private Material orangeTeam;
        [SerializeField] private Material violetTeam;

        // Which tower is link
        private int towerLinkedIndex;
        
        protected override void OnStart()
        {
            base.OnStart();
            uiManager = UIManager.Instance;
            camera = Camera.main;
            blocker.characterColliderBlocker.enabled = true;
            blocker.SetUpBlocker();
            dieCanvas.SetActive(false);
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

            if (team == Enums.Team.Neutral)
            {
                Debug.LogError("Neutral team?");
                return;
            }
            
            var material = team == Enums.Team.Team1 ? orangeTeam : violetTeam;

            foreach (var rd in meshes)
            {
                rd.material = material;
            }

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
            LinkTower();
            
            EntityCollectionManager.AllChampion.Add(this);
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
        
        private void LinkTower()
        {
            if (championName == "Io")
            {
                if (team == Enums.Team.Team1)
                {
                    var tower1 = EntityCollectionManager.GetEntityByIndex(300).GetComponent<Tower>();
                    towerLinkedIndex = tower1.entityIndex;
                    tower1.entityLinkIndex = entityIndex;
                }
                else if (team == Enums.Team.Team2)
                {
                    var tower3 = EntityCollectionManager.GetEntityByIndex(301).GetComponent<Tower>();
                    towerLinkedIndex = tower3.entityIndex;
                    tower3.entityLinkIndex = entityIndex;
                }
            }
            else
            {
                if (team == Enums.Team.Team1)
                {
                    var tower0 = EntityCollectionManager.GetEntityByIndex(302).GetComponent<Tower>();
                    towerLinkedIndex = tower0.entityIndex;
                    tower0.entityLinkIndex = entityIndex;
                }
                else if (team == Enums.Team.Team2)
                {
                    var tower2 = EntityCollectionManager.GetEntityByIndex(303).GetComponent<Tower>();
                    towerLinkedIndex = tower2.entityIndex;
                    tower2.entityLinkIndex = entityIndex;
                }
            }
            
            photonView.RPC("SyncTowerActiveRpc", RpcTarget.All, towerLinkedIndex);
        }

        [PunRPC] [UsedImplicitly]
        public void SyncTowerActiveRpc(int myTowerIndex)
        {
            var myTower = EntityCollectionManager.GetEntityByIndex(myTowerIndex).GetComponent<Tower>();
            myTower.isActive = true;
            myTower.desactivateIcon.SetActive(false);
        }
    }
}