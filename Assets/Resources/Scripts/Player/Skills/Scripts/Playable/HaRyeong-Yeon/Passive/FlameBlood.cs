using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FlameBlood", menuName = "ScriptableObject/Skills/Passive/YeonHaRyeong/FlameBlood")]
public class FlameBlood : SkillBase
{
    [SerializeField] private Skill_Module reinforcedSkill;
    [SerializeField] private Skill_Module basicSkill;

    private Unit unit;
    private IAttackable attack;
    public int flameBloodGage = 0;
    private int previousflameBlood = 0;

    // 유닛의 원본 스탯을 저장할 변수들
    private bool statsCaptured = false;
    private int baseAtt;
    private float baseMoveSpeed;
    private int baseMaxHp;

    public override void Initialize(Skill_Module module)
    {
        base.Initialize(module);
        statsCaptured = false;

        reinforcedSkill = Instantiate(reinforcedSkill);
        reinforcedSkill.InitSkill();
        basicSkill = Instantiate(basicSkill);
        basicSkill.InitSkill();
    }

    public override bool UseSkill(ISkillCaster caster)
    {
        if (unit == null)
        {
            unit = caster.GetGameObject().GetComponent<Unit>();
            attack = unit.GetComponent<IAttackable>();
        }

        if (unit != null && !statsCaptured)
        {
            baseAtt = unit.Att;
            baseMoveSpeed = unit.MoveSpeed;
            baseMaxHp = unit.MaxHp;
            statsCaptured = true;
        }

        flameBloodGage = Mathf.Clamp(flameBloodGage, 0, 50);
        if (flameBloodGage != previousflameBlood)
        {
            Debug.Log("뜌댜");
            if (flameBloodGage >= 25 && flameBloodGage < 40)
            {
                Debug.Log("자연치유 불가.");
                unit.HpRegain = false;
                unit.DmgRate = 1f;
            }

            else if (flameBloodGage >= 40 && flameBloodGage < 50)
            {
                Debug.Log("받는 피해 증가 및 자연 치유 불가.");
                unit.HpRegain = false;
                unit.DmgRate += .25f;
            }

            else if (flameBloodGage >= 50)
            {
                Debug.Log("폭혈! 터져라!");
                flameBloodGage = 0;
                unit.AddEffectProcess(new Buff_ExplosionBlood(6f, unit, 150, 50, "ExplosionBlood", basicSkill, reinforcedSkill));
                unit.HpRegain = true;
                unit.DmgRate = 1;
            }

            UpdateStats(flameBloodGage);
            previousflameBlood = flameBloodGage;
        }

        return true;
    }

    private void UpdateStats(int gage)
    {
        // 항상 원본 스탯을 기준으로 현재 heatPressure에 맞는 스탯을 계산하여 적용합니다.
        unit.Att = baseAtt + (baseAtt * 2 / 100) * gage;
        unit.MoveSpeed = baseMoveSpeed + (baseMoveSpeed * 1 / 100) * gage;
        unit.MaxHp = baseMaxHp - (baseMaxHp * 5 / 1000) * gage;

        Debug.Log($"스탯 업데이트: 염혈:({gage}) -> 공격력:({unit.Att}), 이동 속도:({unit.MoveSpeed}), 최대 체력:({unit.MaxHp})");
    }
}
