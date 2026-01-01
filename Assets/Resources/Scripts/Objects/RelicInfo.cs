using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RelicInfo
{
    public Sprite sprite;
    public Skill_Module relicModule;
    public string explain;

    public RelicInfo(RelicInfo relic)
    {
        this.sprite = relic.sprite;
        this.relicModule = relic.relicModule;
        this.explain = relic.explain;
    }
}
