using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LaijutsuAction", menuName = "ScriptableObject/Skills/Active/Guren_Kagari/LaijutsuAction")]
public class LaijutsuAction : SkillBase
{
    [SerializeField] private Vector2 hitBoxOffset;
    [SerializeField] private Vector2 hitBoxSize;

    public override bool UseSkill(ISkillCaster caster)
    {
        GameObject hitBox = LocalGameManager.instance.objectPoolManager.poolDic["HitBox"].GetGo("HitBox");
        GameObject effect = LocalGameManager.instance.objectPoolManager.poolDic["Effect"].GetGo("Effect");

        hitBox.transform.SetParent(caster.GetGameObject().transform.GetChild(2).transform.GetChild(0));
        effect.transform.SetParent(caster.GetGameObject().transform.GetChild(2).transform.GetChild(0));

        HitBox hitBoxCom = hitBox.GetComponent<HitBox>();
        Effect effectCom = effect.GetComponent<Effect>();

        hitBox.tag = caster.GetGameObject().tag;

        hitBox.transform.localScale = hitBoxSize;
        hitBox.transform.localPosition = hitBoxOffset;

        effect.transform.localScale = hitBoxSize;
        effect.transform.localPosition = hitBoxOffset;

        hitBoxCom.Initialize(dmgCalculater.Calculate(caster), stunDmg, caster, null, .15f);
        effectCom.Initialize(.2f);

        caster.PlayAnimation(animName);

        return true;
    }
}
