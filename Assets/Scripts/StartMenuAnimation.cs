using UnityEngine;

public class StartMenuAnimation : MonoBehaviour
{
    public RectTransform CanvasRect;
    public Vector2 Velocity;

    [SerializeField] private RectTransform _ballRect;

    private void Awake()
    {
        //_ballRect = GetComponent<RectTransform>();
        _ballRect.pivot = new Vector2(0.5f, 0.5f);
    }

    private void Update()
    {
        _ballRect.anchoredPosition += Velocity * Time.deltaTime;

        float halfCanvasWidth = CanvasRect.rect.width * 0.5f;
        float halfCanvasHeight = CanvasRect.rect.height * 0.5f;

        float halfBallWidth = _ballRect.rect.width * 0.5f;
        float halfBallHeight = _ballRect.rect.height * 0.5f;

        float minX = -halfCanvasWidth + halfBallWidth;
        float maxX = halfCanvasWidth - halfBallWidth;
        float minY = -halfCanvasHeight + halfBallHeight;
        float maxY = halfCanvasHeight - halfBallHeight;

        Vector2 position = _ballRect.anchoredPosition;

        if (position.x < minX)
        {
            position.x = minX;
            Velocity.x = -Velocity.x;
        }
        else if (position.x > maxX)
        {
            position.x = maxX;
            Velocity.x = -Velocity.x;
        }

        if (position.y < minY)
        {
            position.y = minY;
            Velocity.y = -Velocity.y;
        }
        else if (position.y > maxY)
        {
            position.y = maxY;
            Velocity.y = -Velocity.y;
        }

        _ballRect.anchoredPosition = position;
    }
}
