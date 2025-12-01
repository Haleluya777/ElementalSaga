using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeBuff_Catched : StatusEffectBase
{
    GameObject attacker;

    public DeBuff_Catched(float duration, Unit target, int _rate, string _effectName, GameObject _attacker) : base(duration, target)
    {
        effectName = _effectName;
        attacker = _attacker;
    }

    public override void ApplyEffect()
    {
        //행동불가 On
        target.GetComponent<Rigidbody2D>().simulated = false; //물리 효과 비활성화.
        target.GetComponent<BoxCollider2D>().enabled = false; //충돌 효과 비활성화.
        target.gameObject.transform.SetParent(attacker.gameObject.transform); //디버프를 가진 오브젝트를 공격자 오브젝트 위치로 이동
    }

    public override void RemoveEffect()
    {
        //행동불가 Off
        target.GetComponent<Rigidbody2D>().simulated = true; //물리 효과 비활성화.
        target.GetComponent<BoxCollider2D>().enabled = true; //충돌 효과 비활성화.
        target.gameObject.transform.SetParent(null);
    }
}
