using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Flow", menuName = "ScriptableObject/Skills/Passive/YunSeoYeon/Flow")]
public class Flow : SkillBase
{
    private Unit unit;
    private IAttackable attack;

    [SerializeField] private List<Skill_Module> flowSkills = new List<Skill_Module>();
    [SerializeField] private List<Skill_Module> basicSkills = new List<Skill_Module>();

    public override void Initialize(Skill_Module module)
    {
        base.Initialize(module);
        flowSkills = SkillInit(flowSkills);
        basicSkills = SkillInit(basicSkills);
    }

    public override bool UseSkill(ISkillCaster caster)
    {
        if (unit == null) unit = caster.GetCom<Unit>();
        if (attack == null) attack = caster.GetCom<IAttackable>();

        if (!unit.FindEffect("Flow"))
        {
            if (attack.ActiveSkills != basicSkills)
            {
                attack.ActiveSkills = basicSkills;
            }
            else return true;
        }
        else
        {
            if (attack.ActiveSkills != flowSkills)
            {
                attack.ActiveSkills = flowSkills;
            }
            else return true;
        }

        return true;
    }

    private List<T> SkillInit<T>(List<T> skills) where T : class
    {
        for (int i = 0; i < skills.Count; i++)
        {
            if (skills[i] == null) continue;

            var original = skills[i] as UnityEngine.Object;
            if (original == null) continue;

            var newInstance = Instantiate(original);

            if (newInstance is DamagedEventBase damagedEvent)
            {
                damagedEvent.Initialize(unit);
            }
            else if (newInstance is Skill_Module skillModule)
            {
                skillModule.InitSkill();
            }

            skills[i] = newInstance as T;
        }
        return skills;
    }
}
