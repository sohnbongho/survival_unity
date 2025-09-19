using System.Collections.Generic;
using UnityEngine;

public class P_Finder : MonoBehaviour
{
    [SerializeField] private float checkRaduis = 5.0f;
    [SerializeField] private float checkMonsterRaduis = 10.0f;
    [SerializeField] private LayerMask interactableLayer; // "object"Layer를 유니티에서 등록해야 한다.
    [SerializeField] private LayerMask MonsterLayer; // 

    [SerializeField] Canvas uiCanvas;
    [SerializeField] private GameObject IconPrefab;

    [SerializeField] private float activationDistance = 3.0f;
    [SerializeField] private float AttackSpeed;

    private Dictionary<Transform, GameObject> activeIcons = new Dictionary<Transform, GameObject>();
    [HideInInspector] public bool OnInteraction = false;
    public bool GetMonster = false;
    private bool IsAttack = false;

    private Transform closetObject = null;
    public Transform MonsterTarget;

    private void Start()
    {
        Delegate_Holder.OnInteraction += OnInteractionVoid;
        Delegate_Holder.OnInteractionOut += OnInteractionOut;
    }

    void OnInteractionVoid()
    {
        OnInteraction = true;
        transform.LookAt(closetObject.transform.position);
        closetObject = null;
        IconInit();
    }
    void OnInteractionOut()
    {
        OnInteraction = false;
        P_Movement.instance.EquipmentAllDeactive();
        activeIcons.Clear();
    }
    

    private void Update()
    {
        if (OnInteraction)
        {
            return;
        }

        //////////// 근처 몬스터 체크
        Collider[] monsterObjects = Physics.OverlapSphere(transform.position, checkMonsterRaduis, MonsterLayer);
        GetMonster = monsterObjects.Length > 0;
        if (GetMonster)
        {
            MonsterTarget = null;
            float monsterClosetDistance = Mathf.Infinity;
            foreach(var monster in monsterObjects)
            {
                float distance = Vector3.Distance(transform.position, monster.transform.position);
                if(distance < monsterClosetDistance)
                {
                    monsterClosetDistance = distance;
                    MonsterTarget = monster.transform;
                }
            }
            if(MonsterTarget != null)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    if (IsAttack == false)
                    {
                        AttackMonster(monsterObjects);
                        P_Movement.instance.EquipmentChange(Object_Type.Monster, true);
                    }
                }

                transform.LookAt(MonsterTarget);
                closetObject = null;
                IconInit();
            }            
            return;
        }

        P_Movement.instance.EquipmentChange(Object_Type.Monster, false);

        //////////// 근처 오브젝트 체크
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
                M_Object subObject = null;
                if (closetObject.GetComponent<M_Object>() == null)
                {
                    subObject = closetObject.transform.parent.GetComponent<M_Object>();
                }
                else
                {
                    subObject = closetObject.GetComponent<M_Object>();
                }

                subObject.Interaction(GetComponent<Character>());
                //Debug.Log("오브젝트 상호작용!");

                Delegate_Holder.OnStartInteraction();
            }
        }

        IconInit();
    }

    private void AttackMonster(Collider[] mosnters)
    {
        IsAttack = true;
        P_Movement.instance.AnimationWeight(1, 1); // 1번 레이더 weight 1로 설정
        P_Movement.instance.AnimationChange("Attack");
        P_Movement.instance.Colliders = mosnters;
        Invoke("ReturnAttack", AttackSpeed);
    }

    private void ReturnAttack()
    {
        P_Movement.instance.AnimationWeight(1, 0); // 공격끝나면 1번레이어 가중치 0으로 
        IsAttack = false;
    }

    private void IconInit()
    {
        List<Transform> toRemove = new List<Transform>();
        foreach (var iconEntry in activeIcons)
        {
            if (iconEntry.Key != closetObject || closetObject == null)
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
