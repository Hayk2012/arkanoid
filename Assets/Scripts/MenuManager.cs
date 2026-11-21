using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public Button ResetProgressButton;
    public Button PlayButton;
    public Button OpenLevelsPanelButton;
    public Button CloseLevelsPanelButton;
    public Button QuitGameButton;
    public TMP_Text TitleText;
    public TMP_Text LoadingPercentText;
    public GameObject LoadingPanel;
    public RectTransform LevelsPanel;

    private void Awake()
    {
        PlayButton.onClick.AddListener(PlayGame);
        OpenLevelsPanelButton.onClick.AddListener(OpenLevelsPanel);
        CloseLevelsPanelButton.onClick.AddListener(CloseLevelsPanel);
        QuitGameButton.onClick.AddListener(QuitGame);

        ResetProgressButton.onClick.AddListener(() => PlayerPrefs.DeleteAll());
    }

    private void QuitGame()
    {
        QuitGameButton.transform.DOScale(0.9f, 0.1f).OnComplete(() =>
        {
            QuitGameButton.transform.DOScale(1f, 0.1f).OnComplete(() =>
            {
                Application.Quit();
            });
        });
    }

    private void PlayGame()
    {
        PlayButton.transform.DOScale(0.9f, 0.1f).OnComplete(() =>
        {
            PlayButton.transform.DOScale(1f, 0.1f).OnComplete(() =>
            {
                int levelToLoad = ProgressService.UnlockedLevel;
                int maxLevelIndex = SceneManager.sceneCountInBuildSettings - 1;
                if (levelToLoad > maxLevelIndex)
                {
                    levelToLoad = maxLevelIndex;
                }

                StartCoroutine(Load(levelToLoad));
            });
        });



        IEnumerator Load(int level)
        {
            LoadingPanel.SetActive(true);

            float progress = 0;

            while (progress < 100)
            {
                if (progress < 100)
                {
                    progress = Mathf.Lerp(progress, 100, 1f * Time.deltaTime);
                }

                if (progress >= 99.5f)  
                {
                    yield return new WaitForSeconds(1);
                    SceneManager.LoadScene(level);
                }

                LoadingPercentText.text = $"{progress:F0}%";

                yield return null;
            }
        }
    }

    private void OpenLevelsPanel()
    {
        OpenLevelsPanelButton.transform.DOScale(0.9f, 0.1f).OnComplete(() =>
        {
            OpenLevelsPanelButton.transform.DOScale(1f, 0.1f).OnComplete(() =>
            {
                LevelsPanel.localScale = Vector3.zero;
                LevelsPanel.gameObject.SetActive(true);
                LevelsPanel.DOScale(Vector3.one, 1f);
            });
        });
    }

    private void CloseLevelsPanel()
    {
        CloseLevelsPanelButton.transform.DOScale(0.9f, 0.1f).OnComplete(() =>
        {
            CloseLevelsPanelButton.transform.DOScale(1f, 0.1f).OnComplete(() =>
            {
                LevelsPanel.DOScale(Vector3.zero, 1f).OnComplete(() =>
                {
                    LevelsPanel.gameObject.SetActive(false);
                });
                
            });
        });
    }

}
