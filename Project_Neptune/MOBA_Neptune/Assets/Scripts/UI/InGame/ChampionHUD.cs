using System.Collections.Generic;
using Entities;
using Entities.Capacities;
using Entities.Champion;
using GameStates;
using UnityEngine;
using UnityEngine.UI;

public class ChampionHUD : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private Image healthBar;
    [SerializeField] private Image championIcon;
    [SerializeField] private Image championSpell;
    [SerializeField] private Image spellPassiveCooldown;
    [SerializeField] private Image autoAttackCooldown;
    [SerializeField] private Image spellOneCooldown;

    [Header("IO Specs")] 
    [SerializeField] private GameObject ioAutoAttackSlotsHolder;
    
    private Champion champion;
    private IDamageable lifeable;
    private ICastable castable;
    private SpellHolder passiveHolder;
    private Dictionary<byte, SpellHolder> spellHolderDict = new Dictionary<byte, SpellHolder>();

    public class SpellHolder
    {
        public Image spellIcon;
        public Image spellCooldown;

        public void Setup(Sprite image)
        {
            spellIcon.sprite = image;
            spellCooldown.fillAmount = 0;
        }

        public void ChangeIcon(Sprite image)
        {
            spellIcon.sprite = image;
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
                if (!(timer > coolDown)) return;
                GameStateMachine.Instance.OnTick -= Tick;
                spellCooldown.fillAmount = 0;
            }
        }
    }
    
    public void InitHUD(Champion newChampion)
    {
        champion = newChampion;
        castable = champion.GetComponent<ICastable>();
        lifeable = champion.GetComponent<IDamageable>();
        healthBar.fillAmount = lifeable.GetCurrentHpPercent();
        LinkToEvents();
        UpdateIcons(champion);
    }

    private void InitHolders()
    {
        Debug.Log("Has to be modified.");
        
        //var so = champion.championSo;
        //spellPassive.sprite = champion.passiveCapacitiesList[0].AssociatedPassiveCapacitySO().icon;
        //spellOne.sprite = so.activeCapacities[0].icon;
        //spellTwo.sprite = so.activeCapacities[1].icon;
        //spellUltimate.sprite = so.ultimateAbility.icon;
    }

    private void LinkToEvents()
    {
        castable.OnCastFeedback += UpdateCooldown;

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
        
        championIcon.sprite = champion.team == Enums.Team.Team1 ? champion.championIcon[0] : champion.championIcon[1];
        championSpell.sprite = champion.championSpellKit;

        if (champion.championName == "Io")
        {
            ioAutoAttackSlotsHolder.SetActive(true);
        }

        /*
        var so = champion.championSo;
        passiveHolder = new SpellHolder
        {
            spellIcon = spellPassive,
            spellCooldown = spellPassiveCooldown
        };
        var spellAutoAttack = new SpellHolder
        {
            spellIcon = spellOne,
            spellCooldown = spellOneCooldown
        };
        var spellTwoHolder = new SpellHolder
        {
            spellIcon = spellTwo,
            spellCooldown = spellTwoCooldown
        };
        var ultimateHolder = new SpellHolder
        {
            spellIcon = spellUltimate,
            spellCooldown = spellUltimateCooldown
        };
        spellHolderDict.Add(so.activeCapacitiesIndexes[0], spellOneHolder);
        spellHolderDict.Add(so.activeCapacitiesIndexes[1], spellTwoHolder);
        if(!spellHolderDict.ContainsKey(so.ultimateAbilityIndex))spellHolderDict.Add(so.ultimateAbilityIndex, ultimateHolder);
        else Debug.Log("A FIXE, CA BUG ");
        
        if(so.passiveCapacities.Length != 0)
        passiveHolder.Setup(so.passiveCapacities[0].icon);
        spellOneHolder.Setup(so.activeCapacities[0].icon);
        //spellTwoHolder.Setup(so.activeCapacities[1].icon);
        ultimateHolder.Setup(so.ultimateAbility.icon);
        */
    }

    private void UpdateCooldown(byte capacityIndex, int[] intArray, Vector3[] vectors, ActiveCapacity capacity)
    {
        spellHolderDict[capacityIndex].StartTimer(CapacitySOCollectionManager.GetActiveCapacitySOByIndex(capacityIndex).cooldown) ;
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
