using System.Linq;
using Entities;
using Entities.Champion;
using UnityEngine;

public partial class UIManager
{
    [SerializeField] private ChampionHUD[] championOverlays;

    // Choose the right HUD to show based on the champion the player is playing
    public void InstantiateChampionHUD(int entityIndex)
    {
        var entity = EntityCollectionManager.GetEntityByIndex(entityIndex);
        if (entity == null) return;
        if (entity.GetComponent<Champion>() == null) return;
        // TODO: How to identify the champion and show the right HUD?
        ChampionHUD championHUD = championOverlays.FirstOrDefault(c => c.name.Contains(entity.name));
        // if (!photonView.isMine) return;
        if (championHUD == null) return;
        var canvasChampion = Instantiate(championHUD, entity.transform);
        canvasChampion.InitHUD(entity);
    }
}
