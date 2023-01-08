using UnityEngine;

namespace Entities.FogOfWar
{
    public class EntityFOWShowLink : MonoBehaviour
    {
        public void LinkEntity(Entity entity)
        {
            entity.elementsToShow.Add(gameObject);
        }
    }
}
