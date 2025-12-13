using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SuperPressureBlowout_Explosion", menuName = "ScriptableObject/Skills/Active/YeonHaRyeon/SuperPressureBlowout_Explosion")]
public class SuperPressureBlowout_Explosion : SkillBase
{
    private Vector2 hitBoxOffset;
    private float hitBoxRadius;
    private GameObject hitBoxObj;

    public override bool UseSkill(ISkillCaster caster)
    {
        //오브젝트풀에서 히트박스와 이펙트 오브젝트 가져옴.
        GameObject hitBoxObj = GameManager.instance.objectPoolManager.GetGo("HitBox_Circle");
        GameObject effectObj = GameManager.instance.objectPoolManager.GetGo("Effect");

        //위치 조정.
        hitBoxObj.transform.position = caster.GetPosition();
        effectObj.transform.position = caster.GetPosition();

        //각각 HitBox와 Effect 스크립트 가져옴 (HitBox와 Effect 스크립트의 Initialize 메서드를 실행시키기 위함.)
        HitBox hitBoxCom = hitBoxObj.GetComponent<HitBox>();
        Effect effectCom = effectObj.GetComponent<Effect>();

        //히트박스 오브젝트의 태그 변경 (피아식별을 위함.)
        hitBoxObj.tag = caster.GetGameObject().tag;

        //히트박스의 크기 및 위치 조정.
        var hitBox = hitBoxObj.GetComponent<CircleCollider2D>();
        hitBox.radius = hitBoxRadius;
        hitBox.offset = hitBoxOffset;


        hitBoxCom.Initialize(dmgCalculater.Calculate(caster), caster, null, .5f);
        effectCom.Initialize(.5f);

        return true;
    }

    public override void HitBoxInit(float radius, Vector2 offset)
    {
        hitBoxRadius = radius;
        hitBoxOffset = offset;
    }
}
