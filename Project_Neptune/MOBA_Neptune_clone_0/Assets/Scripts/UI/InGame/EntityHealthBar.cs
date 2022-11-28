using Entities;
using UnityEngine;
using UnityEngine.UI;

namespace UIComponents
{
    public class EntityHealthBar : MonoBehaviour
    {
        [SerializeField] private Image healthBar;
        private IDamageable lifeable;
        private IDeadable deadable;
        private Camera cam;

        private void Start()
        {
            cam = Camera.main;
        }
    
        public void InitHealthBar(Entity entity)
        {
            lifeable = entity.GetComponent<IDamageable>();
            deadable = entity.GetComponent<IDeadable>();
        
            transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward, cam.transform.rotation * Vector3.up);
            healthBar.fillAmount = lifeable.GetCurrentHpPercent();

            lifeable.OnSetCurrentHpFeedback += UpdateFillPercent;
            lifeable.OnSetCurrentHpPercentFeedback += UpdateFillPercentByPercent;
            lifeable.OnIncreaseCurrentHpFeedback += UpdateFillPercent;
            lifeable.OnDecreaseCurrentHpFeedback += UpdateFillPercent;
            lifeable.OnIncreaseMaxHpFeedback += UpdateFillPercent;
            lifeable.OnDecreaseMaxHpFeedback += UpdateFillPercent;
            deadable.OnSetCanDieFeedback += DeactivateHealth;
            deadable.OnReviveFeedback += ActivateHealth;
        }

        private void UpdateFillPercentByPercent(float value)
        {
            healthBar.fillAmount = value;
        }
    
        private void UpdateFillPercent(float value)
        {
            healthBar.fillAmount = lifeable.GetCurrentHpPercent();
        }

        private void ActivateHealth()
        {
            gameObject.SetActive(true);
        }

        private void DeactivateHealth(bool active)
        {
            gameObject.SetActive(!active);
        }
    }
}