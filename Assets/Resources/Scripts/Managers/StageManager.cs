using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] private List<StageEnemyInfo> enemyLists = new List<StageEnemyInfo>();
    [SerializeField] private List<Vector2> enemySpawnPoint = new List<Vector2>();
    [SerializeField] private List<Vector2> eliteSpawnPoint = new List<Vector2>();

    public int totalCount;

    public void StageStart(List<int> enemyList)
    {
        totalCount = enemyList.Count; //총 Enemy수.

        foreach (var enemyId in enemyList)
        {
            //일반 몬스터 소환.
            var enemy = GameManager.instance.objectPoolManager.GetGo("Unit");
            var enemyCom = enemy.GetComponent<EnemyCharacter>();

            enemyCom.id = enemyId;
            enemy.tag = "Enemy";

            if (enemyCom.Type == UnitType.Enemy)
            {
                //일반 몬스터 스폰 위치에 몬스터 이동.
                enemy.transform.position = enemySpawnPoint[Random.Range(0, enemySpawnPoint.Count)];
            }
            else if (enemyCom.Type == UnitType.Elite)
            {
                //엘리트 몬스터 스폰 위치에 몬스터 이동.
                enemy.transform.position = eliteSpawnPoint[Random.Range(0, eliteSpawnPoint.Count)];
            }
        }
    }

    public void StageClear()
    {

    }
}
