using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FillFlameBlood", menuName = "ScriptableObject/Skills/Passive/YeonHaRyeong/FillFlameBlood")]
public class FillFlameBlood : OnHitEventBase
{
    private FlameBlood flameBlood;

    public override void Initialize(Unit caster)
    {
        base.Initialize(caster);
        if (flameBlood == null)
        {
            flameBlood = attack.FindSkill(flameBlood);
        }
    }

    public override void Execute(GameObject target, ISkillCaster caster, int dmg)
    {
        if (flameBlood.flameBloodGage >= 50) return;
        flameBlood.flameBloodGage += 1;
    }

}
