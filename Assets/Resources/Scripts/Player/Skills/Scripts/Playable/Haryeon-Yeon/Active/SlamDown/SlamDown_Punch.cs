using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SlamDown_Punch", menuName = "ScriptableObject/Skills/Active/YeonHaRyeon/SlamDown_Punch")]
public class SlamDown_Punch : SkillBase
{
    [SerializeField] private SkillBase punchExplosion;

    [SerializeField] private Vector2 hitBoxOffset;
    [SerializeField] private Vector2 hitBoxSize;

    public override bool UseSkill(ISkillCaster caster)
    {
        GameObject hitBox = GameManager.instance.objectPoolManager.poolDic["HitBox"].GetGo("HitBox");
        GameObject effectObj = GameManager.instance.objectPoolManager.poolDic["Effect"].GetGo("Effect");

        hitBox.transform.position = caster.GetHitBoxPos().position;
        effectObj.transform.position = caster.GetHitBoxPos().position;

        hitBox.tag = caster.GetGameObject().tag;

        HitBox hitBoxCom = hitBox.GetComponent<HitBox>();
        Effect effectCom = effectObj.GetComponent<Effect>();

        hitBoxCom.GetComponent<BoxCollider2D>().size = hitBoxSize;
        hitBoxCom.GetComponent<BoxCollider2D>().offset = hitBoxOffset;

        hitBoxCom.Initialize(dmgCalculater.Calculate(caster), caster, null, .5f);
        effectCom.Initialize(.5f);

        caster.PlayAnimation(animName);
        GameManager.instance.coroutineRunner.StartCoroutine(PunchExplosion(caster));
        return true;
    }

    private IEnumerator PunchExplosion(ISkillCaster caster)
    {
        yield return new WaitForSeconds(.5f);
        Debug.Log("후폭풍!");
        punchExplosion.UseSkill(caster);
    }
}
