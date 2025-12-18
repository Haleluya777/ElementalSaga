using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "ControlBloodDegree", menuName = "ScriptableObject/Skills/Active/YeonHaRyeong/ControlBloodDegree")]
public class ControlBloodDegree : SkillBase
{
    [Header("히트박스 정보")]
    [SerializeField] private Vector2 hitBoxOffset;
    [SerializeField] private Vector2 hitBoxSize;

    private FlameBlood flameBlood;
    private Unit unit;
    private IAttackable attack;

    public override bool UseSkill(ISkillCaster caster)
    {
        if (unit == null) unit = caster.GetCom<Unit>();
        if (attack == null) attack = unit.GetComponentInChildren<IAttackable>();
        if (flameBlood == null) flameBlood = attack.FindSkill(flameBlood);


        if (unit.FindEffect("ExplosionBlood")) //폭혈 상태에서 혈열 조절을 사용할 시.
        {
            Debug.Log("폭발한다!");
            GameManager.instance.coroutineRunner.StartCoroutine(RemoveExplosionBlood(unit));

            GameObject hitBox = GameManager.instance.objectPoolManager.GetGo("HitBox");
            GameObject effectObj = GameManager.instance.objectPoolManager.GetGo("Effect");

            hitBox.transform.position = caster.GetGameObject().transform.position;
            effectObj.transform.position = caster.GetGameObject().transform.position;

            HitBox hitBoxCom = hitBox.GetComponent<HitBox>();
            Effect effectCom = effectObj.GetComponent<Effect>();

            hitBox.tag = caster.GetGameObject().tag;

            hitBoxCom.GetComponent<BoxCollider2D>().size = hitBoxSize;
            hitBoxCom.GetComponent<BoxCollider2D>().offset = hitBoxOffset;

            hitBoxCom.Initialize(dmgCalculater.Calculate(caster), caster, null, .5f);
            effectCom.Initialize(.5f);
        }
        else
        {
            Debug.Log("혈열 감소");
            flameBlood.flameBloodGage -= 10;
            GameManager.instance.coroutineRunner.StartCoroutine(DecreaseGage());
        }
        return true;
    }

    private IEnumerator DecreaseGage()
    {
        yield return null; //한 프레임 쉬고.
        unit.CurHp += (int)(unit.MaxHp * 8 / 100);
        unit.AddEffectProcess(new DeBuff_IncreaseDmgRate(3f, .15f, unit, "IncreaseDmgRate"));

    }

    private IEnumerator RemoveExplosionBlood(Unit unit)
    {
        //폭혈 버프 종료.
        if (unit.activeEffect.TryGetValue("ExplosionBlood", out StatusEffectBase exisitngEffect))
        {
            if (unit.activeEffectCoroutines.TryGetValue("ExplosionBlood", out Coroutine runningCoroutine))
            {
                //코루틴을 이용한 상태이상의 타이머 제거.
                GameManager.instance.coroutineRunner.StopCoroutine(runningCoroutine);
            }
            //적용 되어 있는 상태이상 또한 제거.
            exisitngEffect.RemoveEffect();
        }

        yield return null;
        if (unit.activeEffect.TryGetValue("Slow", out StatusEffectBase slowEffect))
        {
            if (unit.activeEffectCoroutines.TryGetValue("Slow", out Coroutine runningCoroutine))
            {
                //코루틴을 이용한 상태이상의 타이머 제거.
                GameManager.instance.coroutineRunner.StopCoroutine(runningCoroutine);
            }
            //적용 되어 있는 상태이상 또한 제거.
            slowEffect.RemoveEffect();
        }
    }
}
