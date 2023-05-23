using System;
using System.ComponentModel;
using UnityEngine;

[Serializable]
public class Field<I, T> where T : class, I where I : class
{
    [SerializeField]
    private T _Value;

    private I _IValue;

    public I Value
    {
        get => _Value == null ? _IValue : _Value;
        set
        {
            if (value == null)
            {
                _Value = null;
                _IValue = null;
                return;
            }

            _IValue = value;
            _Value = null;
        }
    }
}

public class ComplexSystem3 : MonoBehaviour
{
    public Field<IVideoSystem, VideoSystem> Video = new Field<IVideoSystem, VideoSystem>();

    private void Start()
    {
        Video.Value.Play();
    }
}