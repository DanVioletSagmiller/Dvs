using UnityEngine;

public class ComplexSystem : MonoBehaviour
{
    public VideoSystem Video;

    private void Start()
    {
        Video.Play();
    }
}
