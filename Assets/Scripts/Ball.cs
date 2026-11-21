using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour, IPointerDownHandler
{
    public float BounceForce;
    public Platform Platform;
    public static event Action OnBallFalledDown;

    private Rigidbody2D _rigidbody;
    private Collider2D _collider;
    private SpriteRenderer _renderer;
    private PauseManager _pauseManager;
    private Vector3 _reflectedDirection;
    private Vector3 _startBallPosition;
    private Vector3 _startPlatformPosition;
    private Vector3 _savedVelocity;
    private bool _ballOnPlatform = true;
    private float _minVelocityLimit = 0.5f;

    private GameObject[] _horizontalBorders;
    private GameObject[] _verticalBorders;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _renderer = GetComponent<SpriteRenderer>();
        _pauseManager = FindFirstObjectByType<PauseManager>();

        _startBallPosition = transform.position;
        _startPlatformPosition = Platform.transform.position;

        GameManager.OnBlocksCountEnded += StopBall;
        GameManager.OnAttemptsEnded += StopBall;

        PauseManager.OnGamePaused += PauseControlBall;
        PauseManager.OnGameResumed += ControlBall;

        _horizontalBorders = GameObject.FindGameObjectsWithTag("Horizontal");
        _verticalBorders = GameObject.FindGameObjectsWithTag("Vertical");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CollisionHandler(collision);
        AdjustBallDirection();
        ChangeHorizontalBorders(collision);
        ChangeVerticalBorders(collision);
    }

    private void ChangeHorizontalBorders(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Horizontal"))
        {
            Color newColor = new Color(Random.value, Random.value, Random.value);

            foreach (var border in _horizontalBorders)
            {
                var borderRenderer = border.GetComponent<Image>();
                borderRenderer.color = newColor;
            }
        }
    }

    private void ChangeVerticalBorders(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Vertical"))
        {
            Color newColor = new Color(Random.value, Random.value, Random.value);

            foreach (var border in _verticalBorders)
            {
                var borderRenderer = border.GetComponent<Image>();
                borderRenderer.color = newColor;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DownBorder"))
        {
            StartCoroutine(ReturnPlatformAndBallToStartPosition());
        }
    }

    private void Update()
    {
        InputHandler();
    }

    private void InputHandler()
    {
        if (_ballOnPlatform)
        {
            transform.position = new Vector3(Platform.transform.position.x, transform.position.y);
        }
    }

    private IEnumerator ReturnPlatformAndBallToStartPosition()
    {
        OnBallFalledDown?.Invoke();
        _rigidbody.linearVelocity = Vector2.zero;
        _rigidbody.angularVelocity = 0f;
        _renderer.enabled = false;
        _collider.isTrigger = true;
        transform.position = _startBallPosition;
        Platform.transform.position = _startPlatformPosition;

        yield return new WaitForSeconds(0.001f);

        _collider.isTrigger = false;
        _renderer.enabled = true;
        _ballOnPlatform = true;
    }

    private void CollisionHandler(Collision2D collision)
    {
        Vector3 collisionPoint = collision.contacts[0].normal;
        Vector3 newDirection = Vector3.Reflect(_reflectedDirection, collisionPoint).normalized;
        _rigidbody.linearVelocity = newDirection * BounceForce;
    }

    private void FixedUpdate()
    {
        if (Mathf.Abs(_rigidbody.linearVelocity.magnitude) < BounceForce - _minVelocityLimit && !_ballOnPlatform)
            _rigidbody.linearVelocity = _rigidbody.linearVelocity.normalized * BounceForce;

        _reflectedDirection = _rigidbody.linearVelocity.normalized * BounceForce;
    }

    private void AdjustBallDirection()
    {

        float minYvelocity = 1f;
        Vector2 yBallVelocity = _rigidbody.linearVelocity;
        if (Mathf.Abs(_rigidbody.linearVelocityY) < minYvelocity)
        {
            yBallVelocity.y = yBallVelocity.y > 0 ? minYvelocity : -minYvelocity;
            _rigidbody.linearVelocity = yBallVelocity.normalized * BounceForce;
        }

        float minXvelocity = 1f;
        Vector2 xBallVelocity = _rigidbody.linearVelocity;
        if (Mathf.Abs(_rigidbody.linearVelocityX) < minXvelocity)
        {
            xBallVelocity.x = xBallVelocity.x > 0 ? minXvelocity : -minXvelocity;
            _rigidbody.linearVelocity = xBallVelocity.normalized * BounceForce;
        }

    }
    private void StopBall()
    {
        BounceForce = 0;
        _rigidbody.linearVelocity = Vector2.zero;
        _rigidbody.angularVelocity = 0f;
    }

    private void OnDestroy()
    {
        GameManager.OnBlocksCountEnded -= StopBall;
        GameManager.OnAttemptsEnded -= StopBall;

        PauseManager.OnGameResumed -= ControlBall;
        PauseManager.OnGamePaused -= PauseControlBall;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_ballOnPlatform && !_pauseManager.PausePanel.gameObject.activeSelf)
        {
            //_rigidbody.linearVelocity = (Vector2.up + new Vector2(Random.Range(-1, 2), 0)).normalized * BounceForce;
            _rigidbody.linearVelocity = Vector2.up * BounceForce;

            _ballOnPlatform = false;
        }
    }

    private void ControlBall()
    {
        _rigidbody.constraints = RigidbodyConstraints2D.None;
        _rigidbody.linearVelocity = _savedVelocity;
    }

    private void PauseControlBall()
    {
        _savedVelocity = _rigidbody.linearVelocity;
        _rigidbody.linearVelocity = Vector2.zero;
        _rigidbody.angularVelocity = 0f;
        _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
    }
}
