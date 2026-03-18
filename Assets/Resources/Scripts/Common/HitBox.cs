using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : PoolAble
{
    private int totalDmg;
    private int stunDmg;
    private ISkillCaster caster;
    private List<OnHitEventBase> onHitEvents; //공격이 명중했을 때의 공격자, 혹은 피격자에게 적용되는 효과.
    private float limitTime; //오브젝트가 생성된 후 몇 초 뒤에 비활성화 될지 정하는 시간.

    public void Initialize(int damage, int _stunDmg, ISkillCaster _caster, List<OnHitEventBase> _onHitEvents, float _limitTime)
    {
        totalDmg = damage;
        caster = _caster;
        onHitEvents = _onHitEvents;
        limitTime = _limitTime;
        stunDmg = _stunDmg;
        GameManager.instance.coroutineRunner.StartCoroutine(ReturnToPool());
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (caster == null || other.gameObject.tag == caster.GetGameObject().tag) return;

        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(totalDmg, stunDmg, caster, other.gameObject);
            if (onHitEvents is not null && onHitEvents.Count > 0)
            {
                foreach (var effect in onHitEvents)
                {
                    if (effect != null)
                    {
                        Debug.Log("타격 이벤트!");
                        effect.Execute(other.gameObject, caster, totalDmg);
                    }
                    else
                    {
                        Debug.Log("타격 이벤트가 할당 되어 있지 않음.");
                    }
                }
            }
            else
            {
                Debug.Log("타격 이벤트가 할당 되어 있지 않음.");
            }
        }
    }

    private IEnumerator ReturnToPool()
    {
        yield return new WaitForSeconds(limitTime);
        ReleaseObject();
    }
}
