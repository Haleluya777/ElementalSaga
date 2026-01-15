using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Flow_A", menuName = "ScriptableObject/Skills/Active/YunSeoYeon/Flow_A")]
public class Flow_A : SkillBase
{
    private Unit unit;
    private Rigidbody2D rigid;

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

    [Header("승룡 거리")]
    [SerializeField] private float distance;

    public override bool UseSkill(ISkillCaster caster)
    {
        if (unit == null) unit = caster.GetCom<Unit>();
        if (rigid == null) rigid = caster.GetCom<Rigidbody2D>();

        if (!unit.FindEffect("Flow")) return false;

        var hitBoxOffset = unit.isAirial ? AirialhitBoxOffset : GroundhitBoxOffset;
        var hitBoxSize = unit.isAirial ? AirialhitBoxSize : GroundhitBoxSize;

        animName = unit.isAirial ? airialAnimName : groundAnimName;

        caster.PlayAnimation(animName);
        if (unit.isAirial)
        {
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

            GameManager.instance.coroutineRunner.StartCoroutine(DragonRising(caster, rigid));
        }
        else
        {
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
        }

        return true;
    }

    private IEnumerator DragonRising(ISkillCaster caster, Rigidbody2D rigid)
    {
        float dashSpeed = 5f;
        Vector2 target = new Vector2(rigid.position.x, rigid.position.y + distance);

        while (((Vector2)target - rigid.position).magnitude > .5f)
        {
            if (Physics2D.Raycast(new Vector2(caster.GetGameObject().transform.position.x, caster.GetGameObject().transform.position.y + .5f), Vector2.up * caster.GetGameObject().transform.localScale.x, .75f, 1 << 3))
            {
                break;
            }

            Vector2 direction = ((Vector2)target - rigid.position).normalized;
            Vector3 newPos = rigid.position + direction * dashSpeed * Time.fixedDeltaTime;

            rigid.MovePosition(newPos);

            yield return new WaitForFixedUpdate();
        }
    }
}
