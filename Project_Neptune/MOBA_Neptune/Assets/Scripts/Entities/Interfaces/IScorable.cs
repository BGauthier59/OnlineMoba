public interface IScorable
{
    public void RequestIncreaseScore(int value);
    
    public void SyncIncreaseScoreRPC(int value);
    
    public void SetIncreaseScoreRPC(int value);
}
