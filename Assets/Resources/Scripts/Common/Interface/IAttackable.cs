using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackable
{
    event Action<int> OnHitEvent;

    List<Skill_Module> ActiveSkills { get; set; }
    List<Skill_Module> ModifiedActiveSkills { get; set; }
    List<Skill_Module> PassiveSkills { get; set; }
    List<DamagedEventBase> HitEvents { get; set; }
    int Combo { get; set; }

    bool PerformAttack(Skill_Module skill);
    T FindSkill<T>(T skill) where T : SkillBase;
}
