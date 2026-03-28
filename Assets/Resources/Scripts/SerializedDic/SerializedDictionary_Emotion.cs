using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AYellowpaper.SerializedCollections
{
    [CreateAssetMenu(fileName = "CharacterEmotionSpriteDic", menuName = "ScriptableObject/CharacterEmotionSpriteDic")]
    [System.Serializable]
    public class SerializedDictionary_Emotion : ScriptableObject
    {
        [SerializedDictionary("Emotion", "Sprite")]
        public SerializedDictionary<string, Sprite> sprites;
    }
}

