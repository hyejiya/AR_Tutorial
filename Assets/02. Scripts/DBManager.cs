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

        yield return data.SendWebRequest(); //������ ������ ��û �� �ö� ���� ���

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
                    Debug.Log("line :" + i + ": " + column); //���� �������� ��Ʈ ��� 2~10��°
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
        // Resources �������� JSON ������ �ε�
        TextAsset jsonFile = Resources.Load<TextAsset>("ROIInfo");

        // JSON ������ ������ ���ڿ��� ��ȯ
        string jsonString = jsonFile.text;

        // JSON ���ڿ��� Ŭ������ ��ȯ
        POIDataList poiList = JsonUtility.FromJson<POIDataList>(jsonString);
        
        foreach (POIData poiData in poiList.Places)
        {
            // Ŭ������ ������ ���
            Debug.Log("Name: " + poiData.name);
        }

    }

    /*    void Start()
    {
        // Resources �������� JSON ������ �ε�
        TextAsset jsonFile = Resources.Load<TextAsset>("ROIInfo");

        // JSON ������ ������ ���ڿ��� ��ȯ
        string jsonString = jsonFile.ToString();

        // JSON ���ڿ��� Ŭ������ ��ȯ
        POIData data = JsonUtility.FromJson<POIData>(jsonString);

        // Ŭ������ ������ ���
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
