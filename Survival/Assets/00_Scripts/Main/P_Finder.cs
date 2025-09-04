using System.Collections.Generic;
using UnityEngine;

public class P_Finder : MonoBehaviour
{
    [SerializeField] private float checkRaduis = 5.0f;
    [SerializeField] private LayerMask interactableLayer; // "object"Layer를 유니티에서 등록해야 한다.
    [SerializeField] Canvas uiCanvas;
    [SerializeField] private GameObject IconPrefab;

    [SerializeField] private float activationDistance = 3.0f;

    private Dictionary<Transform, GameObject> activeIcons = new Dictionary<Transform, GameObject>();
    [HideInInspector] public bool OnInteraction = false;
    private Transform closetObject = null;

    private void Start()
    {
        Delegate_Holder.OnInteraction += OnInteractionVoid;
        Delegate_Holder.OnInteractionOut += OnInteractionOut;
    }

    void OnInteractionVoid()
    {
        OnInteraction = true;
        closetObject = null;



        IconInit();
    }
    void OnInteractionOut()
    {
        Invoke("InteractionFalse", 1.0f);
        activeIcons.Clear();
    }
    void InteractionFalse() => OnInteraction = false;

    private void Update()
    {
        if (OnInteraction)
        {
            return;
        }

        Collider[] nearbyObjects = Physics.OverlapSphere(transform.position, checkRaduis, interactableLayer);
        closetObject = null;

        float closetDistance = Mathf.Infinity;

        foreach (Collider obj in nearbyObjects)
        {
            Transform targetTransform = obj.transform;
            float distance = Vector3.Distance(transform.position, targetTransform.position);
            if (distance <= activationDistance && distance < closetDistance)
            {
                closetObject = targetTransform;
                closetDistance = distance;
            }
        }
        if (closetObject != null)
        {
            ShowIcon(closetObject);
            if (Input.GetKeyDown(KeyCode.F))
            {
                //Debug.Log("오브젝트 상호작용!");
                closetObject.GetComponent<M_Object>().Interaction();
                Delegate_Holder.OnStartInteraction();
            }
        }

        IconInit();
    }

    private void IconInit()
    {
        List<Transform> toRemove = new List<Transform>();
        foreach (var iconEntry in activeIcons)
        {
            if (iconEntry.Key != closetObject)
            {
                iconEntry.Value.GetComponent<UI_Animation_Handler>().AnimationChange("Out");
                toRemove.Add(iconEntry.Key);
            }
        }
        foreach (var tranformToRemove in toRemove)
        {
            activeIcons.Remove(tranformToRemove);
        }
    }

    private void ShowIcon(Transform targetTransform)
    {
        if (activeIcons.ContainsKey(targetTransform))
        {
            UpdateIconPositon(targetTransform, activeIcons[targetTransform]);
            return;
        }
        GameObject iconInstance = Instantiate(IconPrefab, uiCanvas.transform);
        activeIcons[targetTransform] = iconInstance;
        UpdateIconPositon(targetTransform, iconInstance);
    }
    private void UpdateIconPositon(Transform targetTransform, GameObject icon)
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(
            new Vector3(
                targetTransform.position.x,
                targetTransform.position.y + 1.5f,
                targetTransform.position.z));

        icon.GetComponent<RectTransform>().position = screenPosition;
    }

}
