using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "FireSlashJump", menuName = "ScriptableObject/Skills/Active/Guren_Kagari/FireSlashJump")]
public class FireSlashJump : SkillBase
{
    private Unit targetUnit;

    [SerializeField] private float delayTime;
    [SerializeField] private SkillBase actionSkill;
    [SerializeField] private Vector2 dangerAreaPos;
    [SerializeField] private Vector2 dangerAreaSize;

    public override bool UseSkill(ISkillCaster caster)
    {
        if (targetUnit == null) targetUnit = LocalGameManager.instance.unitManager.PlayerUnit;

        float targetX = (caster.GetPosition().x - targetUnit.gameObject.transform.position.x) / 2f;
        float targetY = caster.GetPosition().y + 3f;

        Vector2 targetPos = new Vector2(targetX, targetY);

        caster.PlayAnimation(animName);
        LocalGameManager.instance.coroutineRunner.StartCoroutine(PerformDash(caster, caster.GetCom<Rigidbody2D>(), caster.GetGameObject().transform, targetPos));

        return true;
    }

    private IEnumerator PerformDash(ISkillCaster caster, Rigidbody2D rigid, Transform casterTransform, Vector2 tagetPos)
    {
        caster.GetCom<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

        float dashSpeed = 35f; // 대쉬 속도
        float minSqrDistance = .5f;

        while (((Vector2)tagetPos - rigid.position).magnitude > minSqrDistance)
        {
            if (Physics2D.Raycast(new Vector2(caster.GetGameObject().transform.position.x, caster.GetGameObject().transform.position.y + .5f), Vector2.right * caster.GetGameObject().transform.localScale.x, .75f, 1 << 3))
            {
                break;
            }

            Vector2 direction = ((Vector2)tagetPos - rigid.position).normalized;
            Vector3 newPos = rigid.position + direction * (dashSpeed / 10) * Time.fixedDeltaTime;

            rigid.MovePosition(newPos);

            yield return new WaitForFixedUpdate();
        }

        GameObject dangerArea = LocalGameManager.instance.objectPoolManager.poolDic["DangerArea"].GetGo("DangerAreaX");

        dangerArea.transform.SetParent(caster.GetGameObject().transform.GetChild(2).transform.GetChild(0));
        dangerArea.transform.localPosition = dangerAreaPos;
        dangerArea.transform.localScale = new Vector2(caster.GetDirection().x * -1 * dangerAreaSize.x, dangerAreaSize.y);

        var dangerAreaCom = dangerArea.GetComponent<DangerArea>();

        // Activate 호출 시 콜백을 인자로 넘겨주어 이벤트 누적 문제를 방지합니다.
        dangerAreaCom.Activate(delayTime, () => actionSkill?.UseSkill(caster));

        yield return null;
    }

}
