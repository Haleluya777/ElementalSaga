using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AYellowpaper.SerializedCollections
{
    [CreateAssetMenu(fileName = "ObjectPools", menuName = "ScriptableObject/SerializedDic/ObjectPools")]
    [System.Serializable]
    public class ObjectPoolDic : ScriptableObject
    {
        [SerializedDictionary("Name", "Pool")]
        public SerializedDictionary<string, ObjectPooler> pools;
    }
}