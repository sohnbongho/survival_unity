using UnityEngine;

public class Character : MonoBehaviour
{
    public bool MainPlayer = false;

    public int HP;
    public int MaxHP;

    [SerializeField] protected GameObject[] Equipments;
    protected Animator animator;
    public M_Object m_Object = null;
    public Collider[] Colliders;
    [SerializeField] protected GameObject HitParticle;
    [SerializeField] private Transform GetParticleTransform;

    public virtual void Start()
    {
        animator = GetComponent<Animator>();
        HP = MaxHP;
    }
    public void AnimationWeight(int layer, float weight)
    {
        // [layer]번 레이어 weight값 설정
        animator.SetLayerWeight(layer, weight);
    }

    public virtual void Hit()
    {
        if (m_Object == null)
            return;

        m_Object.HP -= 20;                
        GetHitParticle();

        m_Object.OnHit(this);
    }

    /// <summary>
    /// 근거리 공격용
    /// </summary>
    public virtual void Attack()
    {
        GetHitParticle();
        for (int i = 0; i < Colliders.Length; i++)
        {
            Colliders[i].GetComponent<Monster>().GetDamage(40);
        }
    }

    /// <summary>
    /// 원거리 공격용
    /// </summary>
    public virtual void Bullet()
    {

    }

    public void GetHitParticle()
    {
        if (GetParticleTransform == null)
            return;

        var realPos = GetParticleTransform.position;
        Vector3 pos = new Vector3(
            realPos.x + Random.Range(-0.5f, 0.5f),
            realPos.y,
            realPos.z + Random.Range(-0.5f, 0.5f));
        Instantiate(HitParticle, pos, Quaternion.identity);
    }
    

    public void EquipmentChange(Object_Type type, bool active)
    {
        Equipments[(int)type].gameObject.SetActive(active);
    }


    public void AnimationChange(string temp)
    {
        animator.SetTrigger(temp);
    }
    public virtual void EquipmentAllDeactive()
    {
        for (int i = 0; i < Equipments.Length; i++)
        {
            Equipments[i].SetActive(false);
        }

    }
}
