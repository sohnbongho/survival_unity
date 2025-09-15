using System.Xml.Linq;
using UnityEngine;

public class UIPART : MonoBehaviour
{
    public bool IsActive => gameObject.activeSelf;
    public virtual void Open()
    {
        gameObject.SetActive(true);
        Canvas_Holder.Uis.Enqueue(this);

    }
    public virtual void Close()
    {
        if (IsActive == false)
        {
            Debug.LogWarning($"Not acitive this UI.");
            return;
        }
        Canvas_Holder.Uis.Dequeue();

        if (GetComponent<Animator>() != null)
        {
            GetComponent<Animator>().SetTrigger("Out");
            return; // 바로 아래 Active로 바로 끄면 애니메이션이 플레이가 안된다.
        }

        gameObject.SetActive(false);
    }

    public virtual void Toggle()
    {
        if (IsActive)
        {
            Close();
        }
        else
        {
            Open();
        }
    }
}
