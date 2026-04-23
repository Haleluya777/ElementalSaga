using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FireSlashJump", menuName = "ScriptableObject/Skills/Active/Guren_Kagari/FireSlashJump")]
public class FireSlashJump : SkillBase
{
    public override bool UseSkill(ISkillCaster caster)
    {


        return true;
    }

}
