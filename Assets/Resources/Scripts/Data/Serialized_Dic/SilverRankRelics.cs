using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AYellowpaper.SerializedCollections
{
    [CreateAssetMenu(fileName = "SilverRelics", menuName = "ScriptableObject/SerializedDic/SilverRelics")]
    [System.Serializable]
    public class SilverRankRelics : ScriptableObject
    {
        [SerializedDictionary("Num", "Relic")]
        public SerializedDictionary<int, RelicInfo> relic;
    }
}

