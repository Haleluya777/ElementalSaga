using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class AmendChest : PoolAble, IInteractable
{
    public enum ChestTier { Bronze, Silver, Gold }
    [SerializeField] private BronzeRankRelics bronzeRelics;
    [SerializeField] private SilverRankRelics silverRelics;
    [SerializeField] private GoldRankRelics goldRelics;

    [SerializeField] private ChestTier tier;
    [SerializeField] private List<RelicInfo> relics = new List<RelicInfo>();

    public void ChestInit(ChestTier _tier, List<RelicInfo> confirmedRelicList = null)
    {
        tier = _tier;
        int relicCount = GameManager.instance.stageManager.amendCount;
        if (confirmedRelicList is not null)
        {
            relics = confirmedRelicList;
            relicCount = relicCount - confirmedRelicList.Count > 0 ? relicCount - confirmedRelicList.Count : 0;
        }

        switch (_tier)
        {
            case ChestTier.Bronze:
                relics.AddRange(bronzeRelics.GetRandomRelicList(relicCount));
                break;

            case ChestTier.Silver:
                relics.AddRange(silverRelics.GetRandomRelicList(relicCount));
                break;

            case ChestTier.Gold:
                relics.AddRange(goldRelics.GetRandomRelicList(relicCount));
                break;
        }
    }

    public void Interaction()
    {
        GameManager.instance.canvasManager.ActiveAmendPanel(relics);
    }
}
