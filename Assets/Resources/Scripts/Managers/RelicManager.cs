using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

public class RelicManager : MonoBehaviour, IDataInitializeable
{
    public List<RelicSerializedDic> relicMaps;

    public void DataInitialize()
    {
        foreach (var relicMap in relicMaps)
        {
            relicMap.RelicInit();
        }
    }

    public void RemoveRelic(RelicInfo relic)
    {
        switch (relic.tier)
        {
            case AmendChest.RelicTier.Bronze:
                relicMaps[0].RemoveRelic(relic);
                break;

            case AmendChest.RelicTier.Silver:
                relicMaps[1].RemoveRelic(relic);
                break;

            case AmendChest.RelicTier.Gold:
                relicMaps[2].RemoveRelic(relic);
                break;
        }
    }
}
