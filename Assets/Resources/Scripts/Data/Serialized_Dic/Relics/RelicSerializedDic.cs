using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RelicSerializedDic : ScriptableObject
{
    public abstract void RelicInit();
    public abstract List<RelicInfo> GetRandomRelicList(int cnt);
    public abstract void RemoveRelic(RelicInfo relic);
}
