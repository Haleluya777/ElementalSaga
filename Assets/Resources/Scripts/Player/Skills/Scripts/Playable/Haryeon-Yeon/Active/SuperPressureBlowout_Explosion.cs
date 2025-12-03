using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SuperPressureBlowout_Explosion", menuName = "ScriptableObject/Skills/Active/YeonHaRyeon/SuperPressureBlowout_Explosion")]
public class SuperPressureBlowout_Explosion : SkillBase
{
    [SerializeField] private Vector2 hitBoxOffset;
    [SerializeField] private Vector2 hitBoxSize;
    private GameObject hitBoxObj;

    public override bool UseSkill(ISkillCaster caster)
    {

        return true;
    }

    public override void HitBoxInit(Vector2 size, Vector2 offset)
    {
        hitBoxSize = size;
        hitBoxOffset = offset;
    }
}
