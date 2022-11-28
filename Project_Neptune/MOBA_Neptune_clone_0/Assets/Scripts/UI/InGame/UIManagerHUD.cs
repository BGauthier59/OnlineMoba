using GameStates;
using UnityEngine;

public partial class UIManager
{
    [SerializeField] private ChampionHUD[] championOverlays;
    
    public void InstantiateChampionHUD()
    {
        var champion = GameStateMachine.Instance.GetPlayerChampion();
        if (champion == null) return;
        var canvasIndex = champion.championSo.canvasIndex;
        if (canvasIndex >= championOverlays.Length) canvasIndex = 0;
        var canvasChampion = Instantiate(championOverlays[canvasIndex], transform);
        canvasChampion.InitHUD(champion);
    }
}