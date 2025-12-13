using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Basic_B", menuName = "ScriptableObject/Skills/Active/YunSeoYeon/Basic_B")]
public class Water_Basic_B : SkillBase
{
    [Header("공중 시전시 사용할 탄환 오브젝트")]
    [SerializeField] private GameObject bullet;

    [Header("지상에서 스킬을 사용할 때의 히트박스 위치와 크기")]
    [SerializeField] private Vector2 GroundhitBoxOffset;
    [SerializeField] private Vector2 GroundhitBoxSize;

    [Header("지상/공중에서 스킬을 시전할 때 실행할 애니메이션 클립 이름")]
    [SerializeField] string groundAnimName;
    [SerializeField] string airialAnimName;

    [Header("공중 체공 시간")]
    [SerializeField] private float duration;

    private Unit unit;
    private Rigidbody2D rigid;

    public override bool UseSkill(ISkillCaster caster)
    {
        if (unit == null) unit = caster.GetCom<Unit>();
        if (rigid == null) rigid = caster.GetCom<Rigidbody2D>();

        var hitBoxOffset = GroundhitBoxOffset;
        var hitBoxSize = GroundhitBoxSize;

        var animName = unit.isAirial ? airialAnimName : groundAnimName;

        parentModule.AnimName = animName;

        if (!unit.isAirial)
        {
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
        }

        else
        {
            //오브젝트 풀에서 탄환 가져옴.
            GameObject objInstance = GameManager.instance.objectPoolManager.GetGo("Bullet");
            //탄환 시작 위치 조정.
            objInstance.transform.position = caster.GetPosition() + new Vector2(0f, .5f);

            SkillObjBase objComponent = objInstance.GetComponent<SkillObjBase>();

            if (objComponent is not null)
            {
                int calculatedDamage = 0;
                if (dmgCalculater is not null)
                {
                    calculatedDamage = dmgCalculater.Calculate(caster);
                }
                objComponent.ObjInit(caster.GetDirection(), calculatedDamage, caster.GetGameObject().tag, caster, false);
            }

            GameManager.instance.coroutineRunner.StartCoroutine(HangTime(caster, rigid));
            parentModule.blackBoard.Set<bool>("Condition", false);
        }

        return true;
    }

    private IEnumerator HangTime(ISkillCaster caster, Rigidbody2D rigid) //공중 체공.
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
