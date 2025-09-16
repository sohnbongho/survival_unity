using TMPro;
using UnityEngine;

public class CompassBar : MonoBehaviour
{
    private Transform PlayerTransform;
    public TextMeshProUGUI WestText, NorthText, EastText, SouthText;

    public float CompassWidth = 700.0f;

    [Header("## Settings")]
    public float MaxAlpha;
    public float MinAlpha;
    public float MaxScale;
    public float MinScale;

    private void Start()
    {
        PlayerTransform = P_Movement.instance.transform;
    }

    private void Update()
    {
        UpdateCompass();
    }
    private void UpdateCompass()
    {
        float heading = PlayerTransform.eulerAngles.y;

        SetPositions(WestText, heading, 90.0f);
        SetPositions(NorthText, heading, 180.0f);
        SetPositions(EastText, heading, 270.0f);
        SetPositions(SouthText, heading, 0.0f);
    }

    private void SetPositions(TextMeshProUGUI text, float heading, float offset)
    {
        // 기준 각도에 따라 각 텍스트가 중앙 기준으로 이동하도록 계산
        float relativeAngle = (heading - offset + 360.0f) % 360.0f; // 각도 보정
        float normalized = relativeAngle / 360.0f;  // 0~1사이의 값으로 정규화

        float xPosition = Mathf.Lerp(-CompassWidth, CompassWidth, normalized);
        text.rectTransform.anchoredPosition = new Vector2(xPosition, text.rectTransform.anchoredPosition.y);

        // 중심에서의 거리 계산 (0이 중앙, 1이 최대거리)
        float distanceFromCenter = Mathf.Abs(xPosition / CompassWidth);
        float alpha = Mathf.Lerp(MaxAlpha, MinAlpha, distanceFromCenter);
        float scale = Mathf.Lerp(MaxScale, MinScale, distanceFromCenter);

        Color color = text.color;
        color.a = alpha;
        text.color = color;

        text.rectTransform.localScale = Vector3.one * scale;

    }
}
