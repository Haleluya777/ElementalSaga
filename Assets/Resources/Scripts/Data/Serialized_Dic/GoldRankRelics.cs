using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AYellowpaper.SerializedCollections
{
    [CreateAssetMenu(fileName = "GoldRelics", menuName = "ScriptableObject/SerializedDic/GoldRelics")]
    [System.Serializable]
    public class GoldRankRelics : ScriptableObject
    {
        [SerializedDictionary("Num", "Relic")]
        public SerializedDictionary<int, RelicInfo> relic;
    }
}