using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_ExplosionBlood : StatusEffectBase
{
    private Skill_Module reinforced;
    private Skill_Module basic;
    private IAttackable attack;

    private WaitForSeconds interval = new WaitForSeconds(1f);
    private Coroutine coroutine;
    private int attRate;
    private int movementRate;
    private float attIncrease;
    private float moveIncrease;
    private float jumpIncrease;

    public Buff_ExplosionBlood(float duration, Unit target, int _attRate, int _movementRate, string _effectName, Skill_Module _basic, Skill_Module _reinforced) : base(duration, target)
    {
        attack = target.GetComponentInChildren<IAttackable>();

        effectName = _effectName;
        attRate = _attRate;
        movementRate = _movementRate;
        basic = _basic;
        reinforced = _reinforced;
    }

    public override void ApplyEffect()
    {
        coroutine = GameManager.instance.coroutineRunner.StartCoroutine(BloodLoss(target));
        attack.ActiveSkills[0] = reinforced;

        attIncrease = (target.Att * attRate) / 100;
        moveIncrease = (target.MoveSpeed * movementRate) / 100;
        jumpIncrease = (target.JumpForce * movementRate) / 100;

        target.Att += (int)attIncrease;
        target.MoveSpeed += (int)moveIncrease;
        target.JumpForce += (int)jumpIncrease;
    }

    public override void RemoveEffect(bool isRefresh = false)
    {
        GameManager.instance.coroutineRunner.StopCoroutine(coroutine);
        attack.ActiveSkills[0] = basic;
        target.Att -= (int)attIncrease;
        target.MoveSpeed -= (int)moveIncrease;
        target.JumpForce -= (int)jumpIncrease;

        if (!isRefresh)
        {
            target.AddEffectProcess(new Debuff_SlowMoveSpeed(3f, 75f, target, "Slow")); //상태 이상 갱신이 아닌 완전한 종료일 때 슬로우 디버프 적용.
        }
    }

    private IEnumerator BloodLoss(Unit unit)
    {
        int dmgRate = unit.CurHp * 3 / 100;
        while (true)
        {
            yield return interval;
            unit.CurHp -= dmgRate;
        }
    }
}
