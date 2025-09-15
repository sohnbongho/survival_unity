using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum State
{
    IDLE,
    MOVE,
    Arrived,
    Interaction
}

public class Worker : Chracter
{
    public float checkRaduis;
    public float activationDistance;
    public LayerMask interactableLayer;
    public Transform closetObject;

    public State m_State;
    NavMeshAgent agent;

    public override void Start()
    {
        base.Start();

        agent = GetComponent<NavMeshAgent>();
        StateChange(State.IDLE);
    }
    private void Update()
    {
        if (m_State == State.MOVE)
        {
            animator.SetFloat("a_Speed", agent.velocity.magnitude);
            if (agent.remainingDistance <= 1.0f)
            {
                StateChange(State.Arrived);
            }
        }

    }

    public void StateChange(State state)
    {
        m_State = state;
        switch (m_State)
        {
            case State.IDLE:
                animator.SetBool("NoneInteraction", false);
                StartCoroutine(LookAtTarget());
                break;
            case State.MOVE:

                break;
            case State.Arrived:
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

                    subObject.Interaction(GetComponent<Chracter>());

                    animator.SetBool("NoneInteraction", true);
                    animator.SetFloat("a_Speed", 0.0f); //             
                    StateChange(State.Interaction);
                }
                break;
            case State.Interaction:
                break;
        }
    }

    IEnumerator LookAtTarget()
    {
        yield return new WaitForSeconds(1.0f);

        while (closetObject == null)
        {
            FindClosetTarget();
            yield return null;
        }

        StateChange(State.MOVE);
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
