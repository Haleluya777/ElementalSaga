using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class AmendChest : PoolAble, IInteractable
{
    public enum RelicTier { Bronze, Silver, Gold }

    [SerializeField] private RelicTier tier;
    [SerializeField] private List<RelicInfo> relics = new List<RelicInfo>();
    private bool firstInteraction = true;

    public void ChestInit(RelicTier _tier, List<RelicInfo> confirmedRelicList = null)
    {
        relics.Clear();
        tier = _tier;
        int relicCount = GameManager.instance.stageManager.amendCount;

        if (GameManager.instance is not null)
        {
            GameManager.instance.eventManager.ReleaseAllRelicButton -= ReleaseObject;
            GameManager.instance.eventManager.ReleaseAllRelicButton += ReleaseObject;
        }

        if (confirmedRelicList.Count > 0)
        {
            Debug.Log("고정 등장 유물이 존재.");
            relics = confirmedRelicList;
            relicCount = relicCount - confirmedRelicList.Count > 0 ? relicCount - confirmedRelicList.Count : 0;
        }

        switch (_tier)
        {
            case RelicTier.Bronze:
                relics.AddRange(GameManager.instance.relicManager.relicMaps[0].GetRandomRelicList(relicCount));
                break;

            case RelicTier.Silver:
                relics.AddRange(GameManager.instance.relicManager.relicMaps[1].GetRandomRelicList(relicCount));
                break;

            case RelicTier.Gold:
                relics.AddRange(GameManager.instance.relicManager.relicMaps[2].GetRandomRelicList(relicCount));
                break;
        }
    }

    void OnDisable()
    {
        GameManager.instance.eventManager.ReleaseAllRelicButton -= ReleaseObject;
        firstInteraction = true;
    }

    public void Interaction()
    {
        GameManager.instance.canvasManager.ActiveAmendPanel(relics, firstInteraction);
        if (firstInteraction) firstInteraction = false;
    }
}
