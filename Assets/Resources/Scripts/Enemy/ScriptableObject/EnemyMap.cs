using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyDatas
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

[CreateAssetMenu(fileName = "EnemyMap", menuName = "ScriptableObject/DataMaps/Enemy/EnemyMap")]
public class EnemyMap : ScriptableObject
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
