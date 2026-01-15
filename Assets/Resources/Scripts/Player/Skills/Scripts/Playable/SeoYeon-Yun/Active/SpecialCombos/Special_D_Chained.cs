using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Special_D_Chained", menuName = "ScriptableObject/Skills/Active/YunSeoYeon/Special_D_Chained")]
public class Special_D_Chained : SkillBase
{
    [Header("지상에서 스킬을 사용할 때의 히트박스 위치와 크기")]
    [SerializeField] private Vector2 GroundhitBoxOffset;
    [SerializeField] private Vector2 GroundhitBoxSize;

    [Header("공중 체공 시간")]
    [SerializeField] private float duration;

    private Unit unit;
    private Rigidbody2D rigid;

    public override bool UseSkill(ISkillCaster caster)
    {
        if (unit == null) unit = caster.GetCom<Unit>();
        if (rigid == null) rigid = caster.GetCom<Rigidbody2D>();

        var hitBoxOffset = GroundhitBoxOffset;
        var hitBoxSize = GroundhitBoxSize;
        //var animName = groundAnimName;

        //parentModule.AnimName = animName;

        GameObject hitBox = GameManager.instance.objectPoolManager.poolDic["HitBox"].GetGo("HitBox");
        GameObject effectObj = GameManager.instance.objectPoolManager.poolDic["Effect"].GetGo("Effect");

        hitBox.transform.position = caster.GetHitBoxPos().position;
        effectObj.transform.position = caster.GetHitBoxPos().position;

        hitBox.tag = caster.GetGameObject().tag;

        HitBox hitBoxCom = hitBox.GetComponent<HitBox>();
        Effect effectCom = effectObj.GetComponent<Effect>();

        hitBoxCom.GetComponent<BoxCollider2D>().size = hitBoxSize;
        hitBoxCom.GetComponent<BoxCollider2D>().offset = hitBoxOffset;

        hitBoxCom.Initialize(dmgCalculater.Calculate(caster), caster, null, .5f);
        effectCom.Initialize(.5f);

        if (unit.isAirial)
        {
            GameManager.instance.coroutineRunner.StartCoroutine(HangTime(caster, rigid));
            parentModule.blackBoard.Set<bool>("Condition", false);
        }
        caster.PlayAnimation(animName);
        return true;
    }

    private IEnumerator HangTime(ISkillCaster caster, Rigidbody2D rigid)
    {
        float timer = 0f;
        var previousVelocity = rigid.velocity;
        while (timer < duration)
        {
            rigid.velocity = new Vector2(0f, 1f);
            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        //rigid.velocity = previousVelocity;
    }
}
