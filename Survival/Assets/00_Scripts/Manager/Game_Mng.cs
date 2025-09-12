using System.Collections;
using UnityEngine;

public class Game_Mng : MonoBehaviour
{
    public int Stamina, MaxStamina;

    public void Start()
    {
        Stamina = MaxStamina;
        StartCoroutine(DelayStamina());
    }

    IEnumerator DelayStamina()
    {
        yield return new WaitForSeconds(0.02f);
        SetStamina(0, false);
    }

    public void SetStamina(int value, bool getText = true)
    {
        Stamina += value;
        if (getText)
        {
            Color color = value > 0 ? Color.green : Color.red;
            Canvas_Holder.instance.GetText(value.ToString(), color);
        }
        Delegate_Holder.OnStaminaChange(Stamina);
    }

}
