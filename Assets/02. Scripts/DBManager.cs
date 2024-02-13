using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBManager : MonoBehaviour
{    
    class POIDataList
    {
        public List<POIData> pois = new List<POIData>();
    }

    POIDataList data = new POIDataList();  
    void Start()
    {
        TextAsset json = Resources.Load<TextAsset>("ROIInfo");

        data = JsonUtility.FromJson<POIDataList>(json.ToString());

        Debug.Log(data.pois[0].name);
    }
}
