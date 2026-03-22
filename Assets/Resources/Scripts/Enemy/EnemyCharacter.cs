using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : Unit
{
    [Header("적들의 기본 정보")]
    [SerializeField] private EnemyMap maps;

    [Header("캐릭터 ID")]
    public int id;

    [Header("적들만이 가지고 있는 고유한 스탯들")]
    public float MeleeRange;
    public float RangerRange;
    public bool init = false;

    void Start()
    {
        if (init) DataInit();
    }

    private void DataInit()
    {
        unitData = new UnitData(maps.GetDatas(id));
        base.TakeDamageEvent += GetStunGage;
        foreach (var init in GetComponentsInChildren<IDataInitializeable>())
        {
            init.DataInitialize();
        }
    }

    private void GetStunGage(int dmg, ISkillCaster attacker, GameObject character)
    {

    }
}
