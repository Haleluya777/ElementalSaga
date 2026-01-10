using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour, IDataInitializable
{
    public GameObject PlayerUnit;
    private Unit unit;
    private IAttackable unitAttack;

    [Header("플레이어의 돈 잔량")]
    public int money;

    public void DataInit()
    {
        money = 100;
        unit = PlayerUnit.GetComponent<Unit>();
        unitAttack = unit.GetComponentInChildren<IAttackable>();
    }

    public void EquipRelic(RelicInfo relic)
    {
        Debug.Log("유물 장착");
        unitAttack.RelicPowers.Add(relic.relicModule);
    }
}
