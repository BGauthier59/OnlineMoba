using Photon.Pun;
using Entities;

namespace Controllers
{
    public abstract class Controller : MonoBehaviourPun
    {
        protected Entity controlledEntity;

        private void Awake()
        {
            OnAwake();
        }

         protected virtual void OnAwake()
         {
             controlledEntity = GetComponent<Entity>();
         }
         
         protected virtual void Link(Entity entity)
         {
             if(controlledEntity != null) Unlink();
             controlledEntity = entity;
         }

         protected virtual void Unlink()
         {
             controlledEntity = null;
         }
    }
}