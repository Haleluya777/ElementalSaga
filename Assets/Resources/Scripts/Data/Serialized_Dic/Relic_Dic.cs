using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AYellowpaper.SerializedCollections
{
    [CreateAssetMenu(fileName = "SerializedDic_Relic", menuName = "ScriptableObject/SerializedDic/SerializedDic_Relic")]
    [System.Serializable]
    public class Relic_Dic : ScriptableObject
    {
        [SerializedDictionary("Num", "Relic")]
        public SerializedDictionary<int, RelicInfo> relic;
    }
}

