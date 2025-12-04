using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFlow_Type_Through : SkillBase
{
    [SerializeField] private GameObject bullet;
    public override bool UseSkill(ISkillCaster caster)
    {
        if (bullet == null) return false;

        GameObject objInstance = Instantiate(bullet, caster.GetPosition(), caster.GetRotation());
        SkillObjBase objComponent = objInstance.GetComponent<SkillObjBase>();

        if (objComponent is not null)
        {
            int calculatedDamage = 0;
            if (dmgCalculater is not null)
            {
                calculatedDamage = dmgCalculater.Calculate(caster);
            }
            objComponent.ObjInit(caster.GetDirection(), calculatedDamage, caster.GetGameObject().tag, caster);
        }

        return true;
    }
}
