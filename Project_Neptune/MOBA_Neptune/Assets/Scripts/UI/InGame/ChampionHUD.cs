using System.Collections.Generic;
using Controllers.Inputs;
using Entities;
using Entities.Champion;
using GameStates;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChampionHUD : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private Image championIcon;
    [SerializeField] private Image championSpell;
    
    //[SerializeField] private TextMeshProUGUI spellPassiveCooldownText;
    //[SerializeField] private Image spellPassiveCooldown;
    
    [SerializeField] private TextMeshProUGUI autoAttackCooldownText;
    [SerializeField] private Image autoAttackCooldown;
    
    [SerializeField] private TextMeshProUGUI spellOneCooldownText;
    [SerializeField] private Image spellOneCooldown;

    private Champion champion;
    private IDamageable lifeable;
    private ICastable castable;
    private SpellHolder passiveHolder;
    private Dictionary<NewActiveCapacity, SpellHolder> spellHolderDict = new Dictionary<NewActiveCapacity, SpellHolder>();

    private class SpellHolder
    {
        public Image spellCooldown;
        public TextMeshProUGUI spellCooldownText;

        public void Setup(Image image, TextMeshProUGUI textMeshProUGUI)
        {
            spellCooldown = image;
            spellCooldown.fillAmount = 0;
            spellCooldownText = textMeshProUGUI;
        }
        
        public void StartTimer(float coolDown)
        {
            var timer = 0.0;
            var tckRate = GameStateMachine.Instance.tickRate;
            
            
            GameStateMachine.Instance.OnTick += Tick;
            
            void Tick()
            {
                timer += 1.0 / tckRate;
                spellCooldown.fillAmount = 1-(float)(timer / coolDown);
                spellCooldownText.text = (1 - (float)(timer / coolDown)).ToString("F1");
                if (!(timer > coolDown)) return;
                GameStateMachine.Instance.OnTick -= Tick;
                spellCooldown.fillAmount = 0;
                spellCooldownText.text = $"";
            }
        }
    }
    
    public void InitHUD(Champion newChampion)
    {
        //castable = champion.GetComponent<ICastable>();
        lifeable = champion.GetComponent<IDamageable>();
        healthBar.fillAmount = lifeable.GetCurrentHpPercent();
        LinkToEvents();
        UpdateIcons(newChampion);
    }
    
    private void LinkToEvents()
    {
        lifeable.OnSetCurrentHpFeedback += UpdateFillPercentHealth;
        lifeable.OnSetCurrentHpPercentFeedback += UpdateFillPercentByPercentHealth;
        lifeable.OnIncreaseCurrentHpFeedback += UpdateFillPercentHealth;
        lifeable.OnDecreaseCurrentHpFeedback += UpdateFillPercentHealth;
        lifeable.OnIncreaseMaxHpFeedback += UpdateFillPercentHealth;
        lifeable.OnDecreaseMaxHpFeedback += UpdateFillPercentHealth;
    }

    private void UpdateIcons(Champion champion)
    {
        Debug.Log("Has to be modified.");
        
        var autoAttackHolder = new SpellHolder
        {
            spellCooldown = autoAttackCooldown,
            spellCooldownText = autoAttackCooldownText
        };
        var spellOneHolder = new SpellHolder
        {
            spellCooldown = spellOneCooldown,
            spellCooldownText = spellOneCooldownText
        };
        
        spellHolderDict.Add(champion.GetComponent<ChampionInputController>().attackCapacity, autoAttackHolder);
        spellHolderDict.Add(champion.GetComponent<ChampionInputController>().capacity1, spellOneHolder);
    }
    
    private void UpdateFillPercentByPercentHealth(float value)
    {
        healthBar.fillAmount = lifeable.GetCurrentHp()/lifeable.GetMaxHp();
    }
    
    private void UpdateFillPercentHealth(float value)
    {
        healthBar.fillAmount = lifeable.GetCurrentHp()/lifeable.GetMaxHp();
    }
}
