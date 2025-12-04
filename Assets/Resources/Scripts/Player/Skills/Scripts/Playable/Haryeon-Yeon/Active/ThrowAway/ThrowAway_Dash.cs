using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "ThrowAway_Dash", menuName = "ScriptableObject/Skills/Active/YeonHaRyeon/ThrowAway_Dash")]
public class ThrowAway_Dash : SkillBase
{
    [SerializeField] private SkillBase chainedSkill;
    [SerializeField] private float dashDistance = 2f; // 대쉬 거리

    public override bool UseSkill(ISkillCaster caster)
    {
        Transform casterTransform = caster.GetGameObject().transform;
        RaycastHit2D enemyHitted = Physics2D.Raycast(new Vector2(caster.GetPosition().x, caster.GetPosition().y + .5f), caster.GetDirection(), dashDistance, 1 << 6);
        GameObject enemy = null;

        float direction = Mathf.Sign(casterTransform.localScale.x);
        float targetX = casterTransform.position.x + (direction * dashDistance);

        Vector2 target = Vector2.zero;
        if (enemyHitted.collider != null) //돌진 범위 내에 적이 존재할 때.
        {
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
        float dashSpeed = 25f; // 대쉬 속도
        float minSqrDistance = .5f;

        while (((Vector2)target - rigid.position).magnitude > minSqrDistance)
        {
            if (Physics2D.Raycast(new Vector2(caster.GetGameObject().transform.position.x, caster.GetGameObject().transform.position.y + .5f), Vector2.right * caster.GetGameObject().transform.localScale.x, .75f, 1 << 3))
            {
                Debug.Log("대쉬취소");
                break;
            }

            Vector2 direction = ((Vector2)target - rigid.position).normalized;
            Vector3 newPos = rigid.position + direction * dashSpeed * Time.fixedDeltaTime;

            rigid.MovePosition(newPos);

            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(.5f);
        if (enemy != null)
        {
            var enemyUnit = enemy.GetComponent<Unit>();
            enemyUnit.AddEffectProcess(new DeBuff_Throwed(100f, enemyUnit, 0, "Throwed", caster, 5f, chainedSkill));
        }
        yield return null;
    }
}
