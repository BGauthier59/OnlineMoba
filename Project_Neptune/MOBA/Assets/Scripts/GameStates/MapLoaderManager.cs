using Photon.Pun;
using UnityEngine;

namespace GameStates
{
    public class MapLoaderManager : MonoBehaviourPun
    {
        public static MapLoaderManager Instance;
        
        public Transform firstTeamBasePoint;
        public Transform secondTeamBasePoint;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                DestroyImmediate(gameObject);
                return;
            }

            Instance = this;
        }

        private void Start()
        {
            GameStateMachine.Instance.LoadMap();
            if (PhotonNetwork.IsMasterClient) PhotonNetwork.IsMessageQueueRunning = true;
        }
        
    }
}