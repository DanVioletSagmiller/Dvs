using Dvs;
using UnityEngine;

public class VideoSystem2 : MonoBehaviour, IVideoSystem
{
    public void Awake()
    {
        // Completed initialization
        Locator.Set<IVideoSystem>(this);
    }

    public bool IsPlaying { get; private set; }

    public void Play() => IsPlaying = true;

    public void Stop() => IsPlaying = false;
}