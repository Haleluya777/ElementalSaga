using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Purgatory_Explosion", menuName = "ScriptableObject/Skills/Active/YeonHaRyeon/Purgatory_Explosion")]
public class Purgatory_Explosion : SkillBase
{
    private IAttackable attack;

    [SerializeField] private Vector2 hitBoxOffset;
    [SerializeField] private Vector2 hitBoxSize;

    public override bool UseSkill(ISkillCaster caster)
    {
        if (attack is null) attack = caster.GetCom<IAttackable>();

        GameObject hitBox = GameManager.instance.objectPoolManager.poolDic["HitBox"].GetGo("HitBox");
        GameObject effect = GameManager.instance.objectPoolManager.poolDic["Effect"].GetGo("Effect");

        hitBox.transform.position = caster.GetHitBoxPos().position;
        hitBox.transform.SetParent(caster.GetGameObject().transform.GetChild(2).transform.GetChild(0));

        effect.transform.position = caster.GetHitBoxPos().position;
        effect.transform.SetParent(caster.GetGameObject().transform.GetChild(2).transform.GetChild(0));

        HitBox hitBoxCom = hitBox.GetComponent<HitBox>();
        Effect effectCom = effect.GetComponent<Effect>();

        hitBox.tag = caster.GetGameObject().tag;

        var hitBoxCol = hitBoxCom.GetComponent<BoxCollider2D>();

        hitBoxCol.size = hitBoxSize;
        hitBoxCol.offset = hitBoxOffset;

        hitBoxCom.Initialize(dmgCalculater.Calculate(caster), stunDmg, caster, null, .2f);
        effectCom.Initialize(.2f);
        //caster.PlayAnimation(animName);

        Debug.Log($"차징 끝! 펀치! 총 데미지 : {dmgCalculater.Calculate(caster)}");
        return true;
    }
}
