using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LaijutsuAction", menuName = "ScriptableObject/Skills/Active/Guren_Kagari/LaijutsuAction")]
public class LaijutsuAction : SkillBase
{
    [SerializeField] private Vector2 hitBoxOffset;
    [SerializeField] private Vector2 hitBoxSize;
    [SerializeField] private GameObject hitBoxObj;

    public override bool UseSkill(ISkillCaster caster)
    {
        Debug.Log("fgjg");
        GameObject hitBox = LocalGameManager.instance.objectPoolManager.poolDic["HitBox"].GetGo("HitBox");

        hitBox.transform.position = caster.GetHitBoxPos().position;
        hitBox.transform.SetParent(caster.GetGameObject().transform.GetChild(2).transform.GetChild(0));

        HitBox hitBoxCom = hitBox.GetComponent<HitBox>();

        hitBox.tag = caster.GetGameObject().tag;

        var hitBoxCol = hitBoxCom.GetComponent<BoxCollider2D>();

        hitBoxCol.size = hitBoxSize;
        hitBoxCol.offset = hitBoxOffset;

        hitBoxCom.Initialize(dmgCalculater.Calculate(caster), stunDmg, caster, null, .15f);
        caster.PlayAnimation(animName);

        return true;
    }
}
