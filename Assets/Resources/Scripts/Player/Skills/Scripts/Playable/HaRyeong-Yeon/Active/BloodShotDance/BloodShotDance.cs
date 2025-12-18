using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BloodShotDance", menuName = "ScriptableObject/Skills/Active/YeonHaRyeong/BloodShotDance")]
public class BloodShotDance : SkillBase
{
    [SerializeField] private Skill_Module reinforcedSkill;
    [SerializeField] private Skill_Module basicSkill;
    private Unit unit;

    public override void Initialize(Skill_Module module)
    {
        base.Initialize(module);

        reinforcedSkill = Instantiate(reinforcedSkill);
        reinforcedSkill.InitSkill();
        basicSkill = Instantiate(basicSkill);
        basicSkill.InitSkill();
    }

    public override bool UseSkill(ISkillCaster caster)
    {
        if (unit == null) unit = caster.GetCom<Unit>();

        if (!unit.FindEffect("ExplosionBlood")) return false;
        unit.AddEffectProcess(new Buff_ExplosionBlood(8f, unit, 150, 70, "ExplosionBlood", basicSkill, reinforcedSkill));
        GameManager.instance.coroutineRunner.StartCoroutine(RemoveSlow());
        unit.AddEffectProcess(new Buff_KillDrain(8f, unit, 5f, "KillDrain"));

        return true;
    }

    private IEnumerator RemoveSlow()
    {
        yield return null; //한프레임 쉬고.

        //슬로우 삭제.
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
