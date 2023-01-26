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

    public Dictionary<NewActiveCapacity, SpellHolder>
        spellHolderDict = new Dictionary<NewActiveCapacity, SpellHolder>();

    public class SpellHolder
    {
        public Image spellCooldown;
        public TextMeshProUGUI spellCooldownText;

        public void Setup(Image image, TextMeshProUGUI textMeshProUGUI)
        {
            spellCooldown = image;
            spellCooldown.fillAmount = 0;
            spellCooldownText = textMeshProUGUI;
        }

        public void StartTimer(double cooldown)
        {
            var timer = 0.0;
            var tckRate = GameStateMachine.Instance.tickRate;

            GameStateMachine.Instance.OnTickFeedback += Tick;

            void Tick()
            {
                timer += 1.0 / tckRate;
                spellCooldown.fillAmount = 1 - (float)(timer / cooldown);
                spellCooldownText.text = (cooldown - timer).ToString("F1");
                if (!(timer > cooldown)) return;
                GameStateMachine.Instance.OnTickFeedback -= Tick;
                spellCooldown.fillAmount = 0;
                spellCooldownText.text = $"";
            }
        }
    }

    public void InitHUD(Champion newChampion)
    {
        champion = newChampion;
        champion.myHud = this;
        lifeable = champion.GetComponent<IDamageable>();
        healthBar.fillAmount = lifeable.GetCurrentHpPercent();
        LinkToEvents();
        UpdateIcons(newChampion);

        championIcon.sprite = champion.team == Enums.Team.Team1 ? champion.championIcon[0] : champion.championIcon[1];
        championSpell.sprite = champion.championSpellKit;
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
        healthBar.fillAmount = lifeable.GetCurrentHp() / lifeable.GetMaxHp();
    }

    private void UpdateFillPercentHealth(float value)
    {
        healthBar.fillAmount = lifeable.GetCurrentHp() / lifeable.GetMaxHp();
    }
}