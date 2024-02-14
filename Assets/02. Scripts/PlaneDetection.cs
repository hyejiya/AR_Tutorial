using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

//ARRay�� �߻��Ͽ� Plane�� ������ Indicator�� ����ش�.
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
    private float initialDistance = 0f; // �ʱ� �� ��ġ ������ �Ÿ�
    private float initialScaleMagnitude = 1f; // �ʱ� ������Ʈ�� ũ��
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
        //Screen�� �߽ɿ��� ARRay�� ���� ���� �߻��Ѵ�. 
        Vector2 screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);

        //RaycastHit hitinfo;

        //ARRay�� ���� ������
        List<ARRaycastHit> hitinfo = new List<ARRaycastHit>();

        //ARRay�� �߻�
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
        // ȭ�鿡�� ��ġ �̺�Ʈ ����
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if(touch.phase == UnityEngine.TouchPhase.Began)
            {
                Vector2 touchPoint = touch.position;
                List<ARRaycastHit> hitinfo = new List<ARRaycastHit>();
                if (raycastManager.Raycast(touchPoint, hitinfo, TrackableType.Planes))
                {
                    // ��ġ�� ������ Plane ���� ���� ��
                    Pose hitPose = hitinfo[0].pose;
                    // ������ ������ ������Ʈ�� �ִٸ� ����
                    if (instantiatedObject != null)
                    {
                        Destroy(instantiatedObject);
                    }

                    // ������Ʈ�� �����ϰ� �ʱ� ũ�� ����
                    instantiatedObject = Instantiate(modelPrefab, hitPose.position, hitPose.rotation);
                    initialScaleMagnitude = instantiatedObject.transform.localScale.magnitude;
                    touchStartPos = touchPoint;

                    // �� ��ġ ������ �ʱ� �Ÿ� ���
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

            // �� ��ġ ������ �ʱ� �Ÿ��� 0�̸� �ʱ�ȭ
            if (initialDistance == 0f)
            {
                initialDistance = currentDistance;
            }

            // �� ��ġ ������ �Ÿ� ��ȭ�� ���
            float touchDeltaMagnitude = currentDistance - initialDistance;

            // ������ ������Ʈ�� �ְ�, Ȯ��/��� ���� ���
            if (instantiatedObject != null)
            {
                // ���ο� ũ�� ���
                float newScaleMagnitude = initialScaleMagnitude + touchDeltaMagnitude * 0.01f;
                Vector3 newScale = Vector3.one * newScaleMagnitude;

                // ������Ʈ�� ũ�� ����
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

                // Ŭ���� ������ Plane ���� ���� ��
                Pose hitPose = hitinfo[0].pose;
                // ��ġ�� ������ 3D �� ����
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
