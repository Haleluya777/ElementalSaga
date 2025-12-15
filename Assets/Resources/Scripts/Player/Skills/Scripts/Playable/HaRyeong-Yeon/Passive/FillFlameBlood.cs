using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillFlameBlood : OnHitEventBase
{
    [SerializeField] private FlameBlood flameBlood;

    public override void Execute(GameObject target, ISkillCaster caster, int dmg)
    {
        flameBlood.flameBloodGage += 1;
    }

}
