using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//피격 시 실행되는 스킬들의 기초.
//Ex) 직접 공격을 받을 시 데미지 반사, 피격 시 특정 자원 획득 등.
public abstract class DamagedEventBase : ScriptableObject
{
    protected Unit unit; //이 스킬 효과를 시전하는 유닛.

    public virtual void Initialize(Unit caster)
    {
        unit = caster;
        unit.TakeDamageEvent += EffectActivate;
    }

    public abstract void EffectActivate(int dmg, ISkillCaster attacker, GameObject character);
}
