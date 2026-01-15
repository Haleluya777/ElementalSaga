using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AYellowpaper.SerializedCollections
{
    [CreateAssetMenu(fileName = "BronzeRelics", menuName = "ScriptableObject/SerializedDic/BronzeRelics")]
    [System.Serializable]
    public class BronzeRankRelics : RelicSerializedDic
    {
        [SerializedDictionary("Num", "Relic")]
        public SerializedDictionary<int, RelicInfo> relic;
        private SerializedDictionary<int, RelicInfo> currentRelics;
        private List<RelicInfo> relics = new List<RelicInfo>();

        public override void RelicInit()
        {
            foreach (var _relic in relic)
            {
                _relic.Value.tier = AmendChest.RelicTier.Bronze;
            }
            currentRelics = new SerializedDictionary<int, RelicInfo>(relic);
        }

        public override void RemoveRelic(RelicInfo relic)
        {
            for (int i = 0; i < currentRelics.Count; i++)
            {
                if (currentRelics[i] == relic)
                {
                    currentRelics.Remove(i);
                }
            }
        }

        public override List<RelicInfo> GetRandomRelicList(int cnt)
        {
            List<RelicInfo> randomRelics = new List<RelicInfo>();

            foreach (var _relic in currentRelics)
            {
                relics.Add(_relic.Value);
            }

            for (int i = 0; i < cnt; i++)
            {
                RelicInfo _relic = relics[Random.Range(0, relics.Count)];
                randomRelics.Add(_relic);
                relics.Remove(_relic);
            }
            relics.Clear();
            return randomRelics;
        }
    }
}