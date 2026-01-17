using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using AYellowpaper.SerializedCollections;
using Unity.VisualScripting;
using System;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject canvas;

    [Header("스테이지 종료 후 보상 관련 UI요소")]
    public GameObject amendPanel;
    [SerializeField] private GameObject amendContents;

    [Header("상점 NPC 상호작용 시 띄울 상점 UI요소")]
    public GameObject shopPanel;
    [SerializeField] private GameObject productContents;

    [Header("판매 완료 스프라이")]
    [SerializeField] private Sprite soldOutSprite;

    private const int PRODUCT_COUNT = 5;

    //스테이지 클리어 후 보상 상자와 상호작용할 때 나타는 UI판넬
    public void ActiveAmendPanel(List<RelicInfo> relics, bool firstInteraction = true)
    {
        amendPanel.SetActive(true);

        if (firstInteraction)
        {
            for (int i = 0; i < relics.Count; i++)
            {
                //번호 할당
                int index = i;

                //버튼 오브젝트 가져온 후, Contents에 자식으로 넣음.
                var button = GameManager.instance.objectPoolManager.poolDic["UI"].GetGo("RelicSelectButton");
                button.transform.SetParent(amendContents.transform);

                //버튼의 이미지, 텍스트 및 착용할 유물 관련 정보 가져오기.
                var relicImg = button.transform.GetChild(0).GetComponent<Image>();
                var relicTxt = button.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
                //RelicInfo relic = chestInfo.SetRelic();

                //크기 조정.
                button.transform.localScale = new Vector2(1, 1);

                //이미지 및 설명 텍스트 변경.
                relicImg.sprite = relics[i].sprite;
                relicTxt.text = relics[i].explain;

                Button buttonEvent = button.GetComponent<Button>();

                buttonEvent.onClick.AddListener(() => GameManager.instance.unitManager.EquipRelic(relics[index]));
                buttonEvent.onClick.AddListener(() => GameManager.instance.eventManager.ReleaseRelicButton());
                //선택한 유물이 더 이상 다시 등장하지 않게 하기 위해 리스트에서 삭제하는 이벤트 추가.
                buttonEvent.onClick.AddListener(() => GameManager.instance.relicManager.RemoveRelic(relics[index]));
                buttonEvent.onClick.AddListener(() => GameManager.instance.canvasManager.amendPanel.SetActive(false));
            }
        }
    }

    public void ActiveShopPanel()
    {
        shopPanel.SetActive(true);
    }

    //상점에 나타낼 유물 구입 버튼을 미리 초기화하는 함수.
    public void SetShopPanel(List<RelicInfo> relics)
    {
        for (int i = 0; i < relics.Count; i++)
        {
            //번호 할당
            int index = i;

            //오브젝트 풀에서 버튼을 가져온 뒤 컨텐츠 자식으로 할당.
            var button = GameManager.instance.objectPoolManager.poolDic["UI"].GetGo("ProductButton");
            button.transform.SetParent(productContents.transform);

            //버튼의 이미지, 유물 설명 및 가격 등의 정보를 가져옴.
            var relicImg = button.transform.GetChild(0).GetComponent<Image>();
            var relicTxt = button.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            var priceTxt = button.transform.GetChild(2).transform.GetChild(1).GetComponent<TextMeshProUGUI>();

            //크기 조정.
            button.transform.localScale = new Vector2(1, 1);

            //이미지 및 설명 텍스트 변경.
            relicImg.sprite = relics[i].sprite;
            relicTxt.text = relics[i].explain;
            priceTxt.text = relics[i].price.ToString();

            //클릭 시 실행될 이벤트 등록.
            Button buttonEvent = button.GetComponent<Button>();
            buttonEvent.onClick.AddListener(() =>
            {
                if (GameManager.instance.unitManager.money - relics[index].price >= 0)
                {
                    GameManager.instance.unitManager.money -= relics[index].price;
                    buttonEvent.interactable = false;
                    buttonEvent.GetComponent<Image>().sprite = soldOutSprite;

                    buttonEvent.transform.GetChild(0).gameObject.SetActive(false);
                    buttonEvent.transform.GetChild(1).gameObject.SetActive(false);
                    buttonEvent.transform.GetChild(2).gameObject.SetActive(false);

                    GameManager.instance.unitManager.EquipRelic(relics[index]);
                    //해당 유물을 데이터에서 삭제하는 로직 추가.
                    GameManager.instance.relicManager.RemoveRelic(relics[index]);
                }
                else Debug.Log("돈이 부족합니다!");
            });
        }
    }
}
