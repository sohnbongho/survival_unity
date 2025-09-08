using System.Xml.Linq;
using UnityEngine;

public class UIPART : MonoBehaviour
{
    public bool IsActive => gameObject.activeSelf;
    public virtual void Open()
    {
        gameObject.SetActive(true);

    }
    public virtual void Close()
    {
        if (IsActive == false)
        {
            Debug.LogWarning($"Not acitive this UI.");
            return;
        }
        
        gameObject.SetActive(false);
    }

    public virtual void Toggle()
    {
        if(IsActive)
        {
            Close();
        }
        else
        {
            Open();
        }

        
    }
}
