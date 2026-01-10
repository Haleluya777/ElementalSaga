using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : Unit, IInteractable
{
    private enum NPCType { Basic, Shopper, Healer, Buffer }
    [SerializeField] private NPCType npcType;
    public void Interaction()
    {
        switch (npcType)
        {
            case NPCType.Basic:
                Debug.Log("기본 NPC와 상호작용.");
                break;

            case NPCType.Shopper:
                Debug.Log("상점 NPC와 상호작용");
                GameManager.instance.canvasManager.ActiveShopPanel();
                break;

            case NPCType.Healer:
                Debug.Log("힐러 NPC와 상호작용.");
                break;

            case NPCType.Buffer:
                Debug.Log("버퍼 NPC와 상호작용.");
                break;
        }
    }
}
