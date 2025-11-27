using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : PoolAble
{
    private int totalDmg;
    private ISkillCaster caster;
    private List<OnHitEventBase> onHitEvents;
    private float limitTime; //오브젝트가 생성된 후 몇 초 뒤에 비활성화 될지 정하는 시간.

    public void Initialize(int damage, ISkillCaster _caster, List<OnHitEventBase> _onHitEvents, float _limitTime)
    {
        totalDmg = damage;
        caster = _caster;
        onHitEvents = _onHitEvents;
        limitTime = _limitTime;
        GameManager.instance.coroutineRunner.StartCoroutine(ReturnToPool());
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (caster == null || other.gameObject.tag == caster.GetGameObject().tag) return;

        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(totalDmg, caster, other.gameObject);
            if (onHitEvents != null)
            {
                foreach (var effect in onHitEvents)
                {
                    if (effect != null) effect.Execute(other.gameObject, caster, totalDmg);
                }
            }
        }
    }

    private IEnumerator ReturnToPool()
    {
        yield return new WaitForSeconds(limitTime);
        ReleaseObject();
    }
}
