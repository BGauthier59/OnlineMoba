using Photon.Pun;


namespace Entities.Champion
{
    public partial class Champion : IScorable
    {
        //------------- Unused Methods ----------------------
        
        public void CashierRequestIncreaseScore(Entity entityWhoScored, int value)
        {
        }

        public void SyncCashierIncreaseScoreRPC(int value)
        {
        }

        public void CashierIncreaseScoreRPC(int value)
        {
        }

        //----- Increase Bonbons -----------------------
        
        public void ChampionRequestIncreaseScore(int value, Entity championToIncreaseScore)
        {
            photonView.RPC("ChampionIncreaseScore", RpcTarget.MasterClient, value, championToIncreaseScore.entityIndex);
        }

        [PunRPC]
        public void SyncChampionIncreaseScore(int value, int entityIndex)
        {
            var entity = EntityCollectionManager.GetEntityByIndex(entityIndex);
            entity.currentPointCarried = value;
            
            entity.GetComponent<Champion>().pointsText.text = entity.currentPointCarried.ToString();
        }

        [PunRPC]
        public void ChampionIncreaseScore(int value, int entityIndex)
        {
            var entity = EntityCollectionManager.GetEntityByIndex(entityIndex);
            entity.currentPointCarried += value;
            
            photonView.RPC("SyncChampionIncreaseScore", RpcTarget.All, entity.currentPointCarried, entity.entityIndex);
        }

        // ----------- RemoveScore ----------------------
        public void ChampionRequestRemoveScore(Entity entityWhoScored)
        {
            photonView.RPC("SyncChampionRemoveScore", RpcTarget.All, entityWhoScored.entityIndex); 
        }
        
        [PunRPC]
        public void SyncChampionRemoveScore(int entityWhoScored)
        {
            var entity = EntityCollectionManager.GetEntityByIndex(entityIndex);
            entity.currentPointCarried = 0;
            
            entity.GetComponent<Champion>().pointsText.text = entity.currentPointCarried.ToString();
        }
    }
}
