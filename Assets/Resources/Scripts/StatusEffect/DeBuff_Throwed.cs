using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DeBuff_Throwed : StatusEffectBase
{
    private SkillBase chainedSkill;
    private float distance;
    private Vector2 targetPos;
    private Rigidbody2D rigid;
    private ISkillCaster attacker;

    public DeBuff_Throwed(float duration, Unit target, int _rate, string _effectName, ISkillCaster _attacker, float _distance, SkillBase _chainedSkill) : base(duration, target)
    {
        effectName = _effectName;
        distance = _distance;
        attacker = _attacker;
        chainedSkill = _chainedSkill;
    }

    public override void ApplyEffect()
    {
        Transform unitTransform = this.target.gameObject.transform;
        rigid = this.target.GetComponent<Rigidbody2D>();

        float direction = Mathf.Sign(unitTransform.localScale.x);
        float targetX = unitTransform.position.x + (direction * distance);

        targetPos = new Vector2(targetX, unitTransform.position.y);

        GameManager.instance.coroutineRunner.StartCoroutine(Throwing(unitTransform.position));
    }

    public override void RemoveEffect()
    {
        Debug.Log("다 던져짐");
    }

    private IEnumerator Throwing(Vector2 origin)
    {
        BoxCollider2D col = this.target.GetComponentInChildren<BoxCollider2D>();
        Debug.Log(col.size.x / 2 + .1f);
        RaycastHit2D hitted;
        float dashSpeed = 50f; // 대쉬 속도
        float minSqrDistance = .5f;

        while (true)
        {
            hitted = Physics2D.Raycast(this.target.transform.position + new Vector3(col.size.x / 2 + .1f, .5f), targetPos - origin, (col.size.x / 2) + .115f);

            if (hitted.collider != null)
            {
                Debug.Log("부딪힘");
                chainedSkill.UseSkill(attacker, target.transform.position);
                break;
            }

            if ((targetPos - rigid.position).magnitude < minSqrDistance) break;

            Vector2 direction = (targetPos - rigid.position).normalized;
            Vector2 newPos = rigid.position + direction * dashSpeed * Time.fixedDeltaTime;

            rigid.MovePosition(newPos);

            yield return new WaitForFixedUpdate();
        }
        RemoveEffect();
        yield return null;
    }
}
