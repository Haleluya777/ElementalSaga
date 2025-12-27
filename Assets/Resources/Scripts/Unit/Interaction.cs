using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour, IDataInitializable
{
    [SerializeField] private GameObject parentObj;
    private PlayerController controller;
    void Awake()
    {

    }

    void OnEnable()
    {
        if (parentObj.GetComponent<PlayableCharacter>().controlState == PlayableCharacter.ControlState.Player)
        {
            controller = parentObj.GetComponentInChildren<PlayerController>();
            // 구독 전에 항상 이전 구독을 제거하여 중복을 방지합니다.
            controller.interaction -= Interacte;
            controller.interaction += Interacte;
        }
    }

    void OnDisable()
    {
        if (parentObj.GetComponent<PlayableCharacter>().controlState == PlayableCharacter.ControlState.Player)
        {
            controller.interaction -= Interacte;
        }
    }

    public void Interacte()
    {
        RaycastHit2D hitted;
        if (hitted = Physics2D.BoxCast(parentObj.transform.position + new Vector3(0f, .5f), new Vector2(1, 1), 0, Vector2.zero, 0f, 1 << 9))
        {
            hitted.collider.GetComponent<IInteractable>().Interaction();
        }
    }

    public void DataInit()
    {

    }
}
