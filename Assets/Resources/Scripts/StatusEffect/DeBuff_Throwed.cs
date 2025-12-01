using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DeBuff_Throwed : StatusEffectBase
{
    private float distance;
    private Vector2 targetPos;
    private Rigidbody2D rigid;

    public DeBuff_Throwed(float duration, Unit target, int _rate, string _effectName, GameObject _attacker, float _distance) : base(duration, target)
    {
        effectName = _effectName;
        distance = _distance;
    }

    public override void ApplyEffect()
    {
        Transform unitTransform = this.target.gameObject.transform;
        rigid = this.target.GetComponent<Rigidbody2D>();

        float direction = Mathf.Sign(unitTransform.localScale.x);
        float targetX = unitTransform.position.x + (direction * distance);

        targetPos = new Vector2(targetX, unitTransform.position.y);

        GameManager.instance.coroutineRunner.StartCoroutine(Throwing());
    }

    public override void RemoveEffect()
    {
        Debug.Log("다 던져짐");
    }

    private IEnumerator Throwing()
    {
        float dashSpeed = 50f; // 대쉬 속도
        float minSqrDistance = .5f;

        while ((targetPos - rigid.position).magnitude > minSqrDistance)
        {
            Vector2 direction = (targetPos - rigid.position).normalized;
            Vector2 newPos = rigid.position + direction * dashSpeed * Time.fixedDeltaTime;

            rigid.MovePosition(newPos);

            yield return new WaitForFixedUpdate();
        }
        RemoveEffect();
        yield return null;
    }
}
