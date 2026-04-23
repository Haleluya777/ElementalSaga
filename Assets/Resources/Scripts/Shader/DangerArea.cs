using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class DangerArea : PoolAble
{
    public event Action ActiveSkill;
    private Material _material;
    private static readonly int FillAmountId = Shader.PropertyToID("_FillAmount");

    void Awake()
    {
        // 머티리얼 인스턴스를 생성하여 개별 오브젝트마다 독립적인 제어가 가능하게 합니다.
        var renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            _material = renderer.material;
        }
        //Activate(1f);
    }

    public void Activate(float duration, Action onActive = null)
    {
        if (_material == null) return;

        // 기존에 등록된 모든 이벤트를 제거하고, 새로 전달받은 액션만 등록하거나 비웁니다.
        ActiveSkill = onActive;

        // 기존에 실행 중인 트윈이 있다면 중지합니다.
        _material.DOKill();

        // 1. 초기 상태 설정 (1.0 = 비어있음)
        _material.SetFloat(FillAmountId, 1f);

        // 2. DOTween을 사용하여 _FillAmount를 0으로 선형적으로 변화시킵니다.
        _material.DOFloat(0f, FillAmountId, duration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                // 채워지기가 완료되었을 때 실행할 로직
                Debug.Log("공격 범위 충전 완료!");
                ActiveSkill?.Invoke();
                ActiveSkill = null; // 실행 후 초기화 (풀링 대비)
                this.ReleaseObject();
            });
    }
}
