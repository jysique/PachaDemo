using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Timers;

public class Testtimer : MonoBehaviour
{
    Timer timer;
    void Start()
    {
        timer = new Timer(2.0f, ()=>Print());
        // Debug.Log("Hola1");
        //TimersManager.SetTimer(this, 0.5f, () => Print());
        TimersManager.SetTimer(this, timer,true);
    }

    void ClearTimer()
    {
        //delegate converts null timer
        TimersManager.ClearTimer(timer.Delegate());
    }

    void Print() {
        //Debug.Log("Hola1");
    }
}
