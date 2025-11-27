using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : Unit
{
    void Awake()
    {
        DataInit();
    }

    private void DataInit()
    {
        foreach (var init in GetComponentsInChildren<IDataInitializable>())
        {
            init.DataInit();
        }
    }

    public override void Dead()
    {
        base.Dead();
    }
}
