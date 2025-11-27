using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillBase : ScriptableObject
{
    public Skill_Module parentModule { get; private set; }
    public DmgCalculator dmgCalculater;
    public string explanation;

    public virtual void Initialize(Skill_Module module)
    {
        parentModule = module;
    }

    public abstract bool UseSkill(ISkillCaster caster);
}
