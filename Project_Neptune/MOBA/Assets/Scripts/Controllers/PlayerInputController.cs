using Photon.Pun;

namespace Controllers.Inputs
{
    public abstract class PlayerInputController : Controller
    {
        protected PlayerInputs inputs;
        
        /// <summary>
        /// Setup the InputMap of The Player inputs
        /// </summary>
        private void SetupInputMap()
        {
            InputManager.PlayerMap = new PlayerInputs();
            InputManager.PlayerMap.Enable();
            inputs = InputManager.PlayerMap;
        }
        
        public void LinkControlsToPlayer()
        {
            if(!controlledEntity.photonView.IsMine) return;
            SetupInputMap();
            Link(controlledEntity);
        }

        public void LinkCameraToPlayer()
        {
            if(!controlledEntity.photonView.IsMine) return;
            CameraController.Instance.LinkCamera(controlledEntity.transform);
        }

        public void TransferOwnerShipToMaster()
        {
            if(!controlledEntity.photonView.IsMine) return;
            photonView.TransferOwnership(PhotonNetwork.MasterClient.ActorNumber);
        }
    }
}
