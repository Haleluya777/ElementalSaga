using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "SlamDown_Dash", menuName = "ScriptableObject/Skills/Active/YeonHaRyeon/SlamDown_Dash")]
public class SlamDown_Dash : SkillBase
{
    [SerializeField] private SkillBase chainedSkill;
    [SerializeField] private float dashDistance = 5f; // 대쉬 거리

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

        while (((Vector2)target - rigid.position).magnitude > minSqrDistance)
        {
            Vector2 direction = ((Vector2)target - rigid.position).normalized;
            Vector3 newPos = rigid.position + direction * dashSpeed * Time.fixedDeltaTime;

            rigid.MovePosition(newPos);

            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForFixedUpdate();
        if (enemy != null)
        {
            var enemyUnit = enemy.GetComponent<Unit>();
            enemyUnit.AddEffectProcess(new DeBuff_Catched(1.5f, enemyUnit, 0, "Catchted", caster.GetGameObject()));
            //enemyUnit.AddEffectProcess(new DeBuff_Throwed(100f, enemyUnit, 0, "Throwed", caster.GetGameObject(), 5f));
            chainedSkill.UseSkill(caster);
        }
        yield return null;
    }
}
