using UnityEngine;
using UnityEngine.UI;

public class CompassReader : MonoBehaviour
{
    public RectTransform compassNeedle; // 이미지의 RectTransform
    public Text compassValueText;   // Compass 값을 표시할 텍스트 메시

    void Start()
    {
        // Compass 센서 활성화
        Input.compass.enabled = true;
    }

    void Update()
    {
        // Compass 센서에서 방위각 가져오기
        float heading = Input.compass.trueHeading;

        // 이미지 회전
        compassNeedle.localEulerAngles = new Vector3(0f, 0f, -heading);

        // 텍스트로 방위각 표시
        compassValueText.text = heading.ToString("F0") + "º";
    }

    void OnDestroy()
    {
        // Compass 센서 비활성화
        Input.compass.enabled = false;
    }
}
