using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Special_A", menuName = "ScriptableObject/Skills/Active/YunSeoYeon/Special_A")]
public class Special_A : SkillBase
{
    [Header("공중 체공 시간")]
    [SerializeField] private float duration;
    private Unit unit;

    public override bool UseSkill(ISkillCaster caster)
    {
        if (unit == null) unit = caster.GetCom<Unit>();

        if (unit.isAirial)
        {
            GameManager.instance.coroutineRunner.StartCoroutine(HangTime(caster, unit.GetComponent<Rigidbody2D>()));
            GameManager.instance.coroutineRunner.StartCoroutine(DoubleShoot(caster));
            caster.PlayAnimation(animName);
            parentModule.blackBoard.Set<bool>("Condition", false);
        }
        else
        {
            GameObject bullet = GameManager.instance.objectPoolManager.GetGo("Bullet_Through");
            SkillObjBase objComponent = bullet.GetComponent<SkillObjBase>();
            caster.PlayAnimation(animName);
            objComponent.ObjInit(caster.GetDirection(), dmgCalculater.Calculate(caster), caster.GetGameObject().tag, caster, false);
        }

        return true;
    }

    private IEnumerator DoubleShoot(ISkillCaster caster)
    {
        SkillObjBase objComponent;
        int count = 0;

        while (count < 2)
        {
            count++;
            GameObject bullet = GameManager.instance.objectPoolManager.GetGo("Bullet");
            objComponent = bullet.GetComponent<SkillObjBase>();

            objComponent.ObjInit(caster.GetDirection(), dmgCalculater.Calculate(caster), caster.GetGameObject().tag, caster, false);

            yield return new WaitForSeconds(.2f);
        }
        yield return null;
    }

    private IEnumerator HangTime(ISkillCaster caster, Rigidbody2D rigid)
    {
        float timer = 0f;
        var previousVelocity = rigid.velocity;
        while (timer < duration)
        {
            rigid.velocity = new Vector2(0f, 1f);
            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        //rigid.velocity = previousVelocity;
    }
}
