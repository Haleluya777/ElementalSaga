using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ThrowAway_Action", menuName = "ScriptableObject/Skills/Active/YeonHaRyeon/ThrowAway_Action")]
public class ThrowAway_Action : SkillBase
{
    [SerializeField] private Vector2 hitBoxOffset;
    [SerializeField] private Vector2 hitBoxSize;

    private RaycastHit2D hitted;
    private GameObject hitBox;
    private GameObject effect;

    public override bool UseSkill(ISkillCaster caster, Vector3 skillPos)
    {
        //Debug.Log("배애앰");
        //hitted = Physics2D.Raycast(new Vector2(caster.GetPosition().x, caster.GetPosition().y + .5f), caster.GetDirection(), 5f, 1 << 6);

        hitBox = GameManager.instance.objectPoolManager.GetGo("HitBox");
        effect = GameManager.instance.objectPoolManager.GetGo("Effect");

        hitBox.transform.position = skillPos;
        effect.transform.position = skillPos;

        HitBox hitBoxCom = hitBox.GetComponent<HitBox>();
        Effect effectCom = effect.GetComponent<Effect>();

        hitBoxCom.GetComponent<BoxCollider2D>().size = hitBoxSize;
        hitBoxCom.GetComponent<BoxCollider2D>().offset = hitBoxOffset;

        hitBoxCom.Initialize(dmgCalculater.Calculate(caster), caster, null, .5f);
        effectCom.Initialize(.5f);

        return true;
    }
}
