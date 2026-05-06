using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour, IDataInitializeable
{
    public PlayableCharacter PlayerUnit;
    public GameObject TestTargetUnit;
    private Unit unit;
    private IAttackable unitAttack;

    [Header("플레이어의 돈 잔량")]
    public int money;

    public void DataInitialize()
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
