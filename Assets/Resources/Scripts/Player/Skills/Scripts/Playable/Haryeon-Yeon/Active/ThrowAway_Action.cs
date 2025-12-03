using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ThrowAway_Action", menuName = "ScriptableObject/Skills/Active/YeonHaRyeon/ThrowAway_Action")]
public class ThrowAway_Action : SkillBase
{
    private RaycastHit2D hitted;
    private GameObject hitBox;
    private GameObject effect;

    public override bool UseSkill(ISkillCaster caster)
    {
        hitted = Physics2D.Raycast(new Vector2(caster.GetPosition().x, caster.GetPosition().y + .5f), caster.GetDirection(), 5f, 1 << 6);

        hitBox = GameManager.instance.objectPoolManager.GetGo("HitBox");
        effect = GameManager.instance.objectPoolManager.GetGo("Effect");

        hitBox.transform.position = hitted.collider.transform.position;
        effect.transform.position = hitted.collider.transform.position;

        return true;
    }
}
