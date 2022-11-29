namespace Entities.Interfaces
{
    public interface IGrabable
    {
        public void RequestSetCanBeGrabbed(bool canBeGrabbed);

        public void SetCanBeGrabbedRPC(bool canBeGrabbed);

        public void SyncCanBeGrabbedRPC(bool canBeGrabbed);

        public void RequestOnGrabbed();

        public void OnGrabbedRPC();

        public void SyncOnGrabbedRPC();
    }
}
