using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

[System.Serializable]
public struct FloorDetail
{
    public int floorNum;
    public string floorDetail;
}

[System.Serializable]
public class CharacterData
{
    public int id;
    public string characterName; //캐릭터 이름
    public SerializedDictionary_Emotion characterSpriteMap; //캐릭터 스프라이트

}

[CreateAssetMenu(fileName = "CharacterUISpritrMap", menuName = "ScriptableObject/Character/CharacterUISpritrMap")]
public class CharacterMap : ScriptableObject
{
    public CharacterData[] characters; //캐릭터 데이터

    public CharacterData GetCharacter(int index)
    {
        //if (index < 0 || index > characters.Length)
        //{
        //    Debug.Log($"유효하지 않은 캐릭터 인덱스. Index : {index}");
        //    return null;
        //}
        foreach (var character in characters)
        {
            if (character.id == index) return character;
        }
        Debug.Log($"ID값 {index}를 가진 캐릭터가 존재하지 않습니다.");
        return null;
    }
}
