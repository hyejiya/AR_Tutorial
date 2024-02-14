using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Networking;
using static UnityEngine.XR.ARSubsystems.XRCpuImage;

/// <summary>
/// Naver Cloud Platform의 Maps API 중 Directions5 API에 Directions5 요청
/// </summary>
public class DirectionManager : MonoBehaviour
{
    [SerializeField] string baseurl = "https://naveropenapi.apigw.ntruss.com/map-direction/v1/driving";
    [SerializeField] string clientID = "";
    [SerializeField] string clientSerect = "";
    [SerializeField] string mypoint = "";
    [SerializeField] string destination = "";
    [SerializeField]enum OptionCode
    {
        trafast, 
        tracomfort,  
        traoptimal,  
        traavoidtoll,    
        traavoidcaronly 
    }

    [SerializeField] OptionCode optionCode = OptionCode.trafast;

    IEnumerator Start()
    {
        string apiRequestURL = baseurl + $"?start={mypoint}&goal={destination}&option={optionCode.ToString()}";

        UnityWebRequest request = UnityWebRequest.Get(apiRequestURL);
        request.SetRequestHeader("X-NCP-APIGW-API-KEY-ID", clientID);
        request.SetRequestHeader("X-NCP-APIGW-API-KEY", clientSerect);

        yield return request.SendWebRequest();

        switch (request.result)
        {
            case UnityWebRequest.Result.Success:
                break;
            case UnityWebRequest.Result.ConnectionError:
                yield break;
            case UnityWebRequest.Result.ProtocolError:
                yield break;
            case UnityWebRequest.Result.DataProcessingError:
                yield break;
        }

        if(request.isDone)
        {
            string json = request.downloadHandler.text;

            print(json);
        }
    }

  
}
