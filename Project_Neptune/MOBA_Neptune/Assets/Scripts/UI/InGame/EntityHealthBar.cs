using Entities;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace UIComponents
{
    public class EntityHealthBar : MonoBehaviourPun
    {
        [SerializeField] private Image healthBar;
        private IDamageable lifeable;
        
        public void InitHealthBar(Entity entity)
        {
            GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            
            lifeable = (IDamageable)entity;
            //photonView.RPC("SetHUDVisibleRPC", RpcTarget.MasterClient, entity.entityIndex);
            
            transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
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
            healthBar.fillAmount = lifeable.GetCurrentHp()/lifeable.GetMaxHp();
        }
    
        private void UpdateFillPercent(float value)
        {
            healthBar.fillAmount = lifeable.GetCurrentHp()/lifeable.GetMaxHp();
        }

        /*
        [PunRPC]
        public void SetHUDVisibleRPC(int entityIndex)
        {
            EntityCollectionManager.GetEntityByIndex(entityIndex).elementsToShow.Add(gameObject);
            photonView.RPC("SyncSetHUDVisibleRPC", RpcTarget.All, entityIndex);
        }

        [PunRPC]
        public void SyncSetHUDVisibleRPC(int entityIndex)
        {
            EntityCollectionManager.GetEntityByIndex(entityIndex).elementsToShow.Add(gameObject);
        }*/
    }
}