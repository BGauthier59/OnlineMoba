using Entities;
using UnityEngine;
using UnityEngine.UI;

namespace UIComponents
{
    public class EntityResourceBar : MonoBehaviour
    {
        [SerializeField] private Image resourceBar;
        private IResourceable resourceable;
  
 
        public void InitResourceBar(Entity entity)
        {
            resourceable = (IResourceable)entity;

         
            transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
            resourceBar.fillAmount = resourceable.GetCurrentResourcePercent();

            resourceable.OnSetCurrentResourceFeedback += UpdateFillPercent;
            resourceable.OnSetCurrentResourcePercentFeedback += UpdateFillPercentByPercent;
            resourceable.OnIncreaseCurrentResourceFeedback += UpdateFillPercent;
            resourceable.OnDecreaseCurrentResourceFeedback += UpdateFillPercent;
            resourceable.OnIncreaseMaxResourceFeedback += UpdateFillPercent;
            resourceable.OnDecreaseMaxResourceFeedback += UpdateFillPercent;
        }

        private void UpdateFillPercentByPercent(float value)
        {
            resourceBar.fillAmount = value;
        }

        private void UpdateFillPercent(float value)
        {
            resourceBar.fillAmount = resourceable.GetCurrentResource();
        }
        
    }
}