using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

/// <summary>
/// �� Image Library�� �� �̹����� �´� 3D������Ʈ�� Resources �������� �ҷ��� �����Ѵ�
/// </summary>
[RequireComponent(typeof(ARTrackedImageManager))]
public class ImageDetection : MonoBehaviour
{
    ARTrackedImageManager ImageManager;

    void Awake()
    {
        ImageManager = GetComponent<ARTrackedImageManager>();

        //�̺�Ʈ �Լ� : trackedImage�� ��ȭ�� ��
        ImageManager.trackedImagesChanged += OnImageTrackedEvent;
    }

    void OnImageTrackedEvent(ARTrackedImagesChangedEventArgs arg)
    {
        foreach(ARTrackedImage trackedImage in arg.added)
        {
            //reference Image Library�� ����
            string imageName = trackedImage.referenceImage.name;
            Debug.Log(imageName);

            //Resources �������� �ε�
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
