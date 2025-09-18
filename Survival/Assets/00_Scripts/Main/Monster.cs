using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.PlayerSettings;

public class Monster : MonoBehaviour
{
    public int HP;
    public int MaxHP;
    NavMeshAgent Agent;

    [SerializeField] private float Range;

    Coroutine Hit_Coroutine;

    Renderer Renderer;
    Animator Animator;

    Transform Target;

    bool IsAttack = false;
    bool IsDead = false;
    Vector3 MyPos;


    public void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        Animator = GetComponent<Animator>();

        HP = MaxHP;
        Renderer = transform.GetComponentInChildren<Renderer>();
        MyPos = transform.position;

        AnimationChange("IDLE", false);
    }

    private void AnimationChange(string ani, bool isTrigger = false)
    {
        Animator.SetBool("IDLE", false);
        Animator.SetBool("WALK", false);
        if (isTrigger)
        {
            Animator.SetTrigger(ani);
        }
        else
        {
            Animator.SetBool(ani, true);
        }

    }
    private void Attack()
    {
        P_Movement.instance.GetDamage(5);
    }

    private void Update()
    {
        if (IsDead)
            return;

        if (Target == null)
        {
            if(Agent.remainingDistance <= 2.0f)
            {
                StopMovement(true);
                AnimationChange("IDLE", false);
            }
            return;
        }

        float distance = Vector3.Distance(Target.position, transform.position);

        const float attackedDistance = 2.0f;

        if (distance <= attackedDistance)
        {
            //////// 대상에게 도착
            StopMovement(true);
            if (IsAttack == false)
            {
                AttackPlayer();
            }
        }
        else
        {
            StopMovement(false);
            AnimationChange("WALK", false);
            Agent.SetDestination(Target.position);
        }
    }
    private void AttackReturn() => IsAttack = false;

    private void StopMovement(bool stopped)
    {
        Agent.isStopped = stopped;
        if (stopped)
        {
            Agent.velocity = Vector3.zero;
        }
    }

    private void AttackPlayer()
    {
        IsAttack = true;
        AnimationChange("ATTACK", true);
        Invoke("AttackReturn", 1.0f);
    }

    public void GetPlayer(Transform target)
    {
        Target = target;
        AnimationChange("WALK", false);
    }

    public void RemovePlayer()
    {
        Target = null;
        StopMovement(false);
        AnimationChange("WALK", false);
        Agent.SetDestination(MyPos);
    }

    public void GetDamage(int dmg)
    {
        if (IsDead)
            return;

        var playerPos = P_Movement.instance.transform.position;
        var distance = Vector3.Distance(transform.position, playerPos);
        if (distance <= Range)
        {
            Canvas_Holder.instance.GetText(dmg.ToString(), Color.yellow, transform.position);
            HP -= dmg;

            Canvas_Holder.instance.AddSlider(this);
            P_Movement.instance.GetComponent<Character>().GetHitParticle();

            if (Hit_Coroutine != null)
            {
                StopCoroutine(Hit_Coroutine);
            }
            Hit_Coroutine = StartCoroutine(GetHitCoroutine());

            if (HP <= 0)
            {
                IsDead = true;
                StopAllCoroutines();
                StopMovement(true);
                Canvas_Holder.instance.RemoveSlider(this);
                this.gameObject.layer = LayerMask.NameToLayer("Default");
                AnimationChange("DIE", true);
                Destroy(this.gameObject, 1.5f); // 1.5초뒤 바로 삭제
            }
        }
    }

    IEnumerator GetHitCoroutine()
    {
        float current = 0.0f;
        float percent = 0.0f;
        const float endPercent = 0.2f;

        Color startColor = Color.black;
        Color endColor = Color.white;

        while (percent < 1.0f)
        {
            current += Time.deltaTime;
            percent = current / endPercent;

            Color lerpColor = Color.Lerp(startColor, endColor, percent);
            Renderer.material.SetColor("_EmissionColor", lerpColor);
            yield return null;
        }

        current = 0.0f;
        percent = 0.0f;
        while (percent < 1.0f)
        {
            current += Time.deltaTime;
            percent = current / endPercent;

            Color lerpColor = Color.Lerp(endColor, startColor, percent);
            Renderer.material.SetColor("_EmissionColor", lerpColor);
            yield return null;
        }
    }


}
