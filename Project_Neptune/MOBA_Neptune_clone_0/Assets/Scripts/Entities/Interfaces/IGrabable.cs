namespace Entities.Interfaces
{
    public interface IGrabable
    {
        public Enums.Team GetGrabbedTeam();
        public void RequestSetCanBeGrabbed(bool canBeGrabbed);

        public void SetCanBeGrabbedRPC(bool canBeGrabbed);

        public void SyncCanBeGrabbedRPC(bool canBeGrabbed);
        
        public void OnGrabbed();

        public void SyncOnGrabbedRPC();
    }
}
