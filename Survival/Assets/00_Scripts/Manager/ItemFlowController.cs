
using System.Collections.Generic;
using UnityEngine;

public class ItemFlowController : MonoBehaviour
{
    public static Dictionary<int, ITEM> Item_Pairs = new Dictionary<int, ITEM>();

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

        int ID = item.Data.ItemID;

        if (HaveItem(ID))
        {
            Item_Pairs[ID].Count += value;
        }
        else
        {
            Item_Pairs.Add(ID, item);
        }

    }
    public static bool HaveItem(int value)
    {
        if (Item_Pairs.ContainsKey(value))
        {
            return true;
        }
        return false;
    }
}
