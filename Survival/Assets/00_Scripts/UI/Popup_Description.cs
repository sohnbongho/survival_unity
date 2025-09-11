using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Popup_Description : MonoBehaviour
{
    RectTransform rect;
    [SerializeField] private Image IconImage;
    [SerializeField] private TextMeshProUGUI TitleText;
    [SerializeField] private TextMeshProUGUI ExplainText;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    //private void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        Canvas_Holder.instance.DestroyPopUp();
    //    }
    //}

    public void Set_PopUP(String_Table type, string key, Vector2 pos)
    {
        // toolip 오브젝트가 밖으로 나가는 것을 방지하기 위한 코드
        rect.pivot = PivotPoint(pos);

        rect.anchoredPosition = pos;

        IconImage.sprite = Asset_Mng.Get_Atlas(key);
        TitleText.text = Utils.Localization_Text(type, key);
        ExplainText.text = Utils.Localization_Text(type, key + "_Value");
    }

    private Vector2 PivotPoint(Vector2 pos)
    {

        float xPos = pos.x > Screen.width / 2 ? 1.0f : 0.0f;
        float yPos = pos.y > Screen.height / 2 ? 1.0f : 0.0f;

        return new Vector2(xPos, yPos);
    }
}
