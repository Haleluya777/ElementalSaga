using UnityEngine;

public interface ISkillCaster
{
    int TotalDmg { get; set; }
    bool Attacking { get; set; }
    bool CancleAllSkill { get; }

    //int CurrentGage { get; set; }
    //string GetTag();

    //void SetScale(int dir);
    Vector2 GetPosition();
    Vector2 GetDirection();

    Quaternion GetRotation();

    int GetAttackPower();

    IDamageable GetDamageable();
    GameObject GetGameObject();
    Transform GetHitBoxPos();
    Transform GetCatchPos(); //잡기할 때 Enemy의 위치.

    T GetCom<T>();
}
