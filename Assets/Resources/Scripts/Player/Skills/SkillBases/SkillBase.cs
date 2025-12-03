using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillBase : ScriptableObject
{
    public Skill_Module parentModule { get; private set; }
    public DmgCalculator dmgCalculater;
    public GameObject targetObj; //타게팅 스킬에 사용
    public string explanation;

    public virtual void Initialize(Skill_Module module)
    {
        parentModule = module;
    }

    public virtual void HitBoxInit(Vector2 hitBoxSize, Vector2 hitBoxOffSet)
    {

    }

    public abstract bool UseSkill(ISkillCaster caster);
}
