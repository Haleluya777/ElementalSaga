using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SlamDown_Explosion", menuName = "ScriptableObject/Skills/Active/YeonHaRyeon/SlamDown_Explosion")]
public class SlamDown_Explosion : SkillBase
{
    [SerializeField] private Vector2 hitBoxOffset;
    [SerializeField] private Vector2 hitBoxSize;
    [SerializeField] private HeatPressure heatPressurePassive;

    public override bool UseSkill(ISkillCaster caster)
    {
        if (heatPressurePassive.heatPressure > 50 && heatPressurePassive.heatPressure < 100)
        {
            hitBoxSize += hitBoxSize * .4f;
        }

        GameObject hitBox = GameManager.instance.objectPoolManager.GetGo("HitBox");
        GameObject effectObj = GameManager.instance.objectPoolManager.GetGo("Effect");

        hitBox.transform.position = caster.GetHitBoxPos().position;
        effectObj.transform.position = caster.GetHitBoxPos().position;

        HitBox hitBoxCom = hitBox.GetComponent<HitBox>();
        Effect effectCom = effectObj.GetComponent<Effect>();

        hitBox.tag = caster.GetGameObject().tag;

        hitBoxCom.GetComponent<BoxCollider2D>().size = hitBoxSize;
        hitBoxCom.GetComponent<BoxCollider2D>().offset = hitBoxOffset;

        hitBoxCom.Initialize(dmgCalculater.Calculate(caster), caster, null, .5f);
        effectCom.Initialize(.5f);

        return true;
    }
}
