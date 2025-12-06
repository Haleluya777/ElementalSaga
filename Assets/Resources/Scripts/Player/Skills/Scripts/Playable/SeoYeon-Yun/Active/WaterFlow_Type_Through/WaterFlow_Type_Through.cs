using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaterFlow_Type_Through", menuName = "ScriptableObject/Skills/Active/YunSeoYeon/WaterFlow_Type_Through")]
public class WaterFlow_Type_Through : SkillBase
{
    [SerializeField] private Flow flow;
    [SerializeField] private GameObject bullet;
    private ISkillCaster thisCaster;
    private int killCount;
    private bool reinforced;

    public override void Initialize(Skill_Module module)
    {
        killCount = 0;
        base.Initialize(module);
        GameManager.instance.eventManager.DeadEvent += KillCount;
    }

    public override bool UseSkill(ISkillCaster caster)
    {
        if (bullet == null) return false;
        thisCaster = caster;

        GameObject objInstance = Instantiate(bullet, caster.GetPosition() + new Vector2(0f, .5f), caster.GetRotation());
        SkillObjBase objComponent = objInstance.GetComponent<SkillObjBase>();

        if (objComponent is not null)
        {
            int calculatedDamage = 0;
            if (dmgCalculater is not null)
            {
                calculatedDamage = dmgCalculater.Calculate(caster);
            }

            reinforced = flow.flowGage >= 6 ? true : false;
            objComponent.ObjInit(caster.GetDirection(), calculatedDamage, caster.GetGameObject().tag, caster, reinforced);
            if (reinforced) flow.flowGage = 0;
        }

        return true;
    }

    private void KillCount(ISkillCaster attacker)
    {
        if (attacker != thisCaster)
        {
            return;
        }

        killCount++;
        Debug.Log($"킬! {killCount}명째");
        if (killCount >= 3)
        {
            Debug.Log("쿨타임 초기화!");
            ResetCoolDown();
            killCount = 0;
        }
    }

    private void ResetCoolDown()
    {
        parentModule.ResetCoolDown();
    }
}
