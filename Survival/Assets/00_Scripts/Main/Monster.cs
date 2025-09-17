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
    Coroutine Hit_Coroutine;
    Renderer Renderer;

    public void Start()
    {
        HP = MaxHP;
        Renderer = transform.GetComponentInChildren<Renderer>();
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
