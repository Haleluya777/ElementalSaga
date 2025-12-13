using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "TakeDown", menuName = "ScriptableObject/Skills/Passive/YunSeoYeon/TakeDown")]
public class TakeDown : OnHitEventBase
{
    public override void Execute(GameObject target, ISkillCaster caster, int dmg)
    {
        var targetUnit = target.GetComponent<Unit>();
        targetUnit.AddEffectProcess(new DeBuff_Throwed(100f, targetUnit, 0, "Throwed", caster, 5f, null, new Vector2(.25f * caster.GetDirection().x, -1)));
    }
}
