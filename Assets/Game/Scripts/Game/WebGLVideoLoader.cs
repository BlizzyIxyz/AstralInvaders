using UnityEngine;
using UnityEngine.Video;

public class WebGLVideoLoader : MonoBehaviour
{
    [SerializeField] private VideoPlayer _player;
    [SerializeField] private string _fileName = "vid.mp4";

    private void Awake()
    {
        _player.url = System.IO.Path.Combine(Application.streamingAssetsPath, _fileName);
        _player.Prepare();
    }

    public void Play()
    {
        _player.Play();
    }
}