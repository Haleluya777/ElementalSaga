using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Basic_A", menuName = "ScriptableObject/Skills/Active/YunSeoYeon/Basic_A")]
public class Water_Basic_A : SkillBase
{
    [Header("지상에서 스킬을 사용할 때의 히트박스 위치와 크기")]
    [SerializeField] private Vector2 GroundhitBoxOffset;
    [SerializeField] private Vector2 GroundhitBoxSize;

    [Header("공중에서 스킬을 사용할 때의 히트박스 위치와 크기")]
    [SerializeField] private Vector2 AirialhitBoxOffset;
    [SerializeField] private Vector2 AirialhitBoxSize;

    [Header("지상/공중에서 스킬을 시전할 때 실행할 애니메이션 클립 이름")]
    [SerializeField] string groundAnimName;
    [SerializeField] string airialAnimName;

    [Header("공중 체공 시간")]
    [SerializeField] private float duration;

    private Unit unit;
    private Rigidbody2D rigid;

    public override bool UseSkill(ISkillCaster caster)
    {
        if (unit == null) unit = caster.GetCom<Unit>();
        if (rigid == null) rigid = caster.GetCom<Rigidbody2D>();

        var hitBoxOffset = unit.isAirial ? AirialhitBoxOffset : GroundhitBoxOffset;
        var hitBoxSize = unit.isAirial ? AirialhitBoxSize : GroundhitBoxSize;

        animName = unit.isAirial ? airialAnimName : groundAnimName;

        caster.PlayAnimation(animName);

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
            Debug.Log("공중에서 사용!");
            GameManager.instance.coroutineRunner.StartCoroutine(HangTime(caster, rigid));
            parentModule.blackBoard.Set<bool>("Condition", false);
        }

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
