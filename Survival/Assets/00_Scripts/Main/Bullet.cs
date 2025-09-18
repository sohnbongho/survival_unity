using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Speed;
    public float ExplositionRadius = 5.0f;
    public int Damage;
    public GameObject ExlosionParticle;
    public LayerMask MonsterLayer; // 

    private Vector3 Target;
    private Collider[] Colliders;

    public void Init(Transform target)
    {
        Target = new Vector3(target.position.x, 
            target.position.y + 1.5f, 
            target.position.z);
    }
    public void Update()
    {
        float step = Speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, Target, step);
        if(Vector3.Distance(transform.position, Target) < 0.1f)
        {
            Explode();
        }
    }
    private void Explode()
    {
        var go = Instantiate(ExlosionParticle, transform.position, Quaternion.identity);
        
        Collider[] nearbyObjects = Physics.OverlapSphere(transform.position, ExplositionRadius, MonsterLayer);
        foreach(Collider hitCollider in nearbyObjects)
        {
            Monster monster = hitCollider.GetComponent<Monster>();
            if(monster != null)
            {
                monster.GetDamage(Damage);
            }
        }
        Destroy(this.gameObject);
    }

}
