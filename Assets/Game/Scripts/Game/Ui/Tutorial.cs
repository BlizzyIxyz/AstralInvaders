using UnityEngine;
using UnityEngine.Playables;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private PlayableAsset _showPlayable;
    [SerializeField] private PlayableAsset _hidePlayable;
    [SerializeField] private PlayableDirector _playableDirector;

    public void Show()
    {
        _playableDirector.playableAsset = _showPlayable;
        _playableDirector.Play();
    }

    public void Hide()
    {
        _playableDirector.playableAsset = _hidePlayable;
        _playableDirector.Play();
    }
}
