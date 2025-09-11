using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public enum Material_Type
{
    Opaque,
    Transparent,
}

public class Building_OBJ : MonoBehaviour
{
    public Building_Scriptable m_Data;
    [SerializeField] private ParticleSystem paricle;
    public Build_Type type;
    Renderer renderer;
    Collider collider;

    public Material Opaque_M, Transparent_M;
    public Color[] Colors;

    public bool CanBuild = true;
    bool Completed = false;
    bool getTriggerMaterial = true;

    public GameObject Board;
    public GameObject PortalQuad;

    [SerializeField] private UnityEngine.UI.Image IconImage;
    [SerializeField] private Image FillSlilder;
    [SerializeField] private TextMeshProUGUI TitleText;
    [SerializeField] private TextMeshProUGUI PercentageText;

    private void Awake()
    {
        renderer = GetComponentInChildren<Renderer>();
        collider = GetComponentInChildren<Collider>();
    }
    public void Confirm()
    {
        getTriggerMaterial = false;
        paricle.Play();        

        // Canvas UI를 카메라로 바라보게
        Camera mainCamera = Camera.main;
        Transform parent = Board.transform.parent;
        parent.eulerAngles = new Vector3 (55.0f,
            parent.eulerAngles.y - transform.eulerAngles.y, 
            0f);

        Board.SetActive(true);
        IconImage.sprite = Asset_Mng.Get_Atlas(m_Data.Key);
        TitleText.text = m_Data.Key;
        SetBuildData(m_Data.timer, BuildCompleted);
    }

    private void BuildCompleted()
    {
        SetMaterial(Material_Type.Opaque);
        Board.GetComponent<Animator>().SetTrigger("Out");
        StartCoroutine(CompletedCoroutine());
        PortalQuad.SetActive(true); // 포탈 이펙트 on
    }

    private IEnumerator CompletedCoroutine()
    {
        float current = 0.0f;
        float percent = 0.0f;
        float emissionStart = 1.0f;
        float emissionEnd = 20.0f;
        Color startColor = Color.white;
        Color endColor = Color.black;

        while (percent < 1.0f)
        {
            current += Time.deltaTime;
            percent += current / 1.0f;
            float lerpEmission = Mathf.Lerp(emissionStart, emissionEnd, percent);
            renderer.material.SetColor("_EmissionColor", startColor * lerpEmission);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        current = 0.0f;
        percent = 0.0f;
        while (percent < 1.0f)
        {
            current += Time.deltaTime;
            percent += current / 1.0f;
            float lerpEmission = Mathf.Lerp(emissionEnd, emissionStart, percent);
            Color lerpColor = Color.Lerp(startColor, endColor, percent);
            renderer.material.SetColor("_EmissionColor", lerpColor * lerpEmission);
            yield return null;
        }
        Completed = true;
    }


    public void SetBuildData(float time, Action action)
    {
        StartCoroutine(SliderFillCoroutine(time, action));

    }
    IEnumerator SliderFillCoroutine(float time, Action action)
    {
        float t = 0.0f;
        while (t <= time)
        {
            t += Time.deltaTime;
            FillSlilder.fillAmount = t / time;
            PercentageText.text = string.Format("{0:0.0}%",
                FillSlilder.fillAmount * 100.0f);

            yield return null;
        }
        if (action != null)
        {
            action?.Invoke();
        }
    }

    public void SetMaterial(Material_Type type)
    {
        switch (type)
        {
            case Material_Type.Opaque:
                renderer.material = Opaque_M;
                break;
            case Material_Type.Transparent:
                renderer.material = Transparent_M;
                break;
        }
    }
    public void SetTrigger(bool active)
    {
        collider.isTrigger = active;
    }

    // OnTrigger, OnCollider
    private void OnTriggerEnter(Collider other)
    {
        if (!getTriggerMaterial)
            return;

        if (other.gameObject.name != "Terrain")
        {
            SetMaterial_Color(1);
            CanBuild = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!getTriggerMaterial)
            return;

        if (other.gameObject.name != "Terrain")
        {
            SetMaterial_Color(0);
            CanBuild = true;
        }
    }

    public void SetMaterial_Color(int value)
    {
        renderer.material.SetColor("_EmissionColor", Colors[value]);
    }
}
