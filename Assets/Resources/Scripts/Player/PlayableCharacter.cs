using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayableCharacter : Unit
{
    public enum ControlState { Player, AI }

    [Header("플레이 가능한 캐릭터들의 기본 정보")]
    [SerializeField] private UnitDataMap maps;

    [Header("캐릭터 ID")]
    [SerializeField] private int id;

    public ControlState controlState;

    [Header("Player UI Element")]
    [SerializeField] private Slider hpBar;
    [SerializeField] private Slider ellBar;
    [SerializeField] private Slider[] skillCools = new Slider[4];

    private void Start()
    {
        CheckingControlState(controlState);
        DataInit();
    }

    void FixedUpdate()
    {
        if (curGage < MaxGage)
        {
            curGage += Time.deltaTime * 5;
            UpdateEllBar();
        }
    }

    private void DataInit()
    {
        unitData = new UnitData(maps.GetDatas(id));

        hpBar = LocalGameManager.instance.playerUIManager.HpBar;
        ellBar = LocalGameManager.instance.playerUIManager.EllBar;

        skillCools = LocalGameManager.instance.playerUIManager.SkillCools;

        base.TakeDamageEvent += UpdateHpBar;
        GetComponentInChildren<Attack>().UpdateSkillGage += UpdateEllBar;

        foreach (var init in GetComponentsInChildren<IDataInitializable>())
        {
            init.DataInit();
        }
    }

    private void UpdateHpBar(int dmg, ISkillCaster caster, GameObject obj)
    {
        hpBar.value = CurHp / (float)MaxHp;
    }

    private void UpdateEllBar()
    {
        ellBar.value = curGage / MaxGage;
    }

    public void CheckingControlState(ControlState state)
    {
        switch (state)
        {
            case ControlState.Player:
                GetComponentInChildren<PlayerController>().enabled = true;
                break;

            case ControlState.AI:
                GetComponentInChildren<PlayerController>().enabled = false;
                break;
        }
    }
}
