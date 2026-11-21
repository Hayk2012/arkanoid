using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Block : MonoBehaviour
{
    public Sprite[] BlockSprites;

    [Range(0.1f, 10), SerializeField] private float _reduceSpeed;

    private SpriteRenderer _spriteRenderer;
    private Vector3 targetScale = Vector3.one * 0.2f;
    private Collider2D _collider;
    private int _blockHP = 3;
    private Rigidbody2D _rigidbody;
    private LevelManager _levelManager;
    private bool _control = true;

    private void Awake()
    {
        _levelManager = FindFirstObjectByType<LevelManager>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();

        PauseManager.OnGamePaused += PauseControlBlock;
        PauseManager.OnGameResumed += ResumeControlBlock;
    }

    private void OnDestroy()
    {
        PauseManager.OnGamePaused -= PauseControlBlock;
        PauseManager.OnGameResumed -= ResumeControlBlock;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Ball>())
        {
            int levelIndex = SceneManager.GetActiveScene().buildIndex;
            int behaviourIndex = LevelCombinationStorage.Combinations[levelIndex].BehaviourIndex;
            switch (behaviourIndex)
            {
                case 0:
                    Behaviour1();
                    break;
                case 1:
                    Behaviour2();
                    break;
                case 2:
                    Behaviour3();
                    break;
                case 3:
                    Behaviour4();
                    break;

                default:
                    break;
            }
        }
    }

    public void Behaviour1()
    {
        _spriteRenderer.color = new Color(Random.value, Random.value, Random.value);
        DestroyBlockOnTime();
    }

    public void Behaviour2()
    {
        StartCoroutine(Reduce());


        IEnumerator Reduce()
        {
            Vector3 currentScale = transform.localScale;

            _collider.isTrigger = true;
            while (Vector3.Distance(currentScale, targetScale) > 0.1f)
            {
                yield return new WaitWhile(() => !_control);
                currentScale = Vector3.Lerp(currentScale, targetScale, _reduceSpeed * Time.deltaTime);
                transform.localScale = currentScale;
                yield return null;
            }
            DestroyBlockOnTime();
        }
    }

    public void Behaviour3()
    {
        Vector2 midSize = transform.localScale / 1.5f;
        Vector2 minSize = transform.localScale / 2.5f;
        _blockHP--;

        if (_blockHP == 2)
        {
            StartCoroutine(Reduce(midSize));
        }
        else if (_blockHP == 1)
        {
            StartCoroutine(Reduce(minSize));
        }
        else
        {
            DestroyBlockOnTime();
        }

        IEnumerator Reduce(Vector2 targetScale)
        {
            Vector3 currentScale = transform.localScale;

            _collider.isTrigger = true;
            while (Vector3.Distance(currentScale, targetScale) > 0.1f)
            {
                yield return new WaitWhile(() => !_control);
                currentScale = Vector3.Lerp(currentScale, targetScale, _reduceSpeed * Time.deltaTime);
                transform.localScale = currentScale;
                yield return null;
            }
            transform.localScale = currentScale;
            _collider.isTrigger = false;
        }
    }

    public void Behaviour4()
    {
        _blockHP--;
        _spriteRenderer.sprite = BlockSprites[_blockHP];
        if (_blockHP == 0)
        {
            DestroyBlockOnPosition();
        }
    }

    private void DestroyBlockOnPosition()
    {
        StartCoroutine(FallDown());
        IEnumerator FallDown()
        {
            while (transform.position.y > -15)
            {
                _rigidbody.bodyType = RigidbodyType2D.Dynamic;
                _collider.isTrigger = true;
                yield return null;
            }
            Destroy(gameObject);
        }
    }

    private void DestroyBlockOnTime()
    {
        StartCoroutine(DestroyDelay());
        IEnumerator DestroyDelay()
        {
            float timer = 2;

            while (timer > 0)
            {                
                yield return null;

                if (!_control) continue;

                timer -= Time.deltaTime;
            }
            Destroy(gameObject);

        }
    }

    private void PauseControlBlock()
    {
        _control = false;
    }

    private void ResumeControlBlock()
    {
        _control = true;
    }
}
