using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using Unity.VisualScripting;

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

    [Header("해당 스테이지에서 보상으로 등장할 유물 번호와 등장 가중치")]
    public SerializedDictionary<int, int> RelicWeights = new SerializedDictionary<int, int>();

    [Header("모든 유물 맵")]
    [SerializeField] private Relic_Dic relicMap;

    private int SetTotalWeight()
    {
        int total = 0;
        foreach (var relic in RelicWeights)
        {
            total += relic.Value;
        }
        return total;
    }

    public RelicInfo SetRelic()
    {
        int total = SetTotalWeight();
        int weight = 0;
        int num;

        num = Random.Range(0, total) + 1;

        for (int i = 1; i <= RelicWeights.Count; i++)
        {
            weight += RelicWeights[i];
            if (num <= weight)
            {
                RelicInfo temp = relicMap.relic[i];
                return temp;
            }
        }
        return null;
    }
}
