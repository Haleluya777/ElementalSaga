using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeBuff_LatedDamage : StatusEffectBase
{
    private GameObject attacker;
    private int damage;
    private int stunDmg;

    public DeBuff_LatedDamage(float duration, Unit target, string _effectName, GameObject _attacker, int _damage, int _stunDmg) : base(duration, target)
    {
        effectName = _effectName;
        damage = _damage;
        stunDmg = _stunDmg;
        attacker = _attacker;
    }

    public override void ApplyEffect()
    {
        Debug.Log("타이머 시작");
    }

    public override void RemoveEffect(bool isRefresh = false)
    {
        Debug.Log($"타이머 종료. 데미지 들어감 : {damage}.");
        target.TakeDamage(damage, stunDmg, attacker.GetComponentInChildren<ISkillCaster>(), null);
    }
}
