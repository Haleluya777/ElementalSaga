using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeBuff_Catched : StatusEffectBase
{
    GameObject attacker;
    Transform parent;

    public DeBuff_Catched(float duration, Unit target, int _rate, string _effectName, GameObject _attacker, Transform _parent) : base(duration, target)
    {
        effectName = _effectName;
        attacker = _attacker;
        parent = _parent;
    }

    public override void ApplyEffect()
    {
        //행동불가 On
        target.GetComponent<Rigidbody2D>().simulated = false; //물리 효과 비활성화.
        target.GetComponent<BoxCollider2D>().enabled = false; //충돌 효과 비활성화.

        target.gameObject.transform.SetParent(parent); //디버프를 가진 오브젝트를 공격자의 잡기 오브젝트 자식으로 들어감
        target.gameObject.transform.position = parent.position; //위치 조정.
    }

    public override void RemoveEffect()
    {
        //행동불가 Off
        target.GetComponent<Rigidbody2D>().simulated = true; //물리 효과 비활성화.
        target.GetComponent<BoxCollider2D>().enabled = true; //충돌 효과 비활성화.
        target.gameObject.transform.SetParent(null);
    }
}
