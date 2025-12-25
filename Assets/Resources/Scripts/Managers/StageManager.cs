using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public int totalCount;

    public void StageStart(List<int> basicEnemyList, List<int> eliteEnemyList)
    {
        totalCount = basicEnemyList.Count + eliteEnemyList.Count; //총 Enemy수.

        foreach (var enemyId in basicEnemyList)
        {
            //일반 몬스터 소환.
            var unit = GameManager.instance.objectPoolManager.GetGo("Unit");
            unit.GetComponent<EnemyCharacter>().id = enemyId;
        }

        foreach (var eliteId in eliteEnemyList)
        {
            //엘리트 몬스터 소환.
            var unit = GameManager.instance.objectPoolManager.GetGo("Unit");
            unit.GetComponent<EnemyCharacter>().id = eliteId;
        }
    }

    public void StageClear()
    {

    }
}
