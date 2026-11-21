using UnityEngine;

public class Platform : MonoBehaviour
{
    public float MoveSpeed = 30;

    private Rigidbody2D _rigidbody;
    private FixedJoystick _joystick;
    private bool _control = true;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _joystick = FindFirstObjectByType<FixedJoystick>();

        GameManager.OnBlocksCountEnded += StopPlatform;
        GameManager.OnAttemptsEnded += StopPlatform;
        PauseManager.OnGamePaused += PauseControlPlatform;
        PauseManager.OnGameResumed += ControlPlatform;
    }

    private void FixedUpdate()
    {
        if (_control)
        {
            float horizontal = _joystick.Horizontal;
            Vector3 newPosition = _rigidbody.position + Vector2.right * horizontal * MoveSpeed * Time.fixedDeltaTime;
            newPosition.x = Mathf.Clamp(newPosition.x, -14.5f, 28f);
            _rigidbody.MovePosition(newPosition);
        }
    }

    private void OnDestroy()
    {
        GameManager.OnBlocksCountEnded -= StopPlatform;
        GameManager.OnAttemptsEnded -= StopPlatform;
        PauseManager.OnGamePaused -= PauseControlPlatform;
        PauseManager.OnGameResumed -= ControlPlatform;
    }

    private void StopPlatform()
    {
         MoveSpeed = 0;
    }

    private void ControlPlatform()
    {
        _control = true;
    }

    private void PauseControlPlatform()
    {
        _control = false;
    }
}

