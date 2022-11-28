using Entities;
using Entities.Champion;
using UnityEngine;
using UnityEngine.UI;

public class ChampionHUD : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private Image resourceBar;
    [SerializeField] private Image spellOne;
    [SerializeField] private Image spellTwo;
    [SerializeField] private Image spellThree;
    private IResourceable resourceable;
    private IActiveLifeable lifeable;

    public void InitHUD(Entity entity)
    {
        lifeable = entity.GetComponent<IActiveLifeable>();
        resourceable = entity.GetComponent<IResourceable>();

        healthBar.fillAmount = lifeable.GetCurrentHpPercent();
        resourceBar.fillAmount = resourceable.GetCurrentResourcePercent();
        
        // TODO: Set cooldowns effects

        lifeable.OnSetCurrentHpFeedback += UpdateFillPercentHealth;
        lifeable.OnSetCurrentHpPercentFeedback += UpdateFillPercentByPercentHealth;
        lifeable.OnIncreaseCurrentHpFeedback += UpdateFillPercentHealth;
        lifeable.OnDecreaseCurrentHpFeedback += UpdateFillPercentHealth;
        lifeable.OnIncreaseMaxHpFeedback += UpdateFillPercentHealth;
        lifeable.OnDecreaseMaxHpFeedback += UpdateFillPercentHealth;
        
        resourceable.OnSetCurrentResourceFeedback += UpdateFillPercentResource;
        resourceable.OnSetCurrentResourcePercentFeedback += UpdateFillPercentByPercentResource;
        resourceable.OnIncreaseCurrentResourceFeedback += UpdateFillPercentResource;
        resourceable.OnDecreaseCurrentResourceFeedback += UpdateFillPercentResource;
        resourceable.OnIncreaseMaxResourceFeedback += UpdateFillPercentResource;
        resourceable.OnDecreaseMaxResourceFeedback += UpdateFillPercentResource;
        
    }
    
    private void UpdateFillPercentByPercentHealth(float value)
    {
        healthBar.fillAmount = value;
    }
    
    private void UpdateFillPercentHealth(float value)
    {
        healthBar.fillAmount = lifeable.GetCurrentHpPercent();
    }
    
    private void UpdateFillPercentByPercentResource(float value)
    {
        resourceBar.fillAmount = value;
    }

    private void UpdateFillPercentResource(float value)
    {
        resourceBar.fillAmount = resourceable.GetCurrentResource();
    }
}
