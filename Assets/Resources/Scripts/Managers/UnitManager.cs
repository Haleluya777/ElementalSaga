using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour, IDataInitializable
{
    public GameObject PlayerUnit;
    private Unit unit;
    private IAttackable unitAttack;

    public void DataInit()
    {
        unit = PlayerUnit.GetComponent<Unit>();
        unitAttack = unit.GetComponentInChildren<IAttackable>();
    }

    public void EquipRelic(RelicInfo relic)
    {
        Debug.Log("유물 장착");
        unitAttack.RelicPowers.Add(relic.relicModule);
    }
}
