using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FirePunch", menuName = "ScriptableObject/Skills/FirePunch")]
public class FirePunch : SkillBase
{
    [SerializeField] private GameObject throwableObj;

    public override bool UseSkill(ISkillCaster caster)
    {
        if (throwableObj == null) return false;

        GameObject objInstance = Instantiate(throwableObj, caster.GetPosition(), caster.GetRotation());

        SkillObjBase objComponent = objInstance.GetComponent<SkillObjBase>();
        if (objComponent is not null)
        {
            int calculatedDamage = 0;
            if (dmgCalculater is not null)
            {
                calculatedDamage = dmgCalculater.Calculate(caster);
            }
            objComponent.ObjInit(caster.GetDirection(), calculatedDamage, stunDmg, caster.GetGameObject().tag, caster);
        }

        caster.Attacking = false;
        return true;
    }
}
