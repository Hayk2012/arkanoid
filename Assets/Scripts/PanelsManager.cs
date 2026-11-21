using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using Random = UnityEngine.Random;
using System.Collections;
using TMPro;

public class PanelsManager : MonoBehaviour
{
    public static bool LevelEnded;

    public CanvasGroup LevelEndPanel;
    public CanvasGroup LoadingScreen;
    public CanvasGroup WinPanel;
    public Button RestartLevelButton;
    public Button[] QuitMenuButtons;
    public Button NextLevelButton;
    public RectTransform[] WidthParts;
    public RectTransform[] HeightParts;

    private enum AnimationType
    {
        Width,
        Height
    }
    private AnimationType _animationType;
    private RectTransform _winPanelTransform;

    private void Awake()
    {
        LevelEnded = false;

        GameManager.OnAttemptsEnded += ActivateLevelEndPanel;
        GameManager.OnBlocksCountEnded += ActivateLevelWinPanel;
        RestartLevelButton.onClick.AddListener(RestartLevel);
        NextLevelButton.onClick.AddListener(NextLevel);
        QuitMenu(QuitMenuButtons);
        _winPanelTransform = WinPanel.GetComponent<RectTransform>();

        if (SceneManager.GetActiveScene().buildIndex == 16)
        {
            NextLevelButton.gameObject.SetActive(false);
            var childText = WinPanel.transform.GetChild(0);
            var childButton = WinPanel.transform.GetChild(3);

            var button = childButton.GetComponent<Button>();
            button.interactable = true;
            var text = childText.GetComponent<TMP_Text>();
            text.fontSize = 100;
            text.text = $"Congrats, you finish all levels! <3";
        }
    }

    private void OnDestroy()
    {
        GameManager.OnAttemptsEnded -= ActivateLevelEndPanel;
        GameManager.OnBlocksCountEnded -= ActivateLevelWinPanel;
    }

    private void ActivateLevelWinPanel()
    {
        LevelEnded = true;
        Vector3 startPosition = new Vector3(0, 1080);
        _winPanelTransform.anchoredPosition = startPosition;
        WinPanel.alpha = 0;
        WinPanel.gameObject.SetActive(true);

        Sequence seq = DOTween.Sequence();
        seq.Join(WinPanel.DOFade(1, 1)).Join(_winPanelTransform.DOAnchorPos(Vector3.zero, 1)).
            OnComplete(() => seq.Kill(true));
    }

    private void ActivateLevelEndPanel()
    {
        LevelEnded = true;
        LevelEndPanel.gameObject.SetActive(true);
        LevelEndPanel.transform.localScale = Vector3.zero;
        LevelEndPanel.alpha = 0;

        Sequence seq = DOTween.Sequence();
        seq.Join(LevelEndPanel.DOFade(1, 1f)).Join(LevelEndPanel.transform.DOScale(1f, 1f)).OnComplete(() => seq.Kill(true));
    }

    private void RestartLevel()
    {
        RestartLevelButton.transform.DOScale(0.8f, 0.3f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            RestartLevelButton.transform.DOScale(1f, 0.3f).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                ChooseType();
                StartCoroutine(Delay());
            });

            IEnumerator Delay()
            {
                yield return new WaitForSeconds(1f);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                RestartLevelButton.transform.DOKill();
            }
        });
    }

    private void QuitMenu(Button[] buttons)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;

            buttons[i].onClick.AddListener(() =>
            {
                buttons[index].transform.DOScale(0.8f, 0.3f).SetEase(Ease.OutQuad).
                OnComplete(() =>
                {
                    buttons[index].transform.DOScale(1f, 0.3f).SetEase(Ease.OutQuad).OnComplete(() =>
                    {
                        LoadingScreen.gameObject.SetActive(true);
                        LoadingScreen.alpha = 0;
                        LoadingScreen.DOFade(1f, 2f).OnComplete(() => SceneManager.LoadScene(0));
                    });
                });
               
            });            
        }       
    }

    private void NextLevel()
    {
        NextLevelButton.transform.DOScale(0.8f, 0.3f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            NextLevelButton.transform.DOScale(1f, 0.3f).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                ChooseType();
                StartCoroutine(Delay());
            });
        });

        IEnumerator Delay()
        {
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            NextLevelButton.transform.DOKill();
        }
    }    
    
    private void AnimateWidthParts()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(WidthParts[0].DOAnchorPos(new Vector3(551, 0), 1f)).Join(WidthParts[^1].DOAnchorPos(new Vector3(-551, 0), 1f)).
            OnComplete(() => sequence.Kill(true));
    }

    private void AnimateHeightParts()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(HeightParts[0].DOAnchorPos(new Vector3(0, 0), 1f)).Join(HeightParts[^1].DOAnchorPos(new Vector3(0, 0), 1f)).
            OnComplete(() => sequence.Kill(true));
    }

    private void SetType()
    {
        switch (_animationType)
        {
            case AnimationType.Width: AnimateWidthParts(); break;
            case AnimationType.Height: AnimateHeightParts(); break;
        }
    }

    public void ChooseType()
    {
        AnimationType[] indexes = (AnimationType[])Enum.GetValues(typeof(AnimationType));
        _animationType = indexes[Random.Range(0, indexes.Length)];
        SetType();
    }
}
