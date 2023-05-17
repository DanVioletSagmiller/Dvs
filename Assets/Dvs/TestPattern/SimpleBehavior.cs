using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBehavior : MonoBehaviour 
{
    public bool FirstFrameHappened = false;

    public bool StartHappened = false;


    public void Start()
    {
        StartHappened = true;
    }

    public void Update()
    {
        FirstFrameHappened = true;
    }
}
