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
        SetStamina(0);
    }

    public void SetStamina(int value)
    {
        Stamina += value;
        Delegate_Holder.OnStaminaChange(Stamina);
    }

}
