using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Purgatory_Charge", menuName = "ScriptableObject/Skills/Active/YeonHaRyeon/Purgatory_Charge")]
public class Purgatory_Charge : SkillBase
{
    private float chargeGage;

    public override bool UseSkill(ISkillCaster caster)
    {
        if (chargeGage < 5) chargeGage += Time.deltaTime * 10;
        Debug.Log($"차징 중... {chargeGage}%");
        parentModule.ReleaseSkills[0].dmgCalculater.weight += chargeGage;
        return true;
    }
}
