using System.Collections;
using System.Collections.Generic;
using Entities;
using Entities.Minion.MinionStream;
using UnityEngine;
using UnityEngine.UI;

public class MinionHealthBar : MonoBehaviour
{
    [SerializeField] private Image minionHealthBar;
    private IDamageable lifeable;
    
    // Start is called before the first frame update
    void Start()
    {
        lifeable = (IDamageable)GetComponent<Entity>();
        minionHealthBar.fillAmount = lifeable.GetCurrentHpPercent();
        lifeable.OnDecreaseCurrentHpFeedback += UpdateFillPercent;
    }

    private void UpdateFillPercent(float value)
    {
        minionHealthBar.fillAmount = lifeable.GetCurrentHp()/lifeable.GetMaxHp();
    }
}
