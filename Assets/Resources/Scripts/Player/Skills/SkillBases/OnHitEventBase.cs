using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OnHitEventBase : ScriptableObject
{
    protected IAttackable attack; //이 스킬 효과를 시전하는 유닛.

    public virtual void Initialize(Unit caster)
    {
        attack = caster.GetComponentInChildren<IAttackable>();
    }

    public abstract void Execute(GameObject target, ISkillCaster caster, int dmg);
}
