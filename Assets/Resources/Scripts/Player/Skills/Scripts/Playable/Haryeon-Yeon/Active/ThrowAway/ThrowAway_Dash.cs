using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "ThrowAway_Dash", menuName = "ScriptableObject/Skills/Active/YeonHaRyeon/ThrowAway_Dash")]
public class ThrowAway_Dash : SkillBase
{
    [SerializeField] private SkillBase chainedSkill;
    [SerializeField] private float dashDistance = 2f; // 대쉬 거리

    [SerializeField] private string secondDashAnim; //두번째 살짝 대쉬 할 때 사용할 애니메이션.
    [SerializeField] private string throwAnim; //던지기 애니메이션.

    public override bool UseSkill(ISkillCaster caster)
    {
        //Debug.Log("돌진");
        Transform casterTransform = caster.GetGameObject().transform;
        //RaycastHit2D enemyHitted = Physics2D.Raycast(new Vector2(caster.GetPosition().x, caster.GetPosition().y + .5f), caster.GetDirection(), dashDistance, 1 << 6);
        RaycastHit2D enemyHitted = Physics2D.Raycast(caster.GetPosition(), caster.GetDirection(), dashDistance, 1 << 6);
        GameObject enemy = null;

        float direction = Mathf.Sign(casterTransform.localScale.x);
        float targetX = casterTransform.position.x + (direction * dashDistance);

        Vector2 target = Vector2.zero;
        Vector2 target_Throw = Vector2.zero;

        if (enemyHitted.collider != null) //돌진 범위 내에 적이 존재할 때.
        {
            target = new Vector2(enemyHitted.point.x, caster.GetPosition().y);
            target_Throw = new Vector2(target.x + (1f * (target - caster.GetPosition()).normalized.x), caster.GetPosition().y);
            Debug.Log(target_Throw);
            enemy = enemyHitted.collider.gameObject;
        }

        else //돌진 범위 내에 적이 없을 때.
        {
            target = new Vector2(targetX, caster.GetPosition().y);
        }

        GameManager.instance.coroutineRunner.StartRunnerCoroutine(PerformDash(caster, caster.GetCom<Rigidbody2D>(), casterTransform, target, target_Throw, enemy));
        return true;
    }

    private IEnumerator PerformDash(ISkillCaster caster, Rigidbody2D rigid, Transform casterTransform, Vector3 target, Vector2 target_Throw, GameObject enemy)
    {
        Unit enemyUnit;

        float dashSpeed = 25f; // 대쉬 속도
        float minSqrDistance = .5f;

        //스킬 순서 = 목적지로 대쉬 => 앞으로 살짝 전진 => Throwed디버프 부여.

        //목적지로 대쉬하는 부분.
        caster.PlayAnimation(animName);
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

        if (enemy != null)
        {
            enemyUnit = enemy.GetComponent<Unit>();
            //Enemy를 잡은 뒤 두 번째 대쉬 애니메이션 실행.
            enemyUnit.AddEffectProcess(new DeBuff_Catched(100f, enemyUnit, 0, "Catched", caster.GetGameObject(), caster.GetCatchPos()));
            caster.PlayAnimation(secondDashAnim);

            //두 번째 대쉬 실행.
            while (((Vector2)target_Throw - rigid.position).magnitude > minSqrDistance)
            {
                if (Physics2D.Raycast(new Vector2(caster.GetGameObject().transform.position.x, caster.GetGameObject().transform.position.y + .5f), Vector2.right * caster.GetGameObject().transform.localScale.x, .75f, 1 << 3))
                {
                    break;
                }

                Vector2 direction = ((Vector2)target_Throw - rigid.position).normalized;
                Vector3 newPos = rigid.position + direction * (dashSpeed / 10) * Time.fixedDeltaTime;

                rigid.MovePosition(newPos);

                yield return new WaitForFixedUpdate();
            }
            //두 번째 대쉬가 끝난 뒤 한프레임 쉬고 던지기 애니메이션 실행.
            yield return null;
            caster.PlayAnimation(throwAnim);
            yield return new WaitForSeconds(6f * (1f / 30f));

            if (enemyUnit.activeEffect.TryGetValue("Catched", out StatusEffectBase exisitngEffect))
            {
                if (enemyUnit.activeEffectCoroutines.TryGetValue("Catched", out Coroutine runningCoroutine))
                {
                    //코루틴을 이용한 상태이상의 타이머 제거.
                    GameManager.instance.coroutineRunner.StopCoroutine(runningCoroutine);
                }
                //적용 되어 있는 상태이상 또한 제거.
                exisitngEffect.RemoveEffect();
            }
            enemyUnit.AddEffectProcess(new DeBuff_Throwed(100f, enemyUnit, 0, "Throwed", caster, 5f, chainedSkill, caster.GetDirection()));
        }

        yield return null;
    }
}
