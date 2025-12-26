using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitType { PlayableCharacter, Enemy, Elite, Boss }

[Serializable]
public class Datas
{
    [SerializeField] private UnitType type;
    [SerializeField] private int id;

    [SerializeField] private string name;
    [SerializeField] private int hp;
    [SerializeField] private int att;
    //[SerializeField] private int def;
    [SerializeField] private int gage;
    [SerializeField] private int moveSpeed;
    [SerializeField] private int jumpForce;
    [SerializeField] private int addJumpCount;
    [SerializeField] private float attSpeed;

    public UnitType Type => type;
    public int Id => id;
    public string Name => name;
    public int Hp => hp;
    public int Att => att;
    //public int Def => def;
    public int Gage => gage;
    public int MoveSpeed => moveSpeed;
    public int JumpForce => jumpForce;
    public int AddJumpCount => addJumpCount;
    public float AttSpeed => attSpeed;
}

[CreateAssetMenu(fileName = "CharacterMap", menuName = "ScriptableObject/DataMaps/PlayableCharacter/CharcterMap")]
public class UnitDataMap : ScriptableObject
{
    public Datas[] datas;
    public Datas GetDatas(int id)
    {
        foreach (var data in datas)
        {
            if (id == data.Id)
            {
                return data;
            }
        }

        Debug.LogWarning($"ID : {id} 에 해당하는 캐릭터 정보가 존재하지 않습니다.");
        return null;
    }
}
