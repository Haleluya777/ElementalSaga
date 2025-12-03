using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SuperPressureBlowout", menuName = "ScriptableObject/Skills/Active/YeonHaRyeon/SuperPressureBlowout")]
public class SuperPressureBlowout : SkillBase
{
    [SerializeField] private SkillBase explosionSkill;
    [SerializeField] private HeatPressure heatPressurePassive;
    private WaitForSeconds duration = new WaitForSeconds(.5f);
    private int explosionCount;

    public override bool UseSkill(ISkillCaster caster)
    {
        explosionCount = heatPressurePassive.heatPressure == 100 ? 4 : 3;
        heatPressurePassive.heatPressure = 100;

        GameManager.instance.coroutineRunner.StartCoroutine(ExplosionCoroutine(caster));

        return true;
    }

    private IEnumerator ExplosionCoroutine(ISkillCaster caster)
    {
        yield return new WaitForSeconds(1f);
        for (int i = 1; i <= explosionCount; i++)
        {
            Vector2 hitBoxSize = new Vector2((i * 2) - 1, 1);
            Vector2 hitBoxOffset = new Vector2(0, 0.5f);

            explosionSkill.HitBoxInit(hitBoxSize, hitBoxOffset);
            explosionSkill.UseSkill(caster);
            yield return duration;
        }
    }
}
