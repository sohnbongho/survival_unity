using UnityEngine;

public class Character : MonoBehaviour
{
    public bool MainPlayer = false;

    public int HP;
    public int MaxHP;

    [SerializeField] protected GameObject[] Equipments;
    protected Animator animator;
    public M_Object m_Object = null;
    [SerializeField] protected GameObject HitParticle;

    public virtual void Start()
    {
        animator = GetComponent<Animator>();
    }
    public virtual void Hit()
    {
        if (m_Object == null)
            return;

        m_Object.HP -= 20;

        Vector3 pos = new Vector3(
            m_Object.transform.position.x + Random.Range(-0.5f, 0.5f),
            m_Object.transform.position.y + 1.5f,
            m_Object.transform.position.z + Random.Range(-0.5f, 0.5f));

        Instantiate(HitParticle, pos, Quaternion.identity);
        m_Object.OnHit(this);
    }
    public virtual void Attack()
    {
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
