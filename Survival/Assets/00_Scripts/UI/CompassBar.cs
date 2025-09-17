using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MarkerInfo
{
    public Transform TargetTransform;
    public RectTransform MarkerUI;
    public string Key;
    public Image MarkerIcon;
    public TextMeshProUGUI MarkerText;

    public MarkerInfo(Transform targetTransform, RectTransform markerUI, string key)
    {
        TargetTransform = targetTransform;
        MarkerUI = markerUI;
        Key = key;

        MarkerIcon = markerUI.Find("Icon").GetComponent<Image>();
        MarkerText = markerUI.Find("MText").GetComponent<TextMeshProUGUI>();

        MarkerIcon.sprite = Asset_Mng.Get_Atlas(Key);
    }
}

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

    [Header("## Other Transform")]
    public static GameObject MarkerPrefab;
    public static Transform MarkerParent;
    public static List<MarkerInfo> ActiveMarkers = new List<MarkerInfo>();

    private void Start()
    {
        PlayerTransform = P_Movement.instance.transform;
        MarkerPrefab = transform.Find("CompassMarker").gameObject;
        MarkerPrefab.SetActive(false);
        MarkerParent = transform.Find("Mask").transform;
    }

    private void Update()
    {
        UpdateCompass();
        UpdateMarkers();
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

    public static void AddMarker(Transform targetTransform, string key)
    {
        if (ActiveMarkers.Exists(m => m.TargetTransform == targetTransform))
            return;

        GameObject marker = Instantiate(MarkerPrefab, MarkerParent);
        marker.SetActive(true);
        marker.name = "Marker:" + targetTransform.name;
        RectTransform markerRect = marker.GetComponent<RectTransform>();
        ActiveMarkers.Add(new MarkerInfo(targetTransform, markerRect, key));
    }

    public void UpdateMarkers()
    {
        for (int i = ActiveMarkers.Count - 1; i >= 0; i--)
        {
            MarkerInfo markerInfo = ActiveMarkers[i];
            if (markerInfo.TargetTransform == null)
            {
                Destroy(markerInfo.MarkerUI.gameObject);
                ActiveMarkers.RemoveAt(i);
                continue;
            }
            float heading = PlayerTransform.eulerAngles.y;
            Vector3 directionToTarget = markerInfo.TargetTransform.position - PlayerTransform.position;
            float distance = Vector3.Distance(markerInfo.TargetTransform.position, PlayerTransform.position);
            float targetAngle = Mathf.Atan2(-directionToTarget.x, -directionToTarget.z) * Mathf.Rad2Deg; // 각도 생성

            float relativeAngle = (heading - targetAngle + 360.0f) % 360.0f;
            float normalizedAngle = relativeAngle / 360.0f; // 

            float xPosition = Mathf.Lerp(-CompassWidth, CompassWidth, normalizedAngle);
            markerInfo.MarkerUI.anchoredPosition = new Vector2(xPosition, markerInfo.MarkerUI.anchoredPosition.y);
            markerInfo.MarkerText.text = string.Format("{0:0.0} m", distance);

        }

    }
}
