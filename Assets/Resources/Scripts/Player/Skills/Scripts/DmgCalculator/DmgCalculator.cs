using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "DmgCalculator", menuName = "ScriptableObject/DamageCalculator/DmgCalculator")]
public class DmgCalculator : ScriptableObject
{
    public int baseDmg;
    public float weight;

    public int Calculate(ISkillCaster caster)
    {
        return (caster.GetAttackPower() + (int)(caster.GetAttackPower() * weight)) + (int)((caster.GetAttackPower() + (int)(caster.GetAttackPower() * weight)) * (caster.GetGiveDmgRate() / 100f));
    }
}
