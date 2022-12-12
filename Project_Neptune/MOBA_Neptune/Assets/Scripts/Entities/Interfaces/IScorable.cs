 using Entities;
 using Photon.Pun;

 public interface IScorable
{
    // Minion & Champion ---
    public void CashierRequestIncreaseScore(int value, Entity entityWhoScored);
    public void SyncCashierIncreaseScoreRPC(int value);
    public void CashierIncreaseScoreRPC(int value);
    
    // Champion Increase Score ---
    public void ChampionRequestIncreaseScore(int value, Entity entityToInscreasePoints);
    public void SyncChampionIncreaseScore(int value, Entity entityToInscreasePoints);
    public void ChampionIncreaseScore(int value, Entity entityToInscreasePoints);
    
    //Champion Remove Score (Set to 0) ---
    public void ChampionRequestRemoveScore(Entity entityWhoScored);
    public void SyncChampionRemoveScore(Entity entityWhoScored);
}
