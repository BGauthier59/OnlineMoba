using Entities;
using UnityEngine;
using UnityEngine.UI;

public class MinionHealthBar : MonoBehaviour
{
    [SerializeField] private Image minionHealthBar;
    private IDamageable lifeable;
    private IDeadable deadable;
    
    void Start()
    {
        lifeable = (IDamageable)GetComponent<Entity>();
        deadable = (IDeadable)GetComponent<Entity>();
        minionHealthBar.fillAmount = lifeable.GetCurrentHpPercent();
        lifeable.OnDecreaseCurrentHpFeedback += UpdateFillPercent;
        deadable.OnDieFeedback += DisableLifeBar;
    }

    private void UpdateFillPercent(float value)
    {
        minionHealthBar.fillAmount = lifeable.GetCurrentHp()/lifeable.GetMaxHp();
    }

    private void DisableLifeBar()
    {
        minionHealthBar.transform.parent.gameObject.SetActive(false);
    }
}
