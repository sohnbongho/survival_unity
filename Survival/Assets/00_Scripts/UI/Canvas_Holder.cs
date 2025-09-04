using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Canvas_Holder : MonoBehaviour
{
    public static Canvas_Holder instance = null;

    [SerializeField] private GameObject Board;
    [SerializeField] private Image BoardHpFill, BoardHpWhiteFill;
    Coroutine F_Coroutine;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        Delegate_Holder.OnInteraction += GetBoard;
        Delegate_Holder.OnInteractionOut += BoardOut;
    }

    public void GetBoard()
    {
        Board.SetActive(true);
        BoardHpFill.fillAmount = 1.0f;
        BoardHpWhiteFill.fillAmount = 1.0f;
    }

    public void BoardOut() => Board.GetComponent<UI_Animation_Handler>().AnimationChange("Out");

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
