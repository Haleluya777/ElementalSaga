using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "Basic_Attack", menuName = "ScriptableObject/Skills/Basic_Attack")]
public class Basic_Attack : SkillBase
{
    [SerializeField] private List<OnHitEventBase> onHitEvents = new List<OnHitEventBase>();
    [SerializeField] private Vector2 hitBoxOffset;
    [SerializeField] private Vector2 hitBoxSize;
    [SerializeField] private SkillBase chainedSkill;
    [SerializeField] private SkillBase firstComboSkill;
    [SerializeField] private GameObject hitBoxObj;
    [SerializeField] private float dashDistance;
    private Vector2 tagetPos;
    private Coroutine coroutine;

    public override bool UseSkill(ISkillCaster caster)
    {
        if (parentModule.blackBoard.HasKey("Basic_Combo")) //이미 3초 타이머가 진행중인 상태에서 스킬을 사용한다면.
        {
            //타이머 역할을 하는 코루틴 종료 후 스킬 실행.
            GameManager.instance.coroutineRunner.StopCoroutine(parentModule.blackBoard.Get<Coroutine>("Basic_Combo"));
            parentModule.blackBoard.Remove("Basic_Combo");
        }

        //스킬 사용 중 상황.
        //히트 박스 사이즈 및 위치 조정.
        //caster.GetHitBox().offset = hitBoxOffset;
        //caster.GetHitBox().size = hitBoxSize;
        //GameObject hitBox = Instantiate(hitBoxObj, caster.GetPosition(), caster.GetRotation());
        GameObject hitBox = GameManager.instance.objectPoolManager.GetGo("HitBox");

        hitBox.transform.position = caster.GetHitBoxPos().position;
        hitBox.transform.SetParent(caster.GetGameObject().transform.GetChild(2).transform.GetChild(0));

        HitBox hitBoxCom = hitBox.GetComponent<HitBox>();

        hitBox.tag = caster.GetGameObject().tag;

        hitBoxCom.GetComponent<BoxCollider2D>().size = hitBoxSize;
        hitBoxCom.GetComponent<BoxCollider2D>().offset = hitBoxOffset;

        hitBoxCom.Initialize(dmgCalculater.Calculate(caster), caster, onHitEvents, .5f);

        tagetPos = new Vector2(caster.GetPosition().x + (caster.GetDirection().normalized.x * dashDistance), caster.GetPosition().y);

        Debug.Log(tagetPos.x);
        caster.PlayAnimation(animName);
        GameManager.instance.coroutineRunner.StartCoroutine(PerformDash(caster, caster.GetCom<Rigidbody2D>(), caster.GetGameObject().transform, tagetPos));

        //스킬 사용 후 상황.
        //연결된 스킬 사용.
        if (chainedSkill == null) //연결된 스킬이 없을 때. (사용한 스킬이 콤보의 마지막 스킬일 때.)
        {
            parentModule.ChangeSkillModule(this, firstComboSkill); //1번 콤보로 되돌아감.
            caster.GetCom<IAttackable>().Combo = 0;
            //parentModule.coolDown = 3f; //부모 모듈의 쿨타임 변경. (콤보가 끝났으므로.)
        }
        else //다음 콤보가 존재할 때.
        {
            parentModule.ChangeSkillModule(this, chainedSkill); //다음 콤보 진행.
            coroutine = GameManager.instance.coroutineRunner.StartRunnerCoroutine(ReturnCombo(caster)); //타이머 실행
            parentModule.blackBoard.Set("Basic_Combo", coroutine);
            caster.GetCom<IAttackable>().Combo++;
        }

        return true;
    }

    private IEnumerator ReturnCombo(ISkillCaster caster)
    {
        yield return new WaitForSeconds(3f);
        parentModule.ChangeSkillModule(chainedSkill, firstComboSkill);
        parentModule.blackBoard.Remove("Basic_Combo");
        //parentModule.coolDown = 3f;
        caster.GetCom<IAttackable>().Combo = 0;
        Debug.Log("3초지남. 콤보 초기화!");
    }

    private IEnumerator PerformDash(ISkillCaster caster, Rigidbody2D rigid, Transform casterTransform, Vector2 tagetPos)
    {
        float dashSpeed = 35f; // 대쉬 속도
        float minSqrDistance = .5f;

        while (((Vector2)tagetPos - rigid.position).magnitude > minSqrDistance)
        {
            if (Physics2D.Raycast(new Vector2(caster.GetGameObject().transform.position.x, caster.GetGameObject().transform.position.y + .5f), Vector2.right * caster.GetGameObject().transform.localScale.x, .75f, 1 << 3))
            {
                break;
            }

            Vector2 direction = ((Vector2)tagetPos - rigid.position).normalized;
            Vector3 newPos = rigid.position + direction * (dashSpeed / 10) * Time.fixedDeltaTime;

            rigid.MovePosition(newPos);

            yield return new WaitForFixedUpdate();
        }
        yield return null;
    }
}
