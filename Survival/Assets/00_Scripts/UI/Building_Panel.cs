using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Building_Panel : MonoBehaviour
{
    private Building_Scriptable m_Data;
    [SerializeField] private Image m_Icon;
    [SerializeField] private TextMeshProUGUI m_Text;

    public void Init(Building_Scriptable data)
    {
        m_Data = data;
        gameObject.SetActive(true);
    }

    public void SetData()
    {
        m_Icon.sprite = Asset_Mng.Get_Atlas(m_Data.Name);
        m_Text.text = m_Data.Name;
    }
}
