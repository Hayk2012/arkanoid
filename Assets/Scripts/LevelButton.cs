using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private int _levelNumber;
    [SerializeField] private Sprite _unlockedSprite;

    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.interactable = _levelNumber <= ProgressService.UnlockedLevel;
        if(_button.interactable)
        {
            _button.GetComponent<Image>().sprite = _unlockedSprite;
        }

        _button.onClick.AddListener(LoadLevel);
    }

    private void LoadLevel()
    {
        _button.gameObject.transform.DOScale(0.9f, 0.1f).OnComplete(() =>
        {
            _button.gameObject.transform.DOScale(1f, 0.1f).OnComplete(() =>
            {
                SceneManager.LoadScene(_levelNumber);
            });
        });
       
    }
}
