using UnityEngine;

public class Weather_Mng : MonoBehaviour
{
    public Light DirectionalLight;
    public Vector3 SunRotationOffset;
    public Gradient SunColorGradient;

    [Range(0, 24)] public float CurrentTime = 12.0f; // 0~24시간 
    public float TimeSpeed = 60.0f;

    private void Update()
    {
        UpdateTime();
        RotateSun();
        UpdateSunColor();
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
