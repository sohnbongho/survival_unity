using System.Collections;
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
