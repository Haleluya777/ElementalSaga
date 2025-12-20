using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class DeBuff_Throwed : StatusEffectBase
{
    private SkillBase chainedSkill;
    private float distance;
    private Vector2 targetPos;
    private Rigidbody2D rigid;
    private ISkillCaster attacker;
    private Vector2 dir;

    private bool explosion;

    public DeBuff_Throwed(float duration, Unit target, int _rate, string _effectName, ISkillCaster _attacker, float _distance, SkillBase _chainedSkill, Vector2 _dir) : base(duration, target)
    {
        effectName = _effectName;
        distance = _distance;
        attacker = _attacker;
        chainedSkill = _chainedSkill;
        dir = _dir;
    }

    public override void ApplyEffect()
    {
        Transform unitTransform = this.target.gameObject.transform;
        rigid = this.target.GetComponent<Rigidbody2D>();
        //Debug.Log(dir);
        //float targetX = unitTransform.position.x + (dir * distance);

        targetPos = new Vector2(unitTransform.position.x + (dir.x * distance), unitTransform.position.y + (dir.y * distance));

        GameManager.instance.coroutineRunner.StartCoroutine(Throwing(unitTransform.position));
    }

    public override void RemoveEffect(bool isRefresh = false)
    {
        Debug.Log("도착. 버프 제거");
        if (explosion)
        {
            if (chainedSkill != null) chainedSkill.UseSkill(attacker, target.transform.position);
        }

        explosion = false;
    }

    private IEnumerator Throwing(Vector2 origin)
    {
        BoxCollider2D col = this.target.GetComponentInChildren<BoxCollider2D>();
        RaycastHit2D hitted;
        float dashSpeed = 30f; // 대쉬 속도
        float minSqrDistance = .5f;

        float x = dir.x == 0 ? 0 : (dir.x > 0 ? this.target.transform.position.x + col.size.x / 2 + .1f : this.target.transform.position.x - col.size.x / 2 - .1f);
        float y = dir.y == -0.1f ? this.target.transform.position.y + .5f : (dir.y > 0 ? this.target.transform.position.y + col.size.y / 2 + .1f : this.target.transform.position.y);

        hitted = Physics2D.Raycast(new Vector2(x, y), (targetPos - origin).normalized, distance, ~(1 << 8));
        Debug.DrawRay(new Vector2(x, y), (targetPos - origin).normalized, Color.red);

        if (hitted.collider != null)
        {
            targetPos = hitted.point;
            Debug.Log($"이동 범위 내 방해물 존재. 그 곳에서 멈춤. : {targetPos}");
            explosion = true;
        }

        while (true)
        {
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
