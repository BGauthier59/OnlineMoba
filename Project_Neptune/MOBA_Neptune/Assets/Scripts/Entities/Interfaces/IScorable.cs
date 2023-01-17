 using Entities;
 using Photon.Pun;

 public interface IScorable
{
    // Minion & Champion ---
    public void CashierRequestIncreaseScore(Entity entityWhoScored);
    public void SyncCashierIncreaseScoreRPC(int value, int entityIndex);
    public void CashierIncreaseScoreRPC(int value, int entityIndex);
    
    // Champion Increase Score ---
    public void ChampionRequestIncreaseScore(int value, Entity entityToInscreasePoints);
    public void SyncChampionIncreaseScore(int value, int entityToInscreasePoints);
    public void ChampionIncreaseScore(int value, int entityToInscreasePoints);
    
    //Champion Remove Score (Set to 0) ---
    public void ChampionRequestRemoveScore(Entity entityWhoScored);
    public void SyncChampionRemoveScore(int entityWhoScored);
}
