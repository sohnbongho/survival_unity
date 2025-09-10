using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

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

    public GameObject Board;

    [SerializeField] private Image IconImage;
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
        paricle.Play();
        Board.SetActive(true);
        IconImage.sprite = Asset_Mng.Get_Atlas(m_Data.Name);
        TitleText.text = m_Data.Name;
        SetBuildData(m_Data.timer);
    }
    public void SetBuildData(float time)
    {
        StartCoroutine(SliderFillCoroutine(time));

    }
    IEnumerator SliderFillCoroutine(float time)
    {
        float t = 0.0f;
        while(t <= time)
        {
            t += Time.deltaTime;
            FillSlilder.fillAmount = t / time;
            PercentageText.text = string.Format("{0:0.0}%", 
                FillSlilder.fillAmount * 100.0f);

            yield return null;
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
        if (other.gameObject.name != "Terrain")
        {
            SetMaterial_Color(1);
            CanBuild = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
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
