using Entities;
using UnityEngine;
using UnityEngine.UI;

namespace UIComponents
{
    public class EntityResourceBar : MonoBehaviour
    {
        [SerializeField] private Image resourceBar;
        private IResourceable resourceable;
        private IDeadable deadable;
        private Camera cam;

        private void Start()
        {
            cam = Camera.main;
        }

        public void InitResourceBar(Entity entity)
        {
            resourceable = entity.GetComponent<IResourceable>();

            transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward, cam.transform.rotation * Vector3.up);
            resourceBar.fillAmount = resourceable.GetCurrentResourcePercent();

            resourceable.OnSetCurrentResourceFeedback += UpdateFillPercent;
            resourceable.OnSetCurrentResourcePercentFeedback += UpdateFillPercentByPercent;
            resourceable.OnIncreaseCurrentResourceFeedback += UpdateFillPercent;
            resourceable.OnDecreaseCurrentResourceFeedback += UpdateFillPercent;
            resourceable.OnIncreaseMaxResourceFeedback += UpdateFillPercent;
            resourceable.OnDecreaseMaxResourceFeedback += UpdateFillPercent;
            deadable.OnSetCanDieFeedback += DeactivateResource;
            deadable.OnReviveFeedback += ActivateResource;
        }

        private void UpdateFillPercentByPercent(float value)
        {
            resourceBar.fillAmount = value;
        }

        private void UpdateFillPercent(float value)
        {
            resourceBar.fillAmount = resourceable.GetCurrentResource();
        }
        
        private void ActivateResource()
        {
            gameObject.SetActive(true);
        }

        private void DeactivateResource(bool active)
        {
            resourceBar.gameObject.SetActive(!active);
        }
    }
}