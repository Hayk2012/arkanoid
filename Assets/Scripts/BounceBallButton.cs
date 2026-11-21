using DG.Tweening;
using TMPro;
using UnityEngine;

public class BounceBallButton : MonoBehaviour
{
    public TMP_Text ValueText;

    private int _valueToShow = 1;
    private string _minusSign = "-";
    private string _plusSign = "+";
    private RectTransform _textTransform;

    private void Awake()
    {
        _textTransform = ValueText.GetComponent<RectTransform>();
    }

    public void ShowAnimation()
    {
        ValueText.text = "";
        ValueText.text = (PauseManager.IsLeft ? _minusSign : _plusSign) + _valueToShow;
        if (PauseManager.IsLeft)
        {
            DownSlide();
        }
        else
        {
            UpSlide();
        }

    }

    private void UpSlide()
    {
        _textTransform.DOAnchorPosY(75f, 0.1f).OnComplete(() =>
        {
            ValueText.text = "";
            _textTransform.anchoredPosition = new Vector3(_textTransform.anchoredPosition.x, 0);
            ValueText.transform.DOKill();
        });
    }

    private void DownSlide()
    {
        _textTransform.DOAnchorPosY(-75f, 0.1f).OnComplete(() =>
        {
            ValueText.text = "";
            _textTransform.anchoredPosition = new Vector3(_textTransform.anchoredPosition.x, 0);
            ValueText.transform.DOKill();
        });
    }
}


