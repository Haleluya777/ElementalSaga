using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FlowIncrease", menuName = "ScriptableObject/Skills/Passive/YunSeoYeon/FlowIncrease")]
public class FlowIncrease : OnHitEventBase
{
    [SerializeField] private Flow flow;
    public override void Execute(GameObject target, ISkillCaster caster, int dmg)
    {
        //if (flow.flowGage >= 6) return;
        //flow.flowGage++;
    }
}
