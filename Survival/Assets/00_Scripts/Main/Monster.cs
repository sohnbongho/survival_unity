using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    public int HP;
    public int MaxHP;
    NavMeshAgent Agent;


    [SerializeField] private float Range;
    [SerializeField] private GameObject Board;

    [SerializeField] private Image Silider01Fill, Silider02Fill;

    Coroutine Coroutine;
    Coroutine Hit_Coroutine;
    Coroutine Find_Coroutine;
    Renderer Renderer;
    Animator Animator;

    Transform Target;
    Transform CanvasTransform;
    Vector3 LastTargetPosition;

    bool IsAttack = false;
    bool IsDead = false;


    public void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        Animator = GetComponent<Animator>();

        HP = MaxHP;
        Renderer = transform.GetComponentInChildren<Renderer>();

        // Canvas UI를 카메라로 바라보게
        {            
            CanvasTransform = Board.transform.parent;
            CanvasTransform.eulerAngles = new Vector3(55.0f,
                CanvasTransform.eulerAngles.y - transform.eulerAngles.y,
                0f);
        }
        
        
        AnimationChange("IDLE", false);

        StartCoroutine(FindPlayer());
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
        if (Target == null)
            return;

        float distance = Vector3.Distance(Target.position, transform.position);
        const float attackedDistance = 2.0f;

        if (distance > attackedDistance && distance <= 10.0f)
        {
            StopMovement(false);
            Animator.SetBool("WALK", false);
            Agent.SetDestination(Target.position);

            LastTargetPosition = Target.position;
        }
        else if (distance <= attackedDistance)
        {
            //////// 대상에게 도착
            StopMovement(true);
            if (IsAttack == false)
            {
                AttackPlayer();
            }
        }
        else if (distance > 10.0f)
        {
            StopMovement(false);
            Animator.SetBool("WALK", false);

            Target = null;
            LastTargetPosition = Vector3.zero;
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

    IEnumerator FindPlayer()
    {
        float distance = Vector3.Distance(transform.position, P_Movement.instance.transform.position);
        if (Target == null)
        {
            if (distance <= 5.0f)
            {
                Target = P_Movement.instance.transform;
                LastTargetPosition = Target.position;
                AnimationChange("WALK", false);
            }
        }

        yield return new WaitForSeconds(1.0f);
        StartCoroutine(FindPlayer());
    }


    public void GetDamage(int dmg)
    {
        var playerPos = P_Movement.instance.transform.position;
        var distance = Vector3.Distance(transform.position, playerPos);
        if (distance <= Range)
        {
            Board.SetActive(true);
            Canvas_Holder.instance.GetText(dmg.ToString(), Color.yellow, transform.position);
            HP -= dmg;
            P_Movement.instance.GetComponent<Character>().GetHitParticle();

            if (Coroutine != null)
            {
                StopCoroutine(Coroutine);
            }
            Coroutine = StartCoroutine(SliderCoroutine(HP));

            if (Hit_Coroutine != null)
            {
                StopCoroutine(Hit_Coroutine);
            }
            Hit_Coroutine = StartCoroutine(GetHitCoroutine());
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

    IEnumerator SliderCoroutine(int hp)
    {
        float value = (float)HP / (float)MaxHP;
        Silider02Fill.fillAmount = value;

        float timer = 0.0f;
        while (timer < 1.0f)
        {
            timer += Time.deltaTime;
            Silider01Fill.fillAmount = Mathf.Lerp(Silider01Fill.fillAmount, Silider02Fill.fillAmount, timer);
            yield return null;
        }
    }
}
