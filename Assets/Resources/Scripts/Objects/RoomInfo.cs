using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using Unity.VisualScripting;
using Cinemachine;

public class RoomInfo : PoolAble
{
    [Header("현재 방 타입")]
    public RoomType type;

    [Header("등장할 적 종류.")]
    public List<string> enemyList = new List<string>(); //등장할 적 이름들.

    [Header("일반 적이 등장할 위치.")]
    public List<Transform> enemyPos = new List<Transform>();

    [Header("엘리트 적이 등장할 위치.")]
    public List<Transform> elitePos = new List<Transform>();

    [Header("스테이지 클리어 후 다음 스테이지로 진행하게 하는 문이 등장할 위치.")]
    public Transform[] doorPos = new Transform[2];

    [Header("해당 스테이지에서 등장할 보상 상자와 등장 가중치")]
    public SerializedDictionary<AmendChest.RelicTier, int> ChestWeights = new SerializedDictionary<AmendChest.RelicTier, int>();

    [Header("해당 스테이지에서 확정적으로 등장할 유물 리스트")]
    public List<RelicInfo> confirmedRelics = new List<RelicInfo>();

    [Header("해당 스테이지가 상점 스테이질 경우, 등장할 유물 목록")]
    public List<RelicInfo> shopRelics = new List<RelicInfo>();

    [Header("해당 스테이지가 상점 스테이지일 경우, 등장할 3,2,1등급 유물 개수")]
    public List<int> counts = new List<int>();

    private int SetTotalWeight()
    {
        int total = 0;
        foreach (var chest in ChestWeights)
        {
            total += chest.Value;
        }
        return total;
    }

    public void SetShopRelics()
    {
        for (int i = 0; i < 3; i++)
        {
            //shopRelics.AddRange(GameManager.instance.relicManager.relicMaps[i].GetRandomRelicList(counts[i]));
        }

        GameManager.instance.canvasManager.SetShopPanel(shopRelics);
    }

    public AmendChest.RelicTier SetChestTier()
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
        return AmendChest.RelicTier.Bronze;
    }
}
