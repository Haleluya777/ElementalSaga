using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : PoolAble
{
    private float limitTime; //오브젝트가 생성된 후 몇 초 뒤에 비활성화 될지 정하는 시간.

    public void Initialize(float _limitTime)
    {
        limitTime = _limitTime;
        GameManager.instance.coroutineRunner.StartCoroutine(ReturnToPool());
    }

    private IEnumerator ReturnToPool()
    {
        yield return new WaitForSeconds(limitTime);
        ReleaseObject();
    }
}
