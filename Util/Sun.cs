using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{
    public float dayHourDuration = 2;
    public DateTime currentTime = new DateTime(2023,6,3,7,0,0);
    public float angularTilt = 15;

    void Update()
    {
        var timePassed = Time.deltaTime * (24 / dayHourDuration);
        currentTime = currentTime.AddSeconds(timePassed);
        var time = (float)currentTime.Hour + ((float)currentTime.Minute / 60) + (((float)currentTime.Second / 60) / 60);
        time /= 24;
        var angle = 360 * time;
        transform.eulerAngles = new Vector3(angle - 90, angularTilt, 0);
    }
}
