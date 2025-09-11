
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public delegate void onItemGet();


public class ItemFlowController : MonoBehaviour
{
    public static event onItemGet OnItemGet;

    public static Dictionary<string, ITEM> Item_Pairs = new Dictionary<string, ITEM>();
    public static float Player_Weight = 2500.0f;

    public static List<ITEM> DROPITEMLIST(List<ITEMLIST> m_ItemList)
    {
        List<ITEM> Get_Item_List = new List<ITEM>();

        for (int i = 0; i < m_ItemList.Count; i++)
        {
            float RandomValue = Random.Range(0.0f, 100.0f);
            if (RandomValue <= m_ItemList[i].Value)
            {
                int value = Random.Range(1, m_ItemList[i].Maximum);
                ITEM item = new ITEM();
                item.Data = m_ItemList[i].Item_Data;
                item.Count = value;                
                Get_Item_List.Add(item);
            }
        }
        return Get_Item_List;
    }

    public static void GETITEM(Item_Scriptable scriptableData, int value)
    {
        ITEM item = new ITEM();
        item.Data = scriptableData;
        item.Count = value;

        var ID = item.Data.Key;

        if (HaveItem(ID))
        {
            Item_Pairs[ID].Count += value;
        }
        else
        {
            Item_Pairs.Add(ID, item);
        }
        OnItemGet?.Invoke();

    }
    public static bool HaveItem(string key)
    {
        if (Item_Pairs.ContainsKey(key))
        {
            return true;
        }
        return false;
    }
    public static int ItemCount(string key)
    {
        if (Item_Pairs.ContainsKey(key))
        {
            return Item_Pairs[key].Count;
        }
        return 0;
    }
    public static float WeightItem(string key)
    {
        if (HaveItem(key))
        {
            ITEM item = Item_Pairs[key];
            float value = item.Data.Weight * item.Count;
            return value;
        }
        return -1.0f;
    }
    public static float Weight()
    {
        float weight = 0.0f;
        foreach(var item in Item_Pairs)
        {
            weight += WeightItem(item.Key);
        }
        return weight;
    }
}
