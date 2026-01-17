using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Basic_Attack", menuName = "ScriptableObject/Skills/Active/YeonHaRyeon/Basic_Attack")]
public class Basic_Attack_YeonHaRyeon : SkillBase
{
    [SerializeField] private Vector2 hitBoxOffset;
    [SerializeField] private Vector2 hitBoxSize;
    [SerializeField] private SkillBase chainedSkill;
    [SerializeField] private SkillBase firstComboSkill;
    [SerializeField] private GameObject hitBoxObj;
    [SerializeField] private float dashDistance;
    [SerializeField] private List<OnHitEventBase> onHitEvents = new List<OnHitEventBase>();
    private Vector2 tagetPos;
    private Coroutine coroutine;
    private IAttackable attack;
    private bool eventsInitialized = false;

    // 이 스킬에 필요한 OnHitEvent 타입을 new()로 생성하는 대신 typeof()로 타입 정보만 저장합니다.
    private List<Type> needEventTypes = new List<Type> { typeof(FillHeatPressure) };

    // ScriptableObject가 로드될 때 호출됩니다. (게임 시작 시 등)
    private void OnEnable()
    {
        // 초기화 상태 플래그를 리셋하여 게임을 시작할 때마다 이벤트가 새로 할당되도록 합니다.
        eventsInitialized = false;
        onHitEvents.Clear();
    }

    private void InitializeOnHitEvents(ISkillCaster caster)
    {
        if (attack == null) attack = caster.GetCom<IAttackable>();
        if (attack == null) return;

        // onHitEvents 리스트를 초기화
        onHitEvents.Clear();

        // needEventTypes에 정의된 각 이벤트 타입을 Attack 컴포넌트에서 찾아 onHitEvents 리스트에 추가
        foreach (var eventType in needEventTypes)
        {
            var foundEvent = attack.FindOnHitEvent(eventType);
            if (foundEvent != null)
            {
                onHitEvents.Add(foundEvent);
            }
            else
            {
                Debug.LogWarning($"'{eventType.Name}' 이벤트를 Attack 컴포넌트에서 찾을 수 없습니다.");
            }
        }
        eventsInitialized = true;
    }

    public override bool UseSkill(ISkillCaster caster)
    {
        // 이벤트가 초기화되지 않았다면 한번만 실행합니다.
        if (!eventsInitialized)
        {
            InitializeOnHitEvents(caster);
        }

        if (parentModule.blackBoard.HasKey("Basic_Combo")) //이미 3초 타이머가 진행중인 상태에서 스킬을 사용한다면.
        {
            //타이머 역할을 하는 코루틴 종료 후 스킬 실행.
            GameManager.instance.coroutineRunner.StopCoroutine(parentModule.blackBoard.Get<Coroutine>("Basic_Combo"));
            parentModule.blackBoard.Remove("Basic_Combo");
        }

        //스킬 사용 중 상황.
        //히트 박스 사이즈 및 위치 조정.
        GameObject hitBox = GameManager.instance.objectPoolManager.poolDic["HitBox"].GetGo("HitBox");

        hitBox.transform.position = caster.GetHitBoxPos().position;
        hitBox.transform.SetParent(caster.GetGameObject().transform.GetChild(2).transform.GetChild(0));

        HitBox hitBoxCom = hitBox.GetComponent<HitBox>();

        hitBox.tag = caster.GetGameObject().tag;

        hitBoxCom.GetComponent<BoxCollider2D>().size = hitBoxSize;
        hitBoxCom.GetComponent<BoxCollider2D>().offset = hitBoxOffset;

        hitBoxCom.Initialize(dmgCalculater.Calculate(caster), caster, onHitEvents, .5f);

        tagetPos = new Vector2(caster.GetPosition().x + (caster.GetDirection().normalized.x * dashDistance), caster.GetPosition().y);

        //Debug.Log(tagetPos.x);
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

    private void OnApplicationQuit()
    {
        onHitEvents.Clear();
    }
}
