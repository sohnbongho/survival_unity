using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Directional_Monster_Slider : MonoBehaviour
{
    [SerializeField] private Image Silider01Fill, Silider02Fill;
    public Monster Monster;
    Coroutine Coroutine;

    public void GetSliderCheck()
    {
        if (Coroutine != null)
        {
            StopCoroutine(Coroutine);
        }
        Coroutine = StartCoroutine(SliderCoroutine());
    }

    IEnumerator SliderCoroutine()
    {
        float value = (float)Monster.HP / (float)Monster.MaxHP;
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
