using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Navagation_Mng : MonoBehaviour
{
    public static Navagation_Mng instance = null;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    [SerializeField] private Transform Content;
    [SerializeField] private int Maximum;
    Nav_Item[] P_Item;

    private void Start()
    {
        P_Item = GetComponentsInChildren<Nav_Item>(true);
    }

    public void PanelGet_Item(Item_Scriptable data, int count)
    {
        var go = MakeItem(0);
        go.Init(data, count);
    }
    public void PanelGet_Toast(Scriptable_Base data, string key)
    {
        var go = MakeItem(1);
        go.Init_Building(data, key);
    }

    private Nav_Item MakeItem(int value)
    {
        var go = Instantiate(P_Item[value], Content);
        go.transform.SetAsFirstSibling(); // 맨 처음 위치로 생성해라
        go.gameObject.SetActive(true);

        if (Content.childCount > Maximum)
        {
            // 즉시 오브젝트를 파괴하라는 함수
            DestroyImmediate(Content.GetChild(Content.childCount - 1).gameObject);
        }
        return go;
    }

}
