using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using Unity.VisualScripting;
using Cinemachine;

public class RoomInfo : PoolAble
{
    [Header("등장할 적 종류.")]
    public List<int> enemyList = new List<int>(); //등장할 적 종류

    [Header("일반 적이 등장할 위치.")]
    public List<Transform> enemyPos = new List<Transform>();

    [Header("엘리트 적이 등장할 위치.")]
    public List<Transform> elitePos = new List<Transform>();

    [Header("스테이지 클리어 후 다음 스테이지로 진행하게 하는 문이 등장할 위치.")]
    public Transform[] doorPos = new Transform[2];

    [Header("해당 스테이지에서 등장할 보상 상자와 등장 가중치")]
    public SerializedDictionary<AmendChest.ChestTier, int> ChestWeights = new SerializedDictionary<AmendChest.ChestTier, int>();

    [Header("해당 스테이지에서 확정적으로 등장할 유물 리스트")]
    public List<RelicInfo> confirmedRelics = new List<RelicInfo>();

    private int SetTotalWeight()
    {
        int total = 0;
        foreach (var chest in ChestWeights)
        {
            total += chest.Value;
        }
        return total;
    }

    public AmendChest.ChestTier SetChestTier()
    {
        int total = SetTotalWeight();
        int weight = 0;
        int num;

        num = Random.Range(0, total) + 1;
        foreach (var chest in ChestWeights)
        {
            weight += chest.Value;
            if (num <= weight)
            {
                return chest.Key;
            }
        }
        return AmendChest.ChestTier.Bronze;
    }
}
