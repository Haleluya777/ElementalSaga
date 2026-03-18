using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillObjBase : PoolAble
{
    public abstract void ObjMovement();
    public virtual void ObjInit(Vector2 dir, int _dmg, int _stunDmg, string _tag, ISkillCaster _caster)
    {

    }
    public virtual void ObjInit(Vector2 dir, int _dmg, int _stunDmg, string _tag, ISkillCaster _caster, bool _reinforced)
    {
        ObjInit(dir, _dmg, _stunDmg, _tag, _caster);
    }

    public virtual void ObjInit(Vector2 dir, int _dmg, int _stunDmg, string _tag, ISkillCaster _caster, bool _reinforced, SkillBase parentSkill)
    {
        ObjInit(dir, _dmg, _stunDmg, _tag, _caster, _reinforced);
    }
}
