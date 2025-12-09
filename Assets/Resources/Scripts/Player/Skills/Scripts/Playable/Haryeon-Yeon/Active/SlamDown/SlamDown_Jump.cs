using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SlamDown_Jump", menuName = "ScriptableObject/Skills/Active/YeonHaRyeon/SlamDown_Jump")]
public class SlamDown_Jump : SkillBase
{
    [SerializeField] private SkillBase explosion;

    [SerializeField] private float distance;
    [SerializeField] private float jumpDuration; //공중 체공 시간.
    [SerializeField] private float defaultDuration; //목표가 정해지지 않을 경우 점프 거리

    private Rigidbody2D rigid;
    private RaycastHit2D enemyHitted;
    public override bool UseSkill(ISkillCaster caster)
    {
        Vector2 target;
        rigid = caster.GetCom<Rigidbody2D>();
        enemyHitted = Physics2D.Raycast(new Vector2(caster.GetPosition().x, caster.GetPosition().y + .5f), caster.GetDirection(), distance, 1 << 6);

        if (enemyHitted.collider != null) //범위 내 적이 존재할 경우.
        {
            target = new Vector2(enemyHitted.point.x, caster.GetGameObject().transform.position.y);
        }
        else //아닐 경우.
        {
            Debug.Log("목표 없음");
            target = caster.GetGameObject().transform.position + new Vector3(2f, 0);
        }

        Vector2 startPos = caster.GetGameObject().transform.position;
        float t = jumpDuration;
        Vector2 requiredVelocity = (target - startPos) / t - .5f * Physics2D.gravity * t;

        rigid.velocity = requiredVelocity;
        GameManager.instance.coroutineRunner.StartCoroutine(CheckGround(caster));
        return true;
    }

    private IEnumerator CheckGround(ISkillCaster caster)
    {
        RaycastHit2D hit;

        while (true)
        {
            hit = Physics2D.Raycast(caster.GetGameObject().transform.position, Vector2.down, 0.1f, 1 << 3);
            if (hit.collider == null)
            {
                break;
            }
            yield return new WaitForFixedUpdate();
        }

        while (true)
        {
            hit = Physics2D.Raycast(caster.GetGameObject().transform.position, Vector2.down, 0.1f, 1 << 3);
            if (hit.collider != null)
            {
                targetObj.GetComponentInChildren<IDamageable>().TakeDamage(dmgCalculater.Calculate(caster), caster, targetObj);
                var unit = targetObj.GetComponent<Unit>();
                if (unit.activeEffect.TryGetValue("Catchted", out StatusEffectBase exisitngEffect))
                {
                    if (unit.activeEffectCoroutines.TryGetValue("Catchted", out Coroutine runningCoroutine))
                    {
                        //코루틴을 이용한 상태이상의 타이머 제거.
                        GameManager.instance.coroutineRunner.StopCoroutine(runningCoroutine);
                    }
                    //적용 되어 있는 상태이상 또한 제거.
                    exisitngEffect.RemoveEffect();
                }
                explosion.UseSkill(caster);
                break;
            }
            yield return new WaitForFixedUpdate();
        }
    }
}
