using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class Attack : MonoBehaviour, IAttackable, ISkillCaster, IDataInitializable
{
    public event Action<int> OnHitEvent;

    [SerializeField] private Skill_Module[] basicAttacks = new Skill_Module[2]; //유닛이 땅/공중에 있을 때 교체할 기본 공격 모듈.
    [SerializeField] private GameObject parentObj;
    [SerializeField] private BoxCollider2D hitBox;
    [SerializeField] private Rigidbody2D rigid;
    private PlayerController controller;
    private Unit unit;
    private Animator anim;

    [Header("특정 키를 눌러 사용하는 액티브 스킬.")] //기본 공격도 포함.
    [SerializeField] private List<Skill_Module> activeSkills = new List<Skill_Module>(); //액티브 기본 스킬
    [SerializeField] private List<Skill_Module> modifiedActiveSkills = new List<Skill_Module>(); //특정 캐릭터의 커맨드로 발동하는 액티브 스킬.

    [Header("특정 조건을 만족하면 자동으로 사용되는 패시브 스킬")]
    [SerializeField] private List<Skill_Module> passiveSkills = new List<Skill_Module>();

    [Header("유물 효과들")]
    [SerializeField] private List<Skill_Module> relicPowers = new List<Skill_Module>();

    [Header("맞을 때 실행할 효과들")]
    //맞을 때 실행할 효과들은 공격자의 정보가 필요한 경우도 존재하므로, 새로운 클래스를 만들어 사용.
    [SerializeField] private List<DamagedEventBase> hitEvents = new List<DamagedEventBase>();

    [SerializeField] private int combo;

    public int TotalDmg { set; get; }
    public bool Attacking { set; get; }
    public bool CancleAllSkill { get; }
    public int Combo { get => combo; set => combo = value; }

    public List<Skill_Module> ActiveSkills { get => activeSkills; set => activeSkills = value; }
    public List<Skill_Module> ModifiedActiveSkills { get => modifiedActiveSkills; set => modifiedActiveSkills = value; }
    public List<Skill_Module> PassiveSkills { get => passiveSkills; set => passiveSkills = value; }
    public List<Skill_Module> RelicPowers { get => relicPowers; set => relicPowers = value; }
    public List<DamagedEventBase> HitEvents { get => hitEvents; set => hitEvents = value; }

    void Update()
    {
        UpdateCoolDown();
        UsePassiveSkills();
        //ChangeAirialBasicAttack();
    }

    public void DataInit()
    {
        unit = parentObj.GetComponent<Unit>();
        anim = parentObj.GetComponent<Animator>();
        rigid = parentObj.GetComponent<Rigidbody2D>();

        // 모든 스킬 및 이벤트 리스트를 순회하며 초기화합니다.
        SkillInit(hitEvents);
        SkillInit(activeSkills);
        SkillInit(modifiedActiveSkills);
        SkillInit(passiveSkills);
    }

    private void SkillInit<T>(List<T> skills) where T : class
    {
        // ScriptableObject는 Instantiate로 복제해서 개별 상태(쿨타임 등)를 관리해야 합니다.
        for (int i = 0; i < skills.Count; i++)
        {
            if (skills[i] == null) continue;

            // UnityEngine.Object로 캐스팅하여 Instantiate를 사용합니다.
            var original = skills[i] as UnityEngine.Object;
            if (original == null) continue;

            var newInstance = Instantiate(original);

            // C# 7.0부터 지원하는 is 표현식 + 패턴 매칭을 사용하여 코드를 간결하게 만듭니다.
            // 타입에 따라 적절한 Initialize 메서드를 호출합니다.
            if (newInstance is DamagedEventBase damagedEvent)
            {
                damagedEvent.Initialize(unit);
            }
            else if (newInstance is Skill_Module skillModule)
            {
                // Skill_Module은 ISkillCaster(자기 자신 'this')를 넘겨주어 초기화한다고 가정합니다.
                // 만약 다른 파라미터가 필요하다면 이 부분을 수정해야 합니다.
                skillModule.InitSkill();
            }

            // 새로 생성된 인스턴스를 다시 리스트에 할당합니다.
            skills[i] = newInstance as T;
        }
    }

    public T FindSkill<T>(T skill) where T : SkillBase
    {
        var allSkillModule = activeSkills.Concat(modifiedActiveSkills).Concat(passiveSkills);

        foreach (var module in allSkillModule)
        {
            if (module == null) continue;

            var foundSkill = module.FindInternalSkill<T>();
            if (foundSkill != null)
            {
                return foundSkill;
            }
        }
        return null;
    }

    private void ChangeAirialBasicAttack()
    {
        if (activeSkills.Count == 0 || basicAttacks.Length == 0) return;
        if (unit.isAirial)
        {
            ActiveSkills[0] = basicAttacks[1];
        }
        else
        {
            ActiveSkills[0] = basicAttacks[0];
        }
    }

    private void UsePassiveSkills()
    {
        foreach (var skill in new List<Skill_Module>(activeSkills))
        {
            if (skill.HavePassive)
            {
                skill.ProccessPassive(this);
            }
        }

        foreach (var skill in new List<Skill_Module>(modifiedActiveSkills))
        {
            if (skill.HavePassive)
            {
                skill.ProccessPassive(this);
            }
        }

        foreach (var skill in new List<Skill_Module>(passiveSkills))
        {
            skill.UseSkill(this);
        }
    }

    public bool PerformAttack(Skill_Module skill)
    {
        //if (attNum == -1) return false;
        rigid.velocity = new Vector2(0, rigid.velocity.y);
        if (unit.isAirial) //공중에서 스킬을 쓸 때.
        {
            if (!skill.CanUseAirial) return false;
            else
            {
                bool result = skill.UseSkill(this);
                if (!result) return false;

                // else
                // {
                //     if (skill.BasicAttack) //공중에서 기본 공격을 했을 때.
                //     {
                //         //Debug.Log(Combo);
                //         anim.CrossFade($"BasicAttackAirial_{Combo}", 0f);
                //     }
                //     else
                //     {
                //         anim.CrossFade(skill.AnimName, 0f);
                //     }
                // }
                return true;
            }
        }

        else //땅 위에서 스킬을 쓸 때.
        {
            Debug.Log("할렐루야");
            int comboAnim = Combo;
            bool result = skill.UseSkill(this);
            return true;
            // if (!result)
            // {
            //     Debug.Log("공격 실패");
            //     return false;
            // }

            // else
            // {
            //     if (skill.BasicAttack) //기본 공격을 했을 때.
            //     {
            //         //Debug.Log(Combo);
            //         anim.CrossFade($"BasicAttack_{comboAnim}", 0f);
            //     }
            //     else
            //     {
            //         anim.CrossFade(skill.AnimName, 0f);
            //     }
            //     return true;
            // }
        }
    }

    public void PlayAnimation(string animName)
    {
        anim.CrossFade(animName, 0f);
    }

    private void UpdateCoolDown()
    {
        foreach (var skill in new List<Skill_Module>(activeSkills))
        {
            skill.UpdateCoolDown(Time.deltaTime);
        }

        foreach (var skill in new List<Skill_Module>(modifiedActiveSkills))
        {
            skill.UpdateCoolDown(Time.deltaTime);
        }
    }

    public Vector2 GetPosition()
    {
        return parentObj.transform.position;
    }

    public Vector2 GetDirection()
    {
        return new Vector2(parentObj.transform.localScale.x, 0);
    }

    public Quaternion GetRotation()
    {
        return parentObj.transform.rotation;
    }

    public Transform GetHitBoxPos()
    {
        return this.gameObject.transform.GetChild(0).transform;
    }

    public int GetAttackPower()
    {
        return unit.Att;
    }

    public IDamageable GetDamageable()
    {
        return parentObj.GetComponent<IDamageable>();
    }

    public GameObject GetGameObject()
    {
        return parentObj;
    }

    public Transform GetCatchPos()
    {
        return this.gameObject.transform.GetChild(1).transform;
    }

    public T GetCom<T>() => parentObj.GetComponentInChildren<T>();
}
