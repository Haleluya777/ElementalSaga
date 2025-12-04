using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SlamDown_Punch_Explosion", menuName = "ScriptableObject/Skills/Active/YeonHaRyeon/SlamDown_Punch_Explosion")]
public class SlamDown_Punch_Explosion : SkillBase
{
    [SerializeField] private Vector2 hitBoxOffset;
    [SerializeField] private Vector2 hitBoxSize;

    public override bool UseSkill(ISkillCaster caster)
    {
        Debug.Log("대폭발!");

        GameObject hitBox = GameManager.instance.objectPoolManager.GetGo("HitBox");
        GameObject effectObj = GameManager.instance.objectPoolManager.GetGo("Effect");

        hitBox.transform.position = caster.GetHitBoxPos().position;
        effectObj.transform.position = caster.GetHitBoxPos().position;

        hitBox.tag = caster.GetGameObject().tag;

        HitBox hitBoxCom = hitBox.GetComponent<HitBox>();
        Effect effectCom = effectObj.GetComponent<Effect>();

        hitBoxCom.GetComponent<BoxCollider2D>().size = hitBoxSize;
        hitBoxCom.GetComponent<BoxCollider2D>().offset = hitBoxOffset;

        hitBoxCom.Initialize(dmgCalculater.Calculate(caster), caster, null, .5f);
        effectCom.Initialize(.5f);

        return true;
    }
}
