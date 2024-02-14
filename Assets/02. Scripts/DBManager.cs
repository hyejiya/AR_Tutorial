using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Playables;

public class DBManager : MonoBehaviour
{
    public string url = "https://docs.google.com/spreadsheets/d/10GaozCAYllIZpwaNcOVs_IMyFdZO7P77DjIqR2mWiBY/edit#gid=0";
    class POIDataList
    {
        public List<POIData> Places;
    }

    void Start()
    {
        ReadJson();

        StartCoroutine(RequestCo());
    }

    IEnumerator RequestCo()
    {
        UnityWebRequest data = UnityWebRequest.Get(url);

        yield return data.SendWebRequest(); //서버에 데이터 요청 후 올때 까지 대기

        switch (data.result)
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

        if(data.isDone)
        {
            string json = data.downloadHandler.text;

            string[] rows = json.Split('\n');
            for (int i = 0; i < rows.Length; i++)
            {
                string[] columns = rows[i].Split('\t');

                foreach (var column in columns)
                {
                    Debug.Log("line :" + i + ": " + column); //구글 스프레드 시트 사용 2~10번째
                }
            }
        }


        //DisplayText(json);


        
    }

    private void DisplayText(string json)
    {
        string[] rows = json.Split('\n');
        string[] colums = rows[0].Split('\t');
    }

    void ReadJson()
    {
        // Resources 폴더에서 JSON 파일을 로드
        TextAsset jsonFile = Resources.Load<TextAsset>("ROIInfo");

        // JSON 파일의 내용을 문자열로 변환
        string jsonString = jsonFile.text;

        // JSON 문자열을 클래스로 변환
        POIDataList poiList = JsonUtility.FromJson<POIDataList>(jsonString);
        
        foreach (POIData poiData in poiList.Places)
        {
            // 클래스의 내용을 출력
            Debug.Log("Name: " + poiData.name);
        }

    }

    /*    void Start()
    {
        // Resources 폴더에서 JSON 파일을 로드
        TextAsset jsonFile = Resources.Load<TextAsset>("ROIInfo");

        // JSON 파일의 내용을 문자열로 변환
        string jsonString = jsonFile.ToString();

        // JSON 문자열을 클래스로 변환
        POIData data = JsonUtility.FromJson<POIData>(jsonString);

        // 클래스의 내용을 출력
        Debug.Log("Name: " + data.name);
    }*/

    /*    void Start()
        {
            TextAsset json = Resources.Load<TextAsset>("ROIInfo");
            Debug.Log(json.text);

            data = JsonUtility.FromJson<POIDataList>(json.text);

            foreach (POIData po in data.pois)
            {
                Debug.Log(po.Name);
            }

        }*/
}
