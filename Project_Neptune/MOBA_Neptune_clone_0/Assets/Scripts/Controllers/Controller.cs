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
         
         /// <summary>
         /// Link Inputs to CallBacks Actions and entity
         /// </summary>
         protected virtual void Link(Entity entity)
         {
             if(controlledEntity != null) Unlink();
             controlledEntity = entity;
         }

         /// <summary>
         /// Unlink Inputs to CallBacks Actions
         /// </summary>
         protected virtual void Unlink()
         {
             controlledEntity = null;
         }
    }
}