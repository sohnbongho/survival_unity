using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Canvas_Holder : MonoBehaviour
{
    public static Canvas_Holder instance = null;

    [SerializeField] private Transform UI_PART_PARENT;
    [SerializeField] private GameObject Board;
    [SerializeField] private GameObject InventoryPanel;
    [SerializeField] private TextMeshProUGUI StaminaText;
    [SerializeField] private Image StaminaFill;

    public Image BoardHpFill, BoardHpWhiteFill;
    Coroutine F_Coroutine;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private Dictionary<string, UIPART> uiParts = new Dictionary<string, UIPART>();
    Popup_Description popup;

    public void DestroyPopUp()
    {
        if (popup != null)
        {
            Destroy(popup.gameObject);
        }
    }

    public Popup_Description GetPopUp()
    {
        DestroyPopUp();

        popup = Instantiate(Resources.Load<Popup_Description>("Prefab/PopUp"), transform);
        return popup;
    }

    public void OpenUI(string uiName)
    {
        if (uiParts.ContainsKey(uiName))
        {
            uiParts[uiName].Open();
        }
        else
        {
            Debug.LogWarning($"UI {uiName} not found.");
        }
    }
    public void CloseUI(string uiName)
    {
        if (uiParts.ContainsKey(uiName))
        {
            uiParts[uiName].Close();
        }
        else
        {
            Debug.LogWarning($"UI {uiName} not found.");
        }
    }
    public void CloseAllUI(string name = "")
    {
        foreach (var part in uiParts)
        {
            if (name != part.Key)
            {
                part.Value.Close();
            }
        }
    }

    private void Start()
    {
        // true: 비활성화 된 오브젝트도 찾는다.
        UIPART[] parts = UI_PART_PARENT.GetComponentsInChildren<UIPART>(true);
        foreach (var part in parts)
        {
            uiParts.Add(part.name, part);
        }

        //Delegate_Holder.OnInteraction += GetBoard;
        Delegate_Holder.OnInteractionOut += BoardOut;
        Delegate_Holder.OnStamina += StaminaCheck;
    }
    private void Update()
    {
        CheckUI(KeyCode.I, "INVENTORY");
        CheckUI(KeyCode.B, "BUILDING");
    }
    public void GetText(string temp, Color color)
    {
        Vector3 posReal = P_Movement.instance.transform.position;

        posReal.y += 3.0f;
        posReal.x += Random.Range(-0.5f, 0.5f);
        posReal.z += Random.Range(-0.5f, 0.5f);

        var go = Instantiate(Resources.Load<GameObject>("TextObject"), posReal, Quaternion.Euler(55, 0, 0));

        TextMeshPro textObj = go.GetComponent<TextMeshPro>();
        textObj.color = color;
        textObj.text = temp;
    }

    private void StaminaCheck(int value)
    {
        StaminaText.text = Base_Mng.instance.Game.Stamina + "/" + Base_Mng.instance.Game.MaxStamina;
        StaminaFill.fillAmount = Base_Mng.instance.Game.Stamina / (float)Base_Mng.instance.Game.MaxStamina;
    }

    private void CheckUI(KeyCode key, string uiName)
    {
        if (Input.GetKeyDown(key))
        {
            CloseAllUI(uiName);
            DestroyPopUp();

            uiParts[uiName].Toggle();
        }
    }

    public void GetBoard()
    {
        if (Board.activeSelf == false)
        {
            Board.SetActive(true);
        }
    }

    public void BoardOut() => Board.GetComponent<UI_Animation_Handler>().AnimationChange("Out");
    public void AllStopCoroutine() => StopAllCoroutines();

    public void BoardFill(float hp, float MaxHp)
    {
        BoardHpFill.fillAmount = hp / MaxHp;
        if (F_Coroutine != null)
        {
            StopCoroutine(F_Coroutine);
        }
        F_Coroutine = StartCoroutine(FillCoroutine());
    }

    IEnumerator FillCoroutine()
    {
        while (BoardHpWhiteFill.fillAmount > BoardHpFill.fillAmount)
        {
            BoardHpWhiteFill.fillAmount = Mathf.Lerp(BoardHpWhiteFill.fillAmount,
                BoardHpFill.fillAmount,
                Time.deltaTime * 5.0f);

            yield return null;
        }

    }
}
