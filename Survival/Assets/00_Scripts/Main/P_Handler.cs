using UnityEngine;

public class P_Handler : MonoBehaviour
{
    public static M_Object m_Object = null;

    public void Hit()
    {
        m_Object.HP -= 10;
        m_Object.HP_Init();
    }
    
}
