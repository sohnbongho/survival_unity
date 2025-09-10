using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Building_Panel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private Building_Scriptable m_Data;
    [SerializeField] private Image m_Icon;
    [SerializeField] private TextMeshProUGUI m_Text;

    BUILDING parentPanel;

    public void Init(Building_Scriptable data, BUILDING building)
    {
        m_Data = data;
        parentPanel = building;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (parentPanel == null)
            return;

        if (parentPanel.GetClick == false)
        {
            parentPanel.GetClick = true;
            parentPanel.AnimationChange("Click");
        }
        parentPanel.GetItemsData(m_Data);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (parentPanel == null)
            return;

        parentPanel.SetItemClickAnimation(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (parentPanel == null)
            return;

        if (parentPanel.ItemClickTap.activeSelf == true)
        {
            parentPanel.ItemClickTap.SetActive(false);
        }
    }

    public void SetData()
    {
        gameObject.SetActive(true);
        m_Icon.sprite = Asset_Mng.Get_Atlas(m_Data.Name);
        m_Text.text = m_Data.Name;
    }

    // OnEnable -> 오브젝트가 액티브값이 활성화 됐을때
    // OnDisable -> 오브젝트가 액티브값이 비활성화 됐을때


}
