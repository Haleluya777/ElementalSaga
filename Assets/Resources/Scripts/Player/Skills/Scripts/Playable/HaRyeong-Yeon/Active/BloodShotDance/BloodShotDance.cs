using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BloodShotDance", menuName = "ScriptableObject/Skills/Active/YeonHaRyeong/BloodShotDance")]
public class BloodShotDance : SkillBase
{
    [SerializeField] private Skill_Module reinforcedSkill;
    [SerializeField] private Skill_Module basicSkill;
    private Unit unit;

    public override void Initialize(Skill_Module module)
    {
        base.Initialize(module);

        reinforcedSkill = Instantiate(reinforcedSkill);
        reinforcedSkill.InitSkill();
        basicSkill = Instantiate(basicSkill);
        basicSkill.InitSkill();
    }

    public override bool UseSkill(ISkillCaster caster)
    {
        if (unit == null) unit = caster.GetCom<Unit>();
        if (!unit.FindEffect("ExplosionBlood")) return false;

        unit.AddEffectProcess(new Buff_ExplosionBlood(8f, unit, 150, 70, "ExplosionBlood", basicSkill, reinforcedSkill));
        unit.AddEffectProcess(new Buff_KillDrain(8f, unit, 5f, "KillDrain"));

        return true;
    }
}
