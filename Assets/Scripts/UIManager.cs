using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TMP_Text BlocksCountText;
    public TMP_Text AttemptsCountText;
    public TMP_Text LevelIndexText;

    private int _blocksCount;

    private void Awake()
    {
        GameManager.OnBlocksCountUpdated += ShowBlocksCount;
        GameManager.OnAttemptsCountUpdated += ShowAttempts;
        GameManager.OnLevelLoaded += ShowLevelIndex;

        ShowBlocksCount(_blocksCount);
    }

    private void OnDestroy()
    {
        GameManager.OnBlocksCountUpdated -= ShowBlocksCount;
        GameManager.OnAttemptsCountUpdated -= ShowAttempts;
        GameManager.OnLevelLoaded -= ShowLevelIndex;
    }

    private void ShowBlocksCount(int value)
    {
        _blocksCount = value;
        BlocksCountText.text = $"{value}";
    }

    private void ShowAttempts(int value)
    {
        AttemptsCountText.text = $"{value}";
    }

    private void ShowLevelIndex(int value)
    {
        LevelIndexText.text = $"{value}";
    }
}
