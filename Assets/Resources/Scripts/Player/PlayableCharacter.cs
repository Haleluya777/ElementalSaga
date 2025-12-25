using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableCharacter : Unit
{
    public enum ControlState { Player, AI }

    [Header("플레이 가능한 캐릭터들의 기본 정보")]
    [SerializeField] private UnitDataMap maps;

    [Header("캐릭터 ID")]
    [SerializeField] private int id;

    public ControlState controlState;

    private void Start()
    {
        CheckingControlState(controlState);
        DataInit();
    }

    private void DataInit()
    {
        unitData = new UnitData(maps.GetDatas(id));
        foreach (var init in GetComponentsInChildren<IDataInitializable>())
        {
            init.DataInit();
        }
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
