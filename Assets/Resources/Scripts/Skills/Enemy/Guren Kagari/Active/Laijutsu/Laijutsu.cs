using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Laijutsu", menuName = "ScriptableObject/Skills/Active/Guren_Kagari/Laijutsu")]
public class Laijutsu : SkillBase
{
    [SerializeField] private float delayTime;
    [SerializeField] private SkillBase actionSkill;


    public override bool UseSkill(ISkillCaster caster)
    {
        GameObject dangerArea = LocalGameManager.instance.objectPoolManager.poolDic["DangerArea"].GetGo("DangerAreaX");

        dangerArea.transform.position = caster.GetHitBoxPos().position;
        dangerArea.transform.SetParent(caster.GetGameObject().transform.GetChild(2).transform.GetChild(0));
        dangerArea.transform.localScale = new Vector2(caster.GetDirection().x * -1, 1);

        var dangerAreaCom = dangerArea.GetComponent<DangerArea>();

        // Activate 호출 시 콜백을 인자로 넘겨주어 이벤트 누적 문제를 방지합니다.
        dangerAreaCom.Activate(delayTime, () => actionSkill?.UseSkill(caster));

        return true;
    }
}
