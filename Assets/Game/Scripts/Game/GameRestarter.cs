using UnityEngine;
using UnityEngine.SceneManagement;

public class GameRestarter : MonoBehaviour
{
    [SerializeField] private string _sceneName;

    public void Restart()
    {
        SceneManager.LoadScene(_sceneName);
    }
}