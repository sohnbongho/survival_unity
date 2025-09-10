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
    Nav_Item P_Item;

    private void Start()
    {
        P_Item = GetComponentInChildren<Nav_Item>();
        P_Item.gameObject.SetActive(false);
    }

    public void PanelGet_Item(Item_Scriptable data, int count)
    {
        var go = Instantiate(P_Item, Content);
        go.gameObject.SetActive(true);
        go.Init(data, count);
    }


}
