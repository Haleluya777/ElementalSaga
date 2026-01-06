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
    }
}