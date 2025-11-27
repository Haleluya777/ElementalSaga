using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour, IAttackable, ISkillCaster, IDataInitializable
{
    public event Action<int> OnHitEvent;

    [SerializeField] private GameObject parentObj;
    [SerializeField] private BoxCollider2D hitBox;
    private Unit unit;
    private Animator anim;

    [Header("특정 키를 눌러 사용하는 액티브 스킬.")] //기본 공격도 포함.
    [SerializeField] private List<Skill_Module> activeSkills = new List<Skill_Module>();

    [Header("특정 조건을 만족하면 자동으로 사용되는 패시브 스킬")]
    [SerializeField] private List<Skill_Module> passiveSkills = new List<Skill_Module>();

    [Header("맞을 때 실행할 효과들")]
    //맞을 때 실행할 효과들은 공격자의 정보가 필요한 경우도 존재하므로, 새로운 클래스를 만들어 사용.
    [SerializeField] private List<DamagedEventBase> hitEvents = new List<DamagedEventBase>();

    private int combo;

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
    }

    public void DataInit()
    {
        unit = parentObj.GetComponent<Unit>();
        anim = parentObj.GetComponent<Animator>();

        for (int i = 0; i < hitEvents.Count; i++)
        {
            if (hitEvents[i] == null) break;
            hitEvents[i] = Instantiate(hitEvents[i]);
            hitEvents[i].Initialize(unit);
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
        if (!activeSkills[attNum].UseSkill(this)) return false;
        else
        {
            if (attNum == 1) //기본 공격을 했을 때.
            {
                anim.CrossFade($"BasicAttack_{combo}", 0f);
            }
            else
            {
                anim.CrossFade($"Skill_{attNum}", 0f);
            }
            return true;
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

    public T GetCom<T>() => parentObj.GetComponentInChildren<T>();
}
