using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Profiling;
using UnityEngine;

public abstract class Unit : PoolAble, IDamageable
{
    [Serializable]
    public struct UnitData //유닛들의 모든 데이터.
    {
        public UnitType type;
        public string unitName;
        public int maxHp;
        public int curHp;
        public int att;
        //public int def;
        public int maxGage;
        public int curGage;
        public float moveSpeed;
        public int jumpForce;
        public int addJumpCount;
        public float attSpeed;

        public UnitData(Datas datas)
        {
            type = datas.Type;
            unitName = datas.Name;
            maxHp = datas.Hp;
            curHp = maxHp;
            att = datas.Att;
            //def = datas.Def;
            maxGage = datas.Gage;
            curGage = 0;
            moveSpeed = datas.MoveSpeed;
            jumpForce = datas.JumpForce;
            addJumpCount = datas.AddJumpCount;
            attSpeed = datas.AttSpeed;
        }
    }

    [SerializeField] protected UnitData unitData;
    [SerializeField] private float dmgRate = 1; //받는 피해 비율.
    [SerializeField] private float givingDmgRate = 0; //주는 피해 비율.
    private bool cantAction; //true일 경우 행동 불능.
    public bool graceState; //무적 상태.
    private bool hpRegain;
    private bool gageRegain;

    #region Property
    public float DmgRate { get => dmgRate; set => dmgRate = value; }
    public float GivingDmgRate { get => givingDmgRate; set => givingDmgRate = value; }
    public int MaxHp { get => unitData.maxHp; set => unitData.maxHp = Mathf.Max(0, value); }
    public int CurHp { get => unitData.curHp; set => unitData.curHp = Mathf.Max(0, value); }

    public int Att { get => unitData.att; set => unitData.att = Mathf.Max(0, value); }
    public float AttSpeed { get => unitData.attSpeed; set => unitData.attSpeed = Mathf.Max(0, value); }
    //public int Def { get => unitData.def; set => unitData.def = Mathf.Max(0, value); }
    public int MaxGage { get => unitData.maxGage; set => unitData.maxGage = value; }
    public int curGage { get => unitData.curGage; set => unitData.curGage = value; }
    public float MoveSpeed { get => unitData.moveSpeed; set => unitData.moveSpeed = Mathf.Max(0, value); }
    public int JumpForce { get => unitData.jumpForce; set => unitData.jumpForce = Mathf.Max(0, value); }
    public int AddJumpCount { get => unitData.addJumpCount; set => unitData.addJumpCount = Mathf.Max(0, value); }
    public bool isDead => CurHp <= 0;
    public bool GraceState { get => graceState; set => graceState = value; }
    public UnitType Type => unitData.type;

    public bool CantAction
    {
        get { return cantAction; }
        set
        {
            if (cantAction != value)
            {
                cantAction = value;
                CheckUnitAction(cantAction);
            }
        }
    }

    public bool HpRegain { get { return hpRegain; } set { hpRegain = value; } }
    public bool GageRegain { get { return gageRegain; } set { gageRegain = value; } }

    #endregion

    public event Action<int, ISkillCaster, GameObject> TakeDamageEvent; //데미지를 받을 때 실행하는 이벤트 (받은 데미지, 데미지를 준 공격자와 피격자의 게임 오브젝트 인자.)
    public event Action<ISkillCaster> DeadEvent; //이 캐릭터가 사망할 때 실행하는 이벤트, 아직은 인자값이 필요 없어 보임.
    public Dictionary<string, StatusEffectBase> activeEffect = new Dictionary<string, StatusEffectBase>(); //유닛에 진행중인 상태 이상들. 버프/디버프 등
    public Dictionary<string, Coroutine> activeEffectCoroutines = new Dictionary<string, Coroutine>(); //상태이상 지속을 돕는 코루틴.
    private Coroutine newCorutine;
    public bool isAirial;
    [SerializeField] private GameObject textBox; //이 유닛의 말풍선에 사용할 텍스트 박스.
    //public UnitData currentStats;

    void Update()
    {
        //디버깅 용
        Debug.Log($"현재 주는 데미지 비율 : {GivingDmgRate}");
    }

    void OnEnable()
    {
        //currentStats = unitData;
        if (GameManager.instance is not null)
        {
            GameManager.instance.eventManager.InitChar += SetUnitInDialogue;
        }
    }

    void OnDisable()
    {
        if (GameManager.instance is not null)
        {
            GameManager.instance.eventManager.InitChar -= SetUnitInDialogue;
        }
    }

    public void SetUnitInDialogue(string[] name)
    {
        if (name.Contains(this.gameObject.name))
        {
            Debug.Log(this.gameObject.name);
            GameManager.instance.dialogueRunner.DialogueTextDic.Add(this.gameObject.name, textBox.GetComponentInChildren<TextMeshProUGUI>());
        }
    }

    public void TakeDamage(int dmg, ISkillCaster attacker, GameObject character)
    {
        if (GraceState) return;
        Debug.Log($"데미지 받음! 받은 데미지 : {dmg}");

        //데미지를 받을 때마다 피격 효과 발생.
        TakeDamageEvent?.Invoke(dmg, attacker, character);

        CurHp -= (int)(dmg * dmgRate);// - Def);
        if (isDead) Dead(attacker);
    }

    public virtual void Dead(ISkillCaster attacker)
    {
        //사망 시 적용되는 이벤트 실행
        GameManager.instance.eventManager.ReportDead(attacker, this.gameObject);
        Debug.Log("사망!");
        this.gameObject.SetActive(false);
    }

    private void CheckUnitAction(bool cantAction)
    {
        GameObject attack = transform.GetChild(1).gameObject;
        GameObject movement = transform.GetChild(2).gameObject;

        if (cantAction)
        {
            attack.SetActive(false);
            movement.SetActive(false);
        }
        else
        {
            attack.SetActive(true);
            movement.SetActive(true);
        }
    }

    public void AddEffectProcess(StatusEffectBase effect)
    {
        if (!this.gameObject.activeSelf) return;
        ApplyEffect(effect);
    }

    private void ApplyEffect(StatusEffectBase effect) //상태 이상 적용.
    {
        //적용하려는 상태이상이 이미 존재하고 있는 경우. 적용되어 있는 상태이상 제거 후 다시 재적용.
        if (activeEffect.TryGetValue(effect.effectName, out StatusEffectBase exisitngEffect))
        {
            if (activeEffectCoroutines.TryGetValue(effect.effectName, out Coroutine runningCoroutine))
            {
                //코루틴을 이용한 상태이상의 타이머 제거.
                GameManager.instance.coroutineRunner.StopCoroutine(runningCoroutine);
            }
            //적용 되어 있는 상태이상 또한 제거.
            exisitngEffect.RemoveEffect(true);
        }

        activeEffect[effect.effectName] = effect;
        effect.ApplyEffect();

        if (effect.duration > 0)
        {
            Coroutine newCoroutine = GameManager.instance.coroutineRunner.StartRunnerCoroutine(RemoveEffectAfterDuration(effect));
            activeEffectCoroutines[effect.effectName] = newCoroutine;
        }
    }

    public void RemoveEffect(string effectName)
    {
        if (activeEffect.TryGetValue(effectName, out StatusEffectBase effect))
        {
            if (activeEffectCoroutines.TryGetValue(effectName, out Coroutine runningCoroutine))
            {
                GameManager.instance.coroutineRunner.StopCoroutine(runningCoroutine);
                activeEffectCoroutines.Remove(effectName);
            }

            effect.RemoveEffect();
            activeEffect.Remove(effectName);
        }
    }

    public bool FindEffect(string effectName) //해당 이름의 상태 이상이 존재하는지 체크.
    {
        return activeEffect.ContainsKey(effectName);
    }

    IEnumerator RemoveEffectAfterDuration(StatusEffectBase effect) //상태 이상 삭제.
    {
        yield return new WaitForSeconds(effect.duration);
        effect.RemoveEffect();
        activeEffect.Remove(effect.effectName);
        activeEffectCoroutines.Remove(effect.effectName);
        //GameManager.instance.uIManager.combatUI.RemoveEffectUI(effect.effectName);
        //GameManager.instance.uIManager.combatUI.UpdateEffectUI();
    }
}
