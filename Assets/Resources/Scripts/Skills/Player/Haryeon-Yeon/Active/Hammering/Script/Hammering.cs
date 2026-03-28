using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Hammering", menuName = "ScriptableObject/Skills/Active/YeonHaRyeon/Hammering")]
public class Hammering : SkillBase
{
    private IAttackable attack;
    private HeatSeal heatSeal;

    [SerializeField] private DmgCalculator explosionCal;
    [SerializeField] private Vector2 hitBoxOffset;
    [SerializeField] private Vector2 hitBoxSize;

    public override bool UseSkill(ISkillCaster caster)
    {
        if (attack is null) attack = caster.GetCom<IAttackable>();
        if (heatSeal is null) heatSeal = attack.FindSkill(heatSeal);

        Debug.Log("해머링~");

        GameObject hitBox = LocalGameManager.instance.objectPoolManager.poolDic["HitBox"].GetGo("HitBox");

        hitBox.transform.position = caster.GetHitBoxPos().position;
        hitBox.transform.SetParent(caster.GetGameObject().transform.GetChild(2).transform.GetChild(0));

        HitBox hitBoxCom = hitBox.GetComponent<HitBox>();

        hitBox.tag = caster.GetGameObject().tag;

        var hitBoxCol = hitBoxCom.GetComponent<BoxCollider2D>();

        hitBoxCol.size = hitBoxSize;
        hitBoxCol.offset = hitBoxOffset;

        hitBoxCom.Initialize(dmgCalculater.Calculate(caster), stunDmg, caster, null, .2f);
        caster.PlayAnimation(animName);

        if (heatSeal.heatSeal >= 3)
        {
            RaycastHit2D enemyHitted = Physics2D.Raycast(caster.GetPosition(), caster.GetDirection(), 1, 1 << 6);
            if (enemyHitted.collider != null)
            {
                LocalGameManager.instance.StartCoroutine(Explosion(enemyHitted.collider.gameObject, caster));
            }
        }

        heatSeal.heatSeal = 0;

        return true;
    }

    private IEnumerator Explosion(GameObject enemy, ISkillCaster caster)
    {
        yield return new WaitForSeconds(.4f);

        Debug.Log("폭발!");
        GameObject hitBox = LocalGameManager.instance.objectPoolManager.poolDic["HitBox"].GetGo("HitBox");

        hitBox.transform.position = enemy.gameObject.transform.position;
        hitBox.transform.SetParent(caster.GetGameObject().transform.GetChild(2).transform.GetChild(0));

        HitBox hitBoxCom = hitBox.GetComponent<HitBox>();

        hitBox.tag = caster.GetGameObject().tag;

        var hitBoxCol = hitBoxCom.GetComponent<BoxCollider2D>();

        hitBoxCol.size = new Vector2(2, 1);
        hitBoxCol.offset = new Vector2(1 * hitBox.transform.localScale.x * caster.GetDirection().x, 0);

        hitBoxCom.Initialize(explosionCal.Calculate(caster), stunDmg * 2, caster, null, 10f);
    }
}
