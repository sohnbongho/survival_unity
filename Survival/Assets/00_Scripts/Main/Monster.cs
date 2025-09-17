using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    public int HP;
    public int MaxHP;

    [SerializeField] private float Range;
    [SerializeField] private GameObject Board;

    [SerializeField] private Image Silider01Fill, Silider02Fill;

    Coroutine Coroutine;

    public void Start()
    {
        HP = MaxHP;
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
