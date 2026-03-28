using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_Counter : StatusEffectBase
{
    public Buff_Counter(float duration, Unit target, string _effectName) : base(duration, target)
    {
        base.effectName = _effectName;
    }

    public override void ApplyEffect()
    {
        target.GraceState = true;
        target.TakeDamageEvent += AutoDodge;
    }

    public override void RemoveEffect(bool isRefresh = false)
    {
        target.GraceState = false;
        target.TakeDamageEvent -= AutoDodge;
    }

    public void AutoDodge(int dmg, ISkillCaster attacker, GameObject obj)
    {
        Debug.Log("공격 무시!");
        int dir = (int)attacker.GetDirection().x;
        LocalGameManager.instance.coroutineRunner.StartCoroutine(DodgeMovement(obj, new Vector2((obj.transform.position.x) + (3 * dir), obj.transform.position.y)));
    }

    private IEnumerator DodgeMovement(GameObject obj, Vector2 tagetPos)
    {
        float dashSpeed = 500f; // 대쉬 속도
        float minSqrDistance = .5f;
        Rigidbody2D rigid = obj.GetComponent<Rigidbody2D>();

        while (((Vector2)tagetPos - rigid.position).magnitude > minSqrDistance)
        {
            Vector2 direction = ((Vector2)tagetPos - rigid.position).normalized;
            Vector3 newPos = rigid.position + direction * (dashSpeed / 10) * Time.fixedDeltaTime;

            rigid.MovePosition(newPos);

            yield return new WaitForFixedUpdate();
        }
        RemoveEffect();
        yield return null;
    }
}
