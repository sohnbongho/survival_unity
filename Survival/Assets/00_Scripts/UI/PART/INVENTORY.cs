using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class INVENTORY : UIPART
{
    public GameObject Item_Panel;
    public Transform Content;

    public Image WeightFill;
    public TextMeshProUGUI WeightText;

    List<Item_Panel> items = new List<Item_Panel>();
    int ItemMaximumValue = 50;

    public GameObject ItemClickTap;

    private void Start()
    {
        Init();
    }
    private void OnEnable()
    {
        SetInventory();
    }

    public void Init()
    {
        if (ItemFlowController.Item_Pairs.Count >= ItemMaximumValue)
        {
            ItemMaximumValue = ItemFlowController.Item_Pairs.Count;
        }

        for (int i = 0; i < ItemMaximumValue; ++i)
        {
            var go = Instantiate(Item_Panel, Content);
            go.gameObject.SetActive(true);
            var itemPanel = go.GetComponent<Item_Panel>();
            items.Add(itemPanel);
        }

        int value = 0;
        foreach (var item in ItemFlowController.Item_Pairs)
        {
            items[value].Init(item.Value, this);
            value++;
        }

        SetInventory();
    }

    public void SetInventory()
    {
        for (int i = 0; i < items.Count; ++i)
        {
            items[i].SetItem();
        }
        WeightFill.fillAmount = ItemFlowController.Weight() / ItemFlowController.Player_Weight;
        WeightText.text = string.Format("{0:0.0}/{1:0.0}",
            ItemFlowController.Weight(),
            ItemFlowController.Player_Weight);
    }

    public void SetItemClickAnimation(Item_Panel panel)
    {
        ItemClickTap.gameObject.SetActive(true);
        ItemClickTap.transform.SetParent(panel.transform);
        ItemClickTap.transform.localPosition = Vector2.zero;
    }
}
