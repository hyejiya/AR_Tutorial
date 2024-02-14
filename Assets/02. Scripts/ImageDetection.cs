using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

/// <summary>
/// 각 Image Library의 각 이미지에 맞는 3D오브젝트를 Resources 폴더에서 불러와 생성한다
/// </summary>
[RequireComponent(typeof(ARTrackedImageManager))]
public class ImageDetection : MonoBehaviour
{
    ARTrackedImageManager ImageManager;

    void Awake()
    {
        ImageManager = GetComponent<ARTrackedImageManager>();

        //이벤트 함수 : trackedImage가 변화될 때
        ImageManager.trackedImagesChanged += OnImageTrackedEvent;
    }

    void OnImageTrackedEvent(ARTrackedImagesChangedEventArgs arg)
    {
        foreach(ARTrackedImage trackedImage in arg.added)
        {
            //reference Image Library로 접근
            string imageName = trackedImage.referenceImage.name;
            Debug.Log(imageName);

            //Resources 폴더에서 로드
            GameObject prefab = Resources.Load<GameObject>(imageName);

            if(prefab != null)
            {
                GameObject obj = Instantiate(prefab, trackedImage.transform.position, trackedImage.transform.rotation);
                obj.transform.SetParent(trackedImage.transform);
            }

        }

        foreach(ARTrackedImage trackedImage in arg.updated)
        {
            if(trackedImage.transform.childCount > 0)
            {
                trackedImage.transform.GetChild(0).position = trackedImage.transform.position;
                trackedImage.transform.GetChild(0).rotation = trackedImage.transform.rotation;
                trackedImage.transform.GetChild(0).gameObject.SetActive(true);
            }
        }

        foreach(ARTrackedImage trackedImage in arg.removed)
        {
                trackedImage.transform.GetChild(0).gameObject.SetActive(false);
            
        }
    }

    private void OnDisable()
    {
        ImageManager.trackedImagesChanged -= OnImageTrackedEvent;
    }
}
