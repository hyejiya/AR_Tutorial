using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;
using System.Collections;

public class GPSTextDisplay : MonoBehaviour
{
    public Text latitudeText; // 위도를 표시할 텍스트 UI
    public Text longitudeText; // 경도를 표시할 텍스트 UI


    IEnumerator Start()
    {
        // GPS 권한 요청
        yield return RequestGPSPermission();

        // GPS 권한이 허용된 경우에만 위치 정보를 가져와 출력
        if (Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Input.location.Start();
            while (Input.location.status == LocationServiceStatus.Initializing)
                yield return null;

            if (Input.location.status == LocationServiceStatus.Failed)
            {
                Debug.LogError("GPS 초기화 실패");
                yield break;
            }

            // GPS 서비스가 활성화되고 위치 정보를 업데이트할 때마다 경도와 위도를 UI에 출력
            while (true)
            {
                latitudeText.text = "위도: " + Input.location.lastData.latitude.ToString();
                longitudeText.text = "경도: " + Input.location.lastData.longitude.ToString();
                yield return new WaitForSeconds(1f); // 1초마다 업데이트
            }
        }
        else
        {
            Debug.LogError("GPS 권한이 거부되었습니다.");
        }
    }

    // GPS 권한 요청하는 함수
    IEnumerator RequestGPSPermission()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
            while (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
                yield return null;
        }
    }

    // 스크립트 파괴 시 GPS 서비스 종료
    void OnDestroy()
    {
        Input.location.Stop();
    }
}
