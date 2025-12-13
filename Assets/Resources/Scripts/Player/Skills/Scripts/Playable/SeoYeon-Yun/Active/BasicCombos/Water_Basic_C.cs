using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Basic_C", menuName = "ScriptableObject/Skills/Active/YunSeoYeon/Basic_C")]
public class Water_Basic_C : SkillBase
{
    [SerializeField] private Vector2 GroundhitBoxOffset;
    [SerializeField] private Vector2 GroundhitBoxSize;

    [SerializeField] private Vector2 AirialhitBoxOffset;
    [SerializeField] private Vector2 AirialhitBoxSize;

    [SerializeField] string groundAnimName;
    [SerializeField] string airialAnimName;
    [SerializeField] private float duration;

    private Unit unit;
    private Rigidbody2D rigid;

    public override bool UseSkill(ISkillCaster caster)
    {
        if (unit == null) unit = caster.GetCom<Unit>();
        if (rigid == null) rigid = caster.GetCom<Rigidbody2D>();

        var hitBoxOffset = unit.isAirial ? AirialhitBoxOffset : GroundhitBoxOffset;
        var hitBoxSize = unit.isAirial ? AirialhitBoxSize : GroundhitBoxSize;
        var animName = unit.isAirial ? airialAnimName : groundAnimName;

        parentModule.AnimName = animName;

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

        if (unit.isAirial)
        {
            Debug.Log("공중에서 사용함");
            GameManager.instance.coroutineRunner.StartCoroutine(Dash(caster, rigid));
            parentModule.blackBoard.Set<bool>("Condition", false);
        }

        return true;
    }

    private IEnumerator Dash(ISkillCaster caster, Rigidbody2D rigid)
    {
        Transform casterTransform = caster.GetGameObject().transform;
        float directionf = Mathf.Sign(casterTransform.localScale.x);

        float targetX = casterTransform.position.x + (directionf * 1f);

        Vector3 target = new Vector3(targetX, caster.GetGameObject().transform.position.y, caster.GetGameObject().transform.position.z);

        float dashSpeed = 50f; // 대쉬 속도
        float minSqrDistance = .5f;

        while (((Vector2)target - rigid.position).magnitude > minSqrDistance)
        {
            if (Physics2D.Raycast(new Vector2(caster.GetGameObject().transform.position.x, caster.GetGameObject().transform.position.y + .5f), Vector2.right * caster.GetGameObject().transform.localScale.x, .75f, 1 << 3))
            {
                break;
            }

            Vector2 direction = ((Vector2)target - rigid.position).normalized;
            Vector3 newPos = rigid.position + direction * dashSpeed * Time.fixedDeltaTime;

            rigid.MovePosition(newPos);

            yield return new WaitForFixedUpdate();
        }
        yield return null;
    }
}
