using Entities;

public interface IScorable
{
    public void RequestIncreaseScore(int value, Entity entityWhoScored);
    
    public void SyncIncreaseScoreRPC(int value);
    
    public void SetIncreaseScoreRPC(int value);
}
