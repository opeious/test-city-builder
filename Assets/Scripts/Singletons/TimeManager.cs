using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public delegate void Action();
    
    public static event Action PeriodicUpdate1s;
    public static void RaisePeriodicUpdate1s() { if (PeriodicUpdate1s != null) PeriodicUpdate1s();}


    float timePassed = 0f;

    private void Update() {
        timePassed += Time.deltaTime;
        if(timePassed > 1f) {
            timePassed = 0f;
            RaisePeriodicUpdate1s();
        }
    }
}
