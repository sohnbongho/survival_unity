using UnityEngine;

public class Weather_Mng : MonoBehaviour
{
    [Header("## Sun And Night")]
    public Light DirectionalLight;
    public Vector3 SunRotationOffset;
    public Gradient SunColorGradient;

    [Range(0, 24)] public float CurrentTime = 12.0f; // 0~24시간 
    public float TimeSpeed = 60.0f;

    [Space(20f)]
    [Header("## Sun And Night")]
    public ParticleSystem RainParticleSystem;
    public float MinEmissionRate;
    public float MaxEmissionRate;
    public ParticleSystem.EmissionModule EmissionModule;

    [Space(20f)]
    [Header("## Wind ")]
    public Material WindMaterial;
    public float WindStrength = 0.0f;
    public float MinWindStrength = 0.25f;
    public float MaxWindStrength = 1.0f;

    private void Start()
    {
        EmissionModule = RainParticleSystem.emission;
        Delegate_Holder.RainIntensityChanged += UpdateRainEmission;
    }
    private void OnDestroy()
    {
        Delegate_Holder.RainIntensityChanged -= UpdateRainEmission;
    }

    private void Update()
    {
        UpdateTime();
        RotateSun();
        UpdateSunColor();

        float windStrength = Mathf.Lerp(MinWindStrength, MaxWindStrength, WindStrength);
        WindMaterial.SetFloat("_Wind_Strength", windStrength);

        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            Delegate_Holder.ChangeRainIntensity(0.1f);
        }
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            Delegate_Holder.ChangeRainIntensity(0.5f);
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            Delegate_Holder.ChangeRainIntensity(1.0f);
        }
    }
    public void UpdateRainEmission(float intensity)
    {
        float emissionRate = Mathf.Lerp(MinEmissionRate, MaxEmissionRate, intensity);
        EmissionModule.rateOverTime = emissionRate;
    }
    private void UpdateTime()
    {
        float timeSpeed = 24.0f / TimeSpeed;
        CurrentTime += Time.deltaTime * timeSpeed;
        if (CurrentTime >= 24.0f)
            CurrentTime = 0.0f;
    }

    private void RotateSun()
    {
        float timePercent = CurrentTime / 24.0f;
        float sunXRotation = Mathf.Lerp(-90.0f, 270.0f, timePercent);

        float yPercent = Mathf.Sin(timePercent * Mathf.PI); // 동->서쪽으로 뜨는 해
        float sunYRotation = Mathf.Lerp(-45.0f, 45.0f, yPercent); // 해의 각도 처리

        DirectionalLight.transform.rotation = Quaternion.Euler(sunXRotation,
            sunYRotation + SunRotationOffset.y,
            0);
    }
    private void UpdateSunColor()
    {
        float timePercent = CurrentTime / 24.0f;
        DirectionalLight.color = SunColorGradient.Evaluate(timePercent);
    }
}
