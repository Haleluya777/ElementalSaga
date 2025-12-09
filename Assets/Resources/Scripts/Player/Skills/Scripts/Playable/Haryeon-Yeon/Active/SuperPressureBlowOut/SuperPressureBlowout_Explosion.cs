using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SuperPressureBlowout_Explosion", menuName = "ScriptableObject/Skills/Active/YeonHaRyeon/SuperPressureBlowout_Explosion")]
public class SuperPressureBlowout_Explosion : SkillBase
{
    private Vector2 hitBoxOffset;
    private Vector2 hitBoxSize;
    private GameObject hitBoxObj;

    public override bool UseSkill(ISkillCaster caster)
    {
        GameObject hitBox = GameManager.instance.objectPoolManager.GetGo("HitBox");
        GameObject effectObj = GameManager.instance.objectPoolManager.GetGo("Effect");

        hitBox.transform.position = caster.GetPosition();
        effectObj.transform.position = caster.GetPosition();

        HitBox hitBoxCom = hitBox.GetComponent<HitBox>();
        Effect effectCom = effectObj.GetComponent<Effect>();

        hitBox.tag = caster.GetGameObject().tag;

        hitBoxCom.GetComponent<BoxCollider2D>().size = hitBoxSize;
        hitBoxCom.GetComponent<BoxCollider2D>().offset = hitBoxOffset;

        hitBoxCom.Initialize(dmgCalculater.Calculate(caster), caster, null, .5f);
        effectCom.Initialize(.5f);

        return true;
    }

    public override void HitBoxInit(Vector2 size, Vector2 offset)
    {
        hitBoxSize = size;
        hitBoxOffset = offset;
    }
}
