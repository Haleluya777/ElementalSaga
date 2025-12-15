using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Special_C", menuName = "ScriptableObject/Skills/Active/YunSeoYeon/Special_C")]
public class Special_C : SkillBase
{
    [Header("스킬을 사용할 때의 히트박스 위치와 크기")]
    [SerializeField] private Vector2 hitBoxOffset;
    [SerializeField] private Vector2 hitBoxSize;

    [SerializeField] private SkillBase chainedSkill;
    private Unit unit;

    public override void Initialize(Skill_Module module)
    {
        chainedSkill.Initialize(module);
        base.Initialize(module);
    }

    public override bool UseSkill(ISkillCaster caster)
    {
        if (unit == null) unit = caster.GetCom<Unit>();

        if (unit.isAirial)
        {
            chainedSkill.UseSkill(caster);
        }
        else
        {
            caster.PlayAnimation(animName);

            GameObject hitBox = GameManager.instance.objectPoolManager.GetGo("HitBox");
            GameObject effectObj = GameManager.instance.objectPoolManager.GetGo("Effect");

            hitBox.transform.position = caster.GetHitBoxPos().position;
            effectObj.transform.position = caster.GetHitBoxPos().position;

            hitBox.tag = caster.GetGameObject().tag;

            HitBox hitBoxCom = hitBox.GetComponent<HitBox>();
            Effect effectCom = effectObj.GetComponent<Effect>();

            hitBoxCom.GetComponent<BoxCollider2D>().size = hitBoxSize;
            hitBoxCom.GetComponent<BoxCollider2D>().offset = hitBoxOffset;

            hitBoxCom.Initialize(dmgCalculater.Calculate(caster), caster, null, .5f);
            effectCom.Initialize(.5f);

            GameManager.instance.coroutineRunner.StartCoroutine(UseChainedSkill(caster));
        }

        return true;
    }

    private IEnumerator UseChainedSkill(ISkillCaster caster)
    {
        yield return new WaitForSeconds(.4f);
        yield return null;

        chainedSkill.UseSkill(caster);
        yield return null;
    }
}
