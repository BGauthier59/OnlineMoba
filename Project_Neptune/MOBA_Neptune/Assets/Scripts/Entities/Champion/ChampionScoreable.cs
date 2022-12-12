using Photon.Pun;


namespace Entities.Champion
{
    public partial class Champion : IScorable
    {
        //------------- Unused Methods ----------------------
        
        public void CashierRequestIncreaseScore(int value, Entity entityWhoScored)
        {
        }

        public void SyncCashierIncreaseScoreRPC(int value)
        {
        }

        public void CashierIncreaseScoreRPC(int value)
        {
        }

        //----- Increase Bonbons -----------------------
        
        public void ChampionRequestIncreaseScore(int value, Entity entity)
        {
            photonView.RPC("ChampionIncreaseScore", RpcTarget.MasterClient, value, entity);
        }

        [PunRPC]
        public void SyncChampionIncreaseScore(int value, Entity entity)
        {
            entity.currentPointCarried = value;
        }

        [PunRPC]
        public void ChampionIncreaseScore(int value, Entity entity)
        {
            var points =  entity.currentPointCarried += value;
            photonView.RPC("SyncChampionIncreaseScore", RpcTarget.All, points, entity);
        }

        // ----------- RemoveScore ----------------------
        public void ChampionRequestRemoveScore(Entity entityWhoScored)
        {
            photonView.RPC("SyncChampionRemoveScore", RpcTarget.All, entityWhoScored); 
        }
        
        [PunRPC]
        public void SyncChampionRemoveScore(Entity entityWhoScored)
        {
            entityWhoScored.currentPointCarried = 0;
        }
    }
}
