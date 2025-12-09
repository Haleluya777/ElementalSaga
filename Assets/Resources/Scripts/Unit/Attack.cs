using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
    [SerializeField] private List<Skill_Module> activeSkills = new List<Skill_Module>();

    [Header("특정 조건을 만족하면 자동으로 사용되는 패시브 스킬")]
    [SerializeField] private List<Skill_Module> passiveSkills = new List<Skill_Module>();

    [Header("맞을 때 실행할 효과들")]
    //맞을 때 실행할 효과들은 공격자의 정보가 필요한 경우도 존재하므로, 새로운 클래스를 만들어 사용.
    [SerializeField] private List<DamagedEventBase> hitEvents = new List<DamagedEventBase>();

    [SerializeField] private int combo;

    public int TotalDmg { set; get; }
    public bool Attacking { set; get; }
    public bool CancleAllSkill { get; }
    public int Combo { get => combo; set => combo = value; }

    public List<Skill_Module> ActiveSkills { get => activeSkills; set => activeSkills = value; }
    public List<Skill_Module> PassiveSkills { get => passiveSkills; set => passiveSkills = value; }
    public List<DamagedEventBase> HitEvents { get => hitEvents; set => hitEvents = value; }

    void Update()
    {
        UpdateCoolDown();
        UsePassiveSkills();
        //ChangeAirialBasicAttack();
    }

    public void DataInit()
    {
        //if (parentObj.GetComponent<PlayableCharacter>().controlState == PlayableCharacter.ControlState.Player)
        //{
        //    controller = parentObj.GetComponentInChildren<PlayerController>();
        //    //controller.attackInput += PerformAttack;
        //}

        unit = parentObj.GetComponent<Unit>();
        anim = parentObj.GetComponent<Animator>();
        rigid = parentObj.GetComponent<Rigidbody2D>();

        for (int i = 0; i < hitEvents.Count; i++)
        {
            if (hitEvents[i] == null) break;
            hitEvents[i] = Instantiate(hitEvents[i]);
            hitEvents[i].Initialize(unit);
        }

        foreach (var skill in activeSkills)
        {
            skill.InitSkill();
        }

        foreach (var skill in passiveSkills)
        {
            skill.InitSkill();
        }
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
        foreach (var skill in new List<Skill_Module>(passiveSkills))
        {
            skill.UseSkill(this);
        }
    }

    public bool PerformAttack(int attNum)
    {
        if (attNum == -1) return false;
        rigid.velocity = new Vector2(0, rigid.velocity.y);
        if (unit.isAirial) //공중에서 스킬을 쓸 때.
        {
            Debug.Log("할렐루야");
            if (!activeSkills[attNum].CanUseAirial) return false;
            else
            {
                bool result = activeSkills[attNum].UseSkill(this);
                if (!result) return false;

                else
                {
                    if (attNum == 0) //공중에서 기본 공격을 했을 때.
                    {
                        //Debug.Log(Combo);
                        anim.CrossFade($"BasicAttackAirial_{Combo}", 0f);
                    }
                    else
                    {
                        anim.CrossFade($"AirialSkill_{attNum}", 0f);
                    }
                }
                return true;
            }
        }

        else //땅 위에서 스킬을 쓸 때.
        {


            int comboAnim = Combo;
            bool result = activeSkills[attNum].UseSkill(this);

            if (!result)
            {

                return false;
            }

            else
            {
                if (attNum == 0) //기본 공격을 했을 때.
                {
                    //Debug.Log(Combo);
                    anim.CrossFade($"BasicAttack_{comboAnim}", 0f);
                }
                else
                {
                    anim.CrossFade($"Skill_{attNum}", 0f);
                }
                return true;
            }
        }
    }

    private void UpdateCoolDown()
    {
        foreach (var skill in new List<Skill_Module>(activeSkills))
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
