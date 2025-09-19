using UnityEngine;

public class Base_Mng : MonoBehaviour
{
    public static Base_Mng instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            Initialize();            
        }
    }
    private void Initialize()
    {
        Build = GetComponentInChildren<Building_Mng>();
        Game = GetComponentInChildren<Game_Mng>();
        Object = GetComponentInChildren<Object_Mng>();
        Weather = GetComponentInChildren<Weather_Mng>();
    }

    public static Building_Mng Build;
    public static Game_Mng Game;
    public static Object_Mng Object;
    public static Weather_Mng Weather;
}
