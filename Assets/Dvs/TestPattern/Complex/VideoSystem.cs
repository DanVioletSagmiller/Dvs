using UnityEngine;

public interface IVideoSystem
{
    bool IsPlaying { get; }
    void Play();
    void Stop();
}

public class VideoSystem : MonoBehaviour, IVideoSystem
{
    private bool IsInited = false;

    public bool IsPlaying { get; private set; }

    public void Play() { if (IsInited) IsPlaying = true; }

    public void Stop() { if (IsInited) IsPlaying = false; }

    private void Start()
    {
        IsInited = true;
    }
}