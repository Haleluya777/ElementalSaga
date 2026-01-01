using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject canvas;

    [Header("스테이지 종료 후 보상 관련 UI요소")]
    public GameObject amendPanel;
    [SerializeField] private GameObject amendContents;

    public void ActiveAmendPanel(RoomInfo roomInfo)
    {
        amendPanel.SetActive(true);
        for (int i = 0; i < 2; i++)
        {
            //버튼 오브젝트 가져온 후, Contents에 자식으로 넣음.
            var button = GameManager.instance.objectPoolManager.GetGo("RelicSelectButton");
            button.transform.SetParent(amendContents.transform);

            //버튼의 이미지, 텍스트 및 착용할 유물 관련 정보 가져오기.
            var relicImg = button.transform.GetChild(0).GetComponent<Image>();
            var relicTxt = button.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            RelicInfo relic = roomInfo.SetRelic();

            //크기 조정.
            button.transform.localScale = new Vector2(1, 1);

            //이미지 및 설명 텍스트 변경.
            relicImg.sprite = roomInfo.SetRelic().sprite;
            relicTxt.text = roomInfo.SetRelic().explain;

            Button buttonEvent = button.GetComponent<Button>();

            buttonEvent.onClick.AddListener(() => GameManager.instance.unitManager.EquipRelic(relic));
            buttonEvent.onClick.AddListener(() => GameManager.instance.eventManager.ReleaseRelicButton());
        }
    }
}
