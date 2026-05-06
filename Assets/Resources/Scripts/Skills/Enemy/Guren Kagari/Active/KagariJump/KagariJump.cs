using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "KagariJump", menuName = "ScriptableObject/Skills/Active/Guren_Kagari/KagariJump")]
public class KagariJump : SkillBase
{
    //임시 게임오브젝트 타겟.
    private GameObject targetUnit;
    private Unit TargetPlayer;

    [SerializeField] private SkillBase actionSkill;

    public override bool UseSkill(ISkillCaster caster)
    {
        if (TargetPlayer == null) TargetPlayer = LocalGameManager.instance.unitManager.PlayerUnit;

        float targetX = (caster.GetPosition().x + TargetPlayer.gameObject.transform.position.x) / 2f;
        float targetY = caster.GetPosition().y + 5f;

        Vector2 targetPos = new Vector2(targetX, targetY);

        caster.PlayAnimation(animName);
        LocalGameManager.instance.coroutineRunner.StartCoroutine(PerformDash(caster, caster.GetCom<Rigidbody2D>(), caster.GetGameObject().transform, targetPos));

        return true;
    }

    private IEnumerator PerformDash(ISkillCaster caster, Rigidbody2D rigid, Transform casterTransform, Vector2 targetPos)
    {
        caster.GetCom<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

        float duration = .75f;

        Sequence jumpSequence = DOTween.Sequence();

        jumpSequence.Join(caster.GetCom<Transform>().DOMoveX(targetPos.x, duration).SetEase(Ease.Linear));
        jumpSequence.Join(caster.GetCom<Transform>().DOMoveY(targetPos.y, duration).SetEase(Ease.OutQuad));

        yield return jumpSequence.WaitForCompletion();
        // rigid.DOMoveX(targetPos.x, duration).SetEase(Ease.Linear);
        // rigid.DOMoveY(targetPos.y, duration).SetEase(Ease.OutQuad).WaitForCompletion();

        actionSkill?.UseSkill(caster);
    }

}
