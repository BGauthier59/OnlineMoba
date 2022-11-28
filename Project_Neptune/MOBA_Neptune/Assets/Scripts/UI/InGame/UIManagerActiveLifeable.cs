using Entities;
using Entities.FogOfWar;
using GameStates;
using UnityEngine;
using UIComponents;

public partial class UIManager
{
    [Header("HealthBar Elements")]
    [SerializeField] private GameObject healthBarPrefab;

    public void InstantiateHealthBarForEntity(int entityIndex)
    {
        var entity = EntityCollectionManager.GetEntityByIndex(entityIndex);
        Debug.Log(entity);
        if (entity == null) return;
        if (entity.GetComponent<IActiveLifeable>() == null) return;
        var canvasHealth = Instantiate(healthBarPrefab, entity.uiTransform.position + entity.offset, Quaternion.identity, entity.uiTransform);
        entity.elementsToShow.Add(canvasHealth);
        if (entity.team != GameStateMachine.Instance.GetPlayerTeam())
        {
            canvasHealth.SetActive(false);
        }
        canvasHealth.GetComponent<EntityHealthBar>().InitHealthBar(entity);
      
    }
}