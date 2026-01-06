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
            relicCount -= confirmedRelicList.Count;
        }

        switch (_tier)
        {
            case ChestTier.Bronze:
                relics.AddRange(SetRelics(bronzeRelics, relicCount));
                break;

            case ChestTier.Silver:
                relics.AddRange(SetRelics(silverRelics, relicCount));
                break;

            case ChestTier.Gold:
                relics.AddRange(SetRelics(goldRelics, relicCount));
                break;
        }
    }

    virtual public List<RelicInfo> SetRelics(BronzeRankRelics relicDic, int _count)
    {
        List<RelicInfo> _relics = new List<RelicInfo>();
        List<RelicInfo> random = new List<RelicInfo>();
        int count = _count;

        foreach (var relic in relicDic.relic.Values)
        {
            random.Add(relic);
        }

        for (int i = 0; i < count; i++)
        {
            RelicInfo relic = random[Random.Range(0, relicDic.relic.Count)];
            _relics.Add(relic);
            random.Remove(relic);
        }
        return _relics;
    }

    public List<RelicInfo> SetRelics(SilverRankRelics relicDic, int _count)
    {
        List<RelicInfo> _relics = new List<RelicInfo>();
        List<RelicInfo> random = new List<RelicInfo>();
        int count = _count;

        foreach (var relic in relicDic.relic.Values)
        {
            random.Add(relic);
        }

        for (int i = 0; i < count; i++)
        {
            RelicInfo relic = random[Random.Range(0, relicDic.relic.Count)];
            _relics.Add(relic);
            random.Remove(relic);
        }
        return _relics;
    }

    public List<RelicInfo> SetRelics(GoldRankRelics relicDic, int _count)
    {
        List<RelicInfo> _relics = new List<RelicInfo>();
        List<RelicInfo> random = new List<RelicInfo>();
        int count = _count;

        foreach (var relic in relicDic.relic.Values)
        {
            random.Add(relic);
        }

        for (int i = 0; i < count; i++)
        {
            RelicInfo relic = random[Random.Range(0, relicDic.relic.Count)];
            _relics.Add(relic);
            random.Remove(relic);
        }
        return _relics;
    }

    public void Interaction()
    {
        GameManager.instance.canvasManager.ActiveAmendPanel(relics);
    }
}
