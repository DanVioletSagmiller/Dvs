using Dvs;
using System;
using UnityEngine;

public class ComplexSystem4 : MonoBehaviour
{
    private IVideoSystem Video;

    public ComplexSystem4()
        => Locator.Observe<IVideoSystem>(OnVideoChange);

    private void OnVideoChange(IVideoSystem obj) 
        => Video = obj;

    private void Start()
        => Video.Play();
}