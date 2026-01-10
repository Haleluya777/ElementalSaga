using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AYellowpaper.SerializedCollections
{
    [CreateAssetMenu(fileName = "BronzeRelics", menuName = "ScriptableObject/SerializedDic/BronzeRelics")]
    [System.Serializable]
    public class BronzeRankRelics : ScriptableObject
    {
        [SerializedDictionary("Num", "Relic")]
        public SerializedDictionary<int, RelicInfo> relic;

        private List<RelicInfo> relics = new List<RelicInfo>();

        public List<RelicInfo> GetRandomRelicList(int cnt)
        {
            List<RelicInfo> randomRelics = new List<RelicInfo>();

            foreach (var _relic in relic)
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