using Entities;
using Entities.Champion;
using GameStates;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace UIComponents
{
    public class EntityHealthBar : MonoBehaviourPun
    {
        [SerializeField] private Image healthBar;
        [SerializeField] private IDamageable lifeable;
        private Camera mainCam;
        public PhotonView photonView;

        public void InitHealthBar(Champion champion)
        {
            lifeable = champion.GetComponent<IDamageable>();
            healthBar.fillAmount = lifeable.GetCurrentHpPercent();
            lifeable = (IDamageable) champion;
            healthBar.fillAmount = lifeable.GetCurrentHpPercent();
            lifeable.OnSetCurrentHpFeedback += UpdateFillPercent;
            lifeable.OnSetCurrentHpPercentFeedback += UpdateFillPercentByPercent;
            lifeable.OnIncreaseCurrentHpFeedback += UpdateFillPercent;
            lifeable.OnDecreaseCurrentHpFeedback += UpdateFillPercent;
            lifeable.OnIncreaseMaxHpFeedback += UpdateFillPercent;
            lifeable.OnDecreaseMaxHpFeedback += UpdateFillPercent;
        }

        private void UpdateFillPercentByPercent(float value)
        {
            healthBar.fillAmount = lifeable.GetCurrentHp() / lifeable.GetMaxHp();
        }

        private void UpdateFillPercent(float value)
        {
            healthBar.fillAmount = lifeable.GetCurrentHp() / lifeable.GetMaxHp();
        }
    }
}