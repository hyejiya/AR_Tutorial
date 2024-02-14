using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

//ARRay를 발사하여 Plane에 닿으면 Indicator를 띄워준다.
[RequireComponent(typeof(ARRaycastManager))]
public class PlaneDetection : MonoBehaviour
{
    [SerializeField] Transform indicator;
    [SerializeField] GameObject modelPrefab;
    [SerializeField] float zoomSpeed = 0.05f;
    ARRaycastManager raycastManager;
    GameObject instantiatedObject;
    private Vector2 touchStartPos;
    private Vector3 initialScale = Vector3.one;
    private float initialDistance = 0f; // 초기 두 터치 사이의 거리
    private float initialScaleMagnitude = 1f; // 초기 오브젝트의 크기
    void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        CastRayFromScreenCenter();

        CastRayByScreenTouch();
        ZoomInOutObjectByPinch();

        CastRayByScreenClick();
        ZoomInOutObjectByWheel();
    }

    void CastRayFromScreenCenter()
    {
        //Screen의 중심에서 ARRay를 전방 으로 발사한다. 
        Vector2 screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);

        //RaycastHit hitinfo;

        //ARRay가 닿은 정보들
        List<ARRaycastHit> hitinfo = new List<ARRaycastHit>();

        //ARRay를 발사
        if (raycastManager.Raycast(screenCenter, hitinfo, TrackableType.Planes))
        {
            indicator.position = hitinfo[0].pose.position;
            indicator.rotation = hitinfo[0].pose.rotation;
            indicator.gameObject.SetActive(true);
        }
        else
        {
            indicator.gameObject.SetActive(false);
        }
    }

    void CastRayByScreenTouch()
    {
        // 화면에서 터치 이벤트 감지
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if(touch.phase == UnityEngine.TouchPhase.Began)
            {
                Vector2 touchPoint = touch.position;
                List<ARRaycastHit> hitinfo = new List<ARRaycastHit>();
                if (raycastManager.Raycast(touchPoint, hitinfo, TrackableType.Planes))
                {
                    // 터치한 지점이 Plane 위에 있을 때
                    Pose hitPose = hitinfo[0].pose;
                    // 이전에 생성된 오브젝트가 있다면 제거
                    if (instantiatedObject != null)
                    {
                        Destroy(instantiatedObject);
                    }

                    // 오브젝트를 생성하고 초기 크기 설정
                    instantiatedObject = Instantiate(modelPrefab, hitPose.position, hitPose.rotation);
                    initialScaleMagnitude = instantiatedObject.transform.localScale.magnitude;
                    touchStartPos = touchPoint;

                    // 두 터치 사이의 초기 거리 계산
                    initialDistance = 0f;

                }
            }
        }
    }
    
    void ZoomInOutObjectByPinch()
    {
        if (Input.touchCount == 2)
        {
            Vector2 currentTouch0Pos = Input.GetTouch(0).position;
            Vector2 currentTouch1Pos = Input.GetTouch(1).position;

            float currentDistance = Vector2.Distance(currentTouch0Pos, currentTouch1Pos);

            // 두 터치 사이의 초기 거리가 0이면 초기화
            if (initialDistance == 0f)
            {
                initialDistance = currentDistance;
            }

            // 두 터치 사이의 거리 변화량 계산
            float touchDeltaMagnitude = currentDistance - initialDistance;

            // 생성된 오브젝트가 있고, 확대/축소 중인 경우
            if (instantiatedObject != null)
            {
                // 새로운 크기 계산
                float newScaleMagnitude = initialScaleMagnitude + touchDeltaMagnitude * 0.01f;
                Vector3 newScale = Vector3.one * newScaleMagnitude;

                // 오브젝트의 크기 조정
                instantiatedObject.transform.localScale = newScale;
            }
        }
    }

    void CastRayByScreenClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 clickPosition = Input.mousePosition;
            List<ARRaycastHit> hitinfo = new List<ARRaycastHit>();
            if (raycastManager.Raycast(clickPosition, hitinfo, TrackableType.Planes))
            {
                if (instantiatedObject != null)
                {
                    Destroy(instantiatedObject);
                }

                // 클릭한 지점이 Plane 위에 있을 때
                Pose hitPose = hitinfo[0].pose;
                // 터치한 지점에 3D 모델 생성
                instantiatedObject = Instantiate(modelPrefab, hitPose.position, hitPose.rotation);
            }
        }
    }

    void ZoomInOutObjectByWheel()
    {
        
        if(Input.mouseScrollDelta.y != 0)
        {
            float scroll = Input.mouseScrollDelta.y;
            if(instantiatedObject != null)
            {
                instantiatedObject.transform.localScale += Vector3.one * scroll * zoomSpeed;
                instantiatedObject.transform.localScale = new Vector3(
                    Mathf.Clamp(instantiatedObject.transform.localScale.x, 0.1f, 5f),
                    Mathf.Clamp(instantiatedObject.transform.localScale.y, 0.1f, 5f),
                    Mathf.Clamp(instantiatedObject.transform.localScale.z, 0.1f, 5f));
            }
        }
    }

}
