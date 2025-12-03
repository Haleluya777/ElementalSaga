using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "SlamDown_Dash", menuName = "ScriptableObject/Skills/Active/YeonHaRyeon/SlamDown_Dash")]
public class SlamDown_Dash : SkillBase
{
    [SerializeField] private SkillBase chainedSkill_Jump;
    [SerializeField] private SkillBase chainedSkill_FirePunch;
    [SerializeField] private HeatPressure heatPressurePassive;

    private float dashDistance; // 대쉬 거리

    public override bool UseSkill(ISkillCaster caster)
    {
        Transform casterTransform = caster.GetGameObject().transform;
        RaycastHit2D enemyHitted = Physics2D.Raycast(new Vector2(caster.GetPosition().x, caster.GetPosition().y + .5f), caster.GetDirection(), dashDistance, 1 << 6);
        GameObject enemy = null;

        //돌진 범위 설정 (50미만 : 3f, 50이상 100미만 : 1.5f, 100 : 0, 돌진없음)
        dashDistance = heatPressurePassive.heatPressure < 50 ? 3f : (heatPressurePassive.heatPressure < 100 ? 1.5f : 0);

        float direction = Mathf.Sign(casterTransform.localScale.x);
        float targetX = casterTransform.position.x + (direction * dashDistance);

        Vector2 target = Vector2.zero;
        if (enemyHitted.collider != null) //돌진 범위 내에 적이 존재할 때.
        {
            //Debug.DrawRay(caster.GetPosition(), caster.GetDirection(), Color.red, 100, true);
            target = new Vector2(enemyHitted.point.x, caster.GetGameObject().transform.position.y);
            enemy = enemyHitted.collider.gameObject;
        }

        else //돌진 범위 내에 적이 없을 때.
        {
            target = new Vector2(targetX, caster.GetGameObject().transform.position.y);
        }

        GameManager.instance.coroutineRunner.StartRunnerCoroutine(PerformDash(caster, caster.GetCom<Rigidbody2D>(), casterTransform, target, enemy));
        return true;
    }

    private IEnumerator PerformDash(ISkillCaster caster, Rigidbody2D rigid, Transform casterTransform, Vector3 target, GameObject enemy)
    {
        float dashSpeed = 50f; // 대쉬 속도
        float minSqrDistance = .5f;

        while (((Vector2)target - rigid.position).magnitude > minSqrDistance) //목적지 까지 돌진.
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

        yield return new WaitForFixedUpdate(); //돌진 이후 다음 프레임까지 살짝 대기.

        if (heatPressurePassive.heatPressure == 100) //열압이 100일 때, 파이어 펀치 실행.
        {
            chainedSkill_FirePunch.UseSkill(caster);
            yield return null;
        }

        else
        {
            if (enemy != null) //Enemy가 비어 있지 않은 경우. (스킬을 시전할 때, 범위 내에 Enemy가 존재하는 경우.)
            {
                var enemyUnit = enemy.GetComponent<Unit>();
                enemyUnit.AddEffectProcess(new DeBuff_Catched(1.5f, enemyUnit, 0, "Catchted", caster.GetGameObject()));
                chainedSkill_Jump.targetObj = enemy;
                chainedSkill_Jump.UseSkill(caster);
            }
        }

        yield return null;
    }
}
