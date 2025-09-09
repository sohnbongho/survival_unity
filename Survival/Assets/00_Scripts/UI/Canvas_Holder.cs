using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Canvas_Holder : MonoBehaviour
{
    public static Canvas_Holder instance = null;

    [SerializeField] private Transform UI_PART_PARENT;
    [SerializeField] private GameObject Board;
    [SerializeField] private GameObject InventoryPanel;

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
    public void CloseAllUI()
    {
        foreach (var part in uiParts.Values)
        {
            part.Close();
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

        Delegate_Holder.OnInteraction += GetBoard;
        Delegate_Holder.OnInteractionOut += BoardOut;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            uiParts["INVENTORY"].Toggle();            
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            uiParts["BUILDING"].Toggle();            
        }
    }

    public void GetBoard()
    {
        Board.SetActive(true);
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
