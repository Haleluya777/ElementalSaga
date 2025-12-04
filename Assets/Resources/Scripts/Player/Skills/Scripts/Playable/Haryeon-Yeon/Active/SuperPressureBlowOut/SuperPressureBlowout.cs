using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SuperPressureBlowout", menuName = "ScriptableObject/Skills/Active/YeonHaRyeon/SuperPressureBlowout")]
public class SuperPressureBlowout : SkillBase
{
    [SerializeField] private SkillBase explosionSkill;
    [SerializeField] private HeatPressure heatPressurePassive;
    private WaitForSeconds duration = new WaitForSeconds(1f);
    private int explosionCount;

    public override bool UseSkill(ISkillCaster caster)
    {
        explosionCount = heatPressurePassive.heatPressure < 100 ? 3 : 4;

        Debug.Log($"{explosionCount} 번 폭발 예정");
        GameManager.instance.coroutineRunner.StartCoroutine(ExplosionCoroutine(caster));

        return true;
    }

    private IEnumerator ExplosionCoroutine(ISkillCaster caster)
    {
        heatPressurePassive.heatPressure = 100;
        yield return new WaitForSeconds(1f);
        for (int i = 1; i <= explosionCount; i++)
        {
            Vector2 hitBoxSize = new Vector2((i * 2) - 1, 1);
            Vector2 hitBoxOffset = new Vector2(-.5f, .5f);

            Debug.Log($"폭발! {i}단계!");

            explosionSkill.dmgCalculater.weight = 4f + (i - 1) * 2f;
            explosionSkill.HitBoxInit(hitBoxSize, hitBoxOffset);
            explosionSkill.UseSkill(caster);
            yield return duration;
        }
    }
}
