using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillFlameBlood : OnHitEventBase
{
    [SerializeField] private FlameBlood flameBlood;

    public override void Execute(GameObject target, ISkillCaster caster, int dmg)
    {
        if(flameBlood.flameBloodGage >= 50) return;
        flameBlood.flameBloodGage += 1;
    }

}
