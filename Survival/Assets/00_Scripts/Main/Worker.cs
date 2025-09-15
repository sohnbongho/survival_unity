using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Worker : MonoBehaviour
{
    public float checkRaduis;
    public float activationDistance;
    public LayerMask interactableLayer;
    public Transform closetObject;

    Animator animator;
    NavMeshAgent agent;
    

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        StartCoroutine(LookAtTarget());
    }
    private void Update()
    {
        animator.SetFloat("a_Speed", agent.velocity.magnitude);
        if(agent.remainingDistance <= 2.0f)
        {
            Debug.Log("AI가 목적지에 도착하였습니다.");
        }

    }


    private void AnimationChange(string temp)
    {
        animator.SetTrigger(temp);
    }

    IEnumerator LookAtTarget()
    {
        while (closetObject == null)
        {
            FindClosetTarget();
            yield return null;
        }
        agent.SetDestination(closetObject.position);
    }
    

    private void FindClosetTarget()
    {
        Collider[] nearbyObjects = Physics.OverlapSphere(transform.position, checkRaduis, interactableLayer);
        closetObject = null;

        float closetDistance = Mathf.Infinity;

        foreach (Collider obj in nearbyObjects)
        {
            if (obj.GetComponent<Interaction_Hit>() != null)
            {
                Transform targetTransform = obj.transform;
                float distance = Vector3.Distance(transform.position, targetTransform.position);
                if (distance <= activationDistance && distance < closetDistance)
                {
                    closetObject = targetTransform;
                    closetDistance = distance;
                }
            }
        }

    }
}
