using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLevelVisualizer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerLvlCounter _lvlCounter;
    [SerializeField] private TMP_Text _levelText;

    [Header("XP Bar Visuals")]
    [SerializeField] private SpriteRenderer _xpBarImage;
    [SerializeField] private Sprite[] _xpStageSprites;

    private void Awake()
    {
        _lvlCounter.OnXpChange += UpdateXpView;
        _lvlCounter.OnLvlChange += UpdateLevelText;
    }

    private void Start()
    {
        UpdateLevelText(_lvlCounter.CurrentLevel);
        UpdateXpView(_lvlCounter.CurrentXp);
    }

    private void OnDestroy()
    {
        _lvlCounter.OnXpChange -= UpdateXpView;
        _lvlCounter.OnLvlChange -= UpdateLevelText;
    }

    private void UpdateXpView(float currentXp)
    {
        if (_xpBarImage == null || _xpStageSprites.Length == 0) return;

        float neededXp = _lvlCounter.GetXpThreshold(_lvlCounter.CurrentLevel);

        if (neededXp <= 0) return;

        float progress = currentXp / neededXp;

        int spriteIndex = Mathf.FloorToInt(progress * 10);

        spriteIndex = Mathf.Clamp(spriteIndex, 0, 9);

        _xpBarImage.sprite = _xpStageSprites[spriteIndex];
    }

    private void UpdateLevelText(int level)
    {
        if (level >= _xpStageSprites.Length)
        {
            if (_levelText == null) return;
            _levelText.text = $"lvl: MAX";
            return;
        }

        if (_levelText == null) return;
        _levelText.text = $"lvl: {level}";
    }
}