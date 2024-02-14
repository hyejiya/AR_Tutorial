using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//POI 데이터 저장을 위한 컨테이너
[Serializable]
public struct POIData
{
    public string name;
/*    private string description;
    private float latitude;
    private float longitude;
    private float altitude;*/

    public POIData(string name)
    {
        this.name = name;
    }

/*    public POIData(string name, string description, float latitude, float longitude, float altitude)
    {
        this.name = name;
        this.description = description;
        this.latitude = latitude;
        this.longitude = longitude;
        this.altitude = altitude;
    } */
}
