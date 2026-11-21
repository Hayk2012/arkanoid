using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static event Action OnGamePaused;
    public static event Action OnGameResumed;
    public static bool IsLeft = false;

    public RectTransform PausePanel;

    public TMP_Text PlatformSpeedText;
    public TMP_Text BallBounceText;

    public Button OpenMenuButton;
    public Button RestartLevelButton;
    public Button PauseGameButton;
    public Button ResumeGameButton;
    public Button IncreaseSpeedPlatformButton;
    public Button DecreaseSpeedPlatformButton;
    public Button IncreaseBallBounceButton;
    public Button DecreaseBallBounceButton;

    private float _time = 0.1f;
    private float _scaleMultiplier = 0.8f;
    private Platform _platform;
    private Ball _ball;

    private void Awake()
    {
        PauseGameButton.onClick.AddListener(GamePause);
        ResumeGameButton.onClick.AddListener(GameResume);

        _platform = FindFirstObjectByType<Platform>();
        _ball = FindFirstObjectByType<Ball>();

        PlatformSpeedText.text = $"{_platform.MoveSpeed}";
        BallBounceText.text = $"{_ball.BounceForce}";

        IncreaseSpeedPlatformButton.onClick.AddListener(() =>
        {
            IsLeft = false;
            IncreaseSpeedPlatformButton.transform.DOScale(0.9f, 0.1f).OnComplete(() =>
            {
                IncreaseSpeedPlatformButton.transform.DOScale(1f, 0.1f).OnComplete(() =>
                {
                    if (_platform.MoveSpeed < 50)
                    {
                        FindFirstObjectByType<PlatformSpeedButton>().ShowAnimation();
                        _platform.MoveSpeed += 5;
                        PlatformSpeedText.text = $"{_platform.MoveSpeed}";
                        IncreaseSpeedPlatformButton.transform.DOKill();
                    }

                });
            });
        });

        DecreaseSpeedPlatformButton.onClick.AddListener(() =>
        {
            IsLeft = true;
            DecreaseSpeedPlatformButton.transform.DOScale(0.9f, 0.1f).OnComplete(() =>
            {
                DecreaseSpeedPlatformButton.transform.DOScale(1f, 0.1f).OnComplete(() =>
                {
                    if (_platform.MoveSpeed > 10)
                    {
                        FindFirstObjectByType<PlatformSpeedButton>().ShowAnimation();
                        _platform.MoveSpeed -= 5;
                        PlatformSpeedText.text = $"{_platform.MoveSpeed}";
                        DecreaseSpeedPlatformButton.transform.DOKill();
                    }
                });
            });
        });

        IncreaseBallBounceButton.onClick.AddListener(() =>
        {
            IsLeft = false;
            IncreaseBallBounceButton.transform.DOScale(0.9f, 0.1f).OnComplete(() =>
            {
                IncreaseBallBounceButton.transform.DOScale(1f, 0.1f).OnComplete(() =>
                {
                    if (_ball.BounceForce < 20)
                    {
                        FindFirstObjectByType<BounceBallButton>().ShowAnimation();
                        _ball.BounceForce += 1;
                        BallBounceText.text = $"{_ball.BounceForce}";
                        IncreaseBallBounceButton.transform.DOKill();
                    }

                });
            });
        });

        DecreaseBallBounceButton.onClick.AddListener(() =>
        {
            IsLeft = true;
            DecreaseBallBounceButton.transform.DOScale(0.9f, 0.1f).OnComplete(() =>
            {
                DecreaseBallBounceButton.transform.DOScale(1f, 0.1f).OnComplete(() =>
                {
                    if (_ball.BounceForce > 10)
                    {
                        FindFirstObjectByType<BounceBallButton>().ShowAnimation();
                        _ball.BounceForce -= 1;
                        BallBounceText.text = $"{_ball.BounceForce}";
                        DecreaseBallBounceButton.transform.DOKill();
                    }

                });
            });
        });

        RestartLevelButton.onClick.AddListener(() =>
        {
            RestartLevelButton.transform.DOScale(0.9f, 0.1f).OnComplete(() =>
            {
                RestartLevelButton.transform.DOScale(1f, 0.1f).OnComplete(() =>
                {
                    StartCoroutine(Delay());
                });
            });

            IEnumerator Delay()
            {
                FindFirstObjectByType<PanelsManager>().ChooseType();
                yield return new WaitForSeconds(1f);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        });

        OpenMenuButton.onClick.AddListener(() =>
        {
            OpenMenuButton.transform.DOScale(0.9f, 0.1f).OnComplete(() =>
            {
                OpenMenuButton.transform.DOScale(1f, 0.1f).OnComplete(() =>
                {
                    StartCoroutine(Delay());
                });
            });

            IEnumerator Delay()
            {
                FindFirstObjectByType<PanelsManager>().ChooseType();
                yield return new WaitForSeconds(1f);
                SceneManager.LoadScene("Menu");
            }
        });
    }

    private void GamePause()
    {
        OnGamePaused?.Invoke();
        PauseGameButton.interactable = false;
        PauseGameButton.transform.DOScale(Vector3.one * _scaleMultiplier, _time).OnComplete(() =>
        {
            PauseGameButton.transform.DOScale(Vector3.one, _time).OnComplete(() =>
            {
                Vector3 startPosition = new Vector3(-812f, 349.4f);
                Vector3 finalPosition = new Vector3(231, 0, 0);
                float duration = 0.3f;
                PausePanel.anchoredPosition = startPosition;
                PausePanel.localScale = Vector3.zero;
                PausePanel.gameObject.SetActive(true);

                Sequence seq = DOTween.Sequence();
                seq.Append(PausePanel.DOAnchorPos(finalPosition, duration)).Join(PausePanel.DOScale(Vector3.one, duration));
            });
        });
    }

    private void GameResume()
    {
        ResumeGameButton.transform.DOScale(Vector3.one * _scaleMultiplier, _time).OnComplete(() =>
        {
            ResumeGameButton.transform.DOScale(Vector3.one, _time).OnComplete(() =>
            {
                Vector3 startPosition = new Vector3(-812f, 349.4f);
                float duration = 0.3f;
                Sequence seq = DOTween.Sequence();
                seq.Append(PausePanel.DOAnchorPos(startPosition, duration)).Join(PausePanel.DOScale(Vector3.zero, duration)).
                OnComplete(() =>
                {
                    PausePanel.gameObject.SetActive(false);
                    PauseGameButton.interactable = true;
                    OnGameResumed?.Invoke();
                });
            });
        });
    }
}
