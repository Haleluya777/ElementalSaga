using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : Unit
{
    [Header("적들의 기본 정보")]
    [SerializeField] private UnitDataMap maps;

    [Header("캐릭터 ID")]
    public int id;

    void Awake()
    {
        //DataInit();
    }

    private void DataInit()
    {
        unitData = new UnitData(maps.GetDatas(id));
        foreach (var init in GetComponentsInChildren<IDataInitializable>())
        {
            init.DataInit();
        }
    }
}
