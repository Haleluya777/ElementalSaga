using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "GageDownEvent", menuName = "ScriptableObject/Skills/GageDownEvent")]
public class GageDownEvent : SkillBase
{
    [SerializeField] private SkillBase reduceGage;
    [SerializeField] private float limitedTime; //제한 시간.
    private float timer;
    private bool eventRunning = false;

    public override bool UseSkill(ISkillCaster caster)
    {
        if (eventRunning) return false;

        timer += Time.deltaTime;

        if (timer >= 10f)
        {
            timer -= 10f;
            Debug.Log("이벤트 시작~");
            GameManager.instance.coroutineRunner.StartRunnerCoroutine(TimingEvent(caster));
        }
        return true;
    }

    private IEnumerator TimingEvent(ISkillCaster caster)
    {
        eventRunning = true;
        try
        {
            const float duration = 3f; //타이밍 이벤트 진행 시간
            const float timeStart = 2.5f; //성공 판정 시간.
            float time = 0f; //경과 시간

            while (time < duration)
            {
                if (Input.GetKeyDown(KeyCode.A))
                {
                    if (time >= timeStart)
                    {
                        Debug.Log($"성공!, {time}초");
                    }
                    else
                    {
                        Debug.Log($"실패!, {time}초");
                    }
                    yield break;
                }
                time += Time.deltaTime;
                yield return null;
            }
            Debug.Log("입력 없음");
        }

        finally
        {
            eventRunning = false;
        }
    }
}
