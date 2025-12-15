using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Special_B", menuName = "ScriptableObject/Skills/Active/YunSeoYeon/Special_B")]
public class Special_B : SkillBase
{
    [SerializeField] private float dashDistance;
    private Unit unit;
    public override bool UseSkill(ISkillCaster caster)
    {
        unit = caster.GetCom<Unit>();
        if (unit.isAirial)
        {
            parentModule.blackBoard.Set<bool>("Condition", false);
        }
        GameManager.instance.coroutineRunner.StartCoroutine(Dash(caster, unit.GetComponent<Rigidbody2D>()));
        unit.AddEffectProcess(new Buff_Flow(.3f, unit, "Flow"));
        return true;
    }

    private IEnumerator Dash(ISkillCaster caster, Rigidbody2D rigid)
    {
        Transform casterTransform = caster.GetGameObject().transform;
        float directionf = Mathf.Sign(casterTransform.localScale.x);

        float targetX = casterTransform.position.x + (directionf * dashDistance);

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
