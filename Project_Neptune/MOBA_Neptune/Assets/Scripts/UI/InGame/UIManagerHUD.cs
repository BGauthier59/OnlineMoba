using Entities.Champion;
using GameStates;
using UnityEngine;

public partial class UIManager
{
    [SerializeField] private ChampionHUD[] championOverlays;

    public void InstantiateChampionHUD()
    {
        Debug.Log("Has to be modified.");
        
        
        var champion = GameStateMachine.Instance.GetPlayerChampion();

        if (champion == null) return;
        if (!champion.photonView.IsMine) return;
        
        var canvasChampion = Instantiate(championOverlays[0], transform);
        canvasChampion.InitHUD(champion);
    }
}