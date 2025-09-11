using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;

public class Navagation_Mng : MonoBehaviour
{
    public static Navagation_Mng instance = null;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    [SerializeField] private Transform Content;
    Nav_Item[] P_Item;
    Nav_Item Toast_Item;

    private void Start()
    {
        P_Item = GetComponentsInChildren<Nav_Item>(true);        
    }

    public void PanelGet_Item(Item_Scriptable data, int count)
    {
        var go = Instantiate(P_Item[0], Content);
        go.gameObject.SetActive(true);
        go.Init(data, count);
    }
    public void PanelGet_Toast(Scriptable_Base data, string key)
    {
        var go = Instantiate(P_Item[1], Content);
        go.gameObject.SetActive(true);
        go.Init_Building(data, key);
    }

}
