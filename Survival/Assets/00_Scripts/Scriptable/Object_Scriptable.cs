using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ITEMLIST
{
    public Item_Scriptable Item_Data;
    [Range(0.0f, 100.0f)]
    public float Value;
}

[CreateAssetMenu(fileName = "Object_Scriptable", menuName = "Scriptable Objects/Object_Scriptable")]
public class Object_Scriptable : ScriptableObject
{
    public Object_Type m_Type;
    public string Name;
    public int HP;

    public List<ITEMLIST> Drop_Items = new List<ITEMLIST>();
}
