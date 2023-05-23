using UnityEngine;

public class ComplexSystem2 : MonoBehaviour
{
    private IVideoSystem _video;
    public IVideoSystem Video
    {
        get => _video;
        set
        {
            if (value is VideoSystem) _Video = (VideoSystem)value;
            else _Video = null;
            _video = value;
        }
    }

    [SerializeField] private VideoSystem _Video;

    public void OnValidate()
    {
        if (_Video == null) return;
        _video = _Video;
    }

    private void Start()
    {
        if (_video == null) _video = _Video;
        Video.Play();
    }
}
