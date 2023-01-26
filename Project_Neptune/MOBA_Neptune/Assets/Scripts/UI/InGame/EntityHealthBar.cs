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
        [SerializeField] private IDeadable deadable;
        private Camera mainCam;
        public PhotonView photonView;

        public void InitHealthBar(Champion champion)
        {
            lifeable = champion.GetComponent<IDamageable>();
            deadable = champion.GetComponent<IDeadable>();
            healthBar.fillAmount = lifeable.GetCurrentHpPercent();
            lifeable = (IDamageable)champion;
            healthBar.fillAmount = lifeable.GetCurrentHpPercent();
            lifeable.OnSetCurrentHpFeedback += UpdateFillPercent;
            lifeable.OnSetCurrentHpPercentFeedback += UpdateFillPercentByPercent;
            lifeable.OnIncreaseCurrentHpFeedback += UpdateFillPercent;
            lifeable.OnDecreaseCurrentHpFeedback += UpdateFillPercent;
            lifeable.OnIncreaseMaxHpFeedback += UpdateFillPercent;
            lifeable.OnDecreaseMaxHpFeedback += UpdateFillPercent;
            deadable.OnDieFeedback += DisableLifeBar;
            deadable.OnReviveFeedback += EnableLifeBar;
        }

        private void UpdateFillPercentByPercent(float value)
        {
            healthBar.fillAmount = lifeable.GetCurrentHp() / lifeable.GetMaxHp();
        }

        private void UpdateFillPercent(float value)
        {
            healthBar.fillAmount = lifeable.GetCurrentHp() / lifeable.GetMaxHp();
        }

        private void EnableLifeBar()
        {
            healthBar.transform.parent.gameObject.SetActive(true);
        }

        private void DisableLifeBar()
        {
            healthBar.transform.parent.gameObject.SetActive(false);
        }
    }
}