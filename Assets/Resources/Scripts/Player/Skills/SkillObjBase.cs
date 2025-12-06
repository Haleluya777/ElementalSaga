using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillObjBase : MonoBehaviour
{
    public abstract void ObjMovement();
    public virtual void ObjInit(Vector2 dir, int _dmg, string _tag, ISkillCaster _caster)
    {

    }
    public virtual void ObjInit(Vector2 dir, int _dmg, string _tag, ISkillCaster _caster, bool _reinforced)
    {

    }
}
