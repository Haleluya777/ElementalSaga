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
            controller.interaction += Interaciton;
        }
    }

    void OnDisable()
    {
        if (parentObj.GetComponent<PlayableCharacter>().controlState == PlayableCharacter.ControlState.Player)
        {
            controller.interaction -= Interaciton;
        }
    }

    public void Interaciton()
    {
        Debug.Log("상호작용 시도");
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
