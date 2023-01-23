using Entities;
using Entities.FogOfWar;
using GameStates;
using Photon.Pun;
using UnityEngine;
using UIComponents;

public partial class UIManager
{
    [Header("HealthBar Elements")] [SerializeField]
    private GameObject healthBarPrefab;

    public void InstantiateHealthBarForEntity(int entityIndex)
    {
        // var entity = EntityCollectionManager.GetEntityByIndex(entityIndex);
        // if (entity == null) return;
        // if (!entity.photonView.IsMine) return;
        // if (entity.GetComponent<IDamageable>() == null) return;
        
        
        //var canvasHealth = PhotonNetwork.Instantiate(healthBarPrefab.name, entity.uiTransform.position + entity.guiOffset, Quaternion.identity);
        //canvasHealth.transform.parent = entity.uiTransform;
        //entity.elementsToShow.Add(canvasHealth);
        
        // if (entity.team != GameStateMachine.Instance.GetPlayerTeam())
        // {
        //     canvasHealth.SetActive(false);
        // }

        //canvasHealth.GetComponent<EntityHealthBar>().InitHealthBar(entity);
    }
}