using System;
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

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        base.Start();
    }

    public void SetDestination(Vector3 pos, Action action)
    {
        agent.SetDestination(pos);
        animator.SetFloat("a_Speed", 1.0f);
        StartCoroutine(DestinationCoroutine(action));
    }

    private void Update()
    {
        if (m_State == State.MOVE)
        {
            if (closetObject == null)
            {
                StateChange(State.IDLE);
            }
        }
        else if (m_State == State.Interaction)
        {
            if (closetObject == null)
            {
                StateChange(State.IDLE);
            }
        }
    }

    IEnumerator DestinationCoroutine(Action action)
    {
        yield return new WaitForSeconds(0.5f);

        while (agent.remainingDistance > agent.stoppingDistance)
        {
            yield return null;
        }

        action?.Invoke();
    }


    public void StateChange(State state)
    {
        m_State = state;
        switch (m_State)
        {
            case State.IDLE:
                StopAllCoroutines();
                agent.stoppingDistance = 3.0f;
                EquipmentAllDeactive();
                animator.SetFloat("a_Speed", 0.0f);
                animator.SetBool("NoneInteraction", false);
                StartCoroutine(LookAtTarget());
                break;
            case State.MOVE:
                break;
            case State.Arrived:
                {
                    M_Object subObject = null;
                    if (closetObject == null)
                    {
                        StateChange(State.IDLE);
                    }

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
                    transform.LookAt(closetObject.transform);
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
            yield return new WaitForSeconds(0.5f);
        }

        //var targetPosition = new Vector3(closetObject.position.x,
        //    transform.position.y,
        //    closetObject.position.z);
        var targetPosition = closetObject.position;
        SetDestination(targetPosition, () => StateChange(State.Arrived));

        yield return new WaitForSeconds(0.02f);
        StateChange(State.MOVE);
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
