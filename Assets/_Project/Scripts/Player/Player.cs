using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class Player : SingletonMonobehaviour<Player>
{
    // Input
    private GameInput _gameInput;
    private InputAction _moveAction;
    private InputAction _walkAction;

    // Movement Parameters
    private float _xInput;
    private float _yInput;
    private bool _isCarrying;
    private bool _isIdle;
    private bool _isLiftingToolDown;
    private bool _isLiftingToolLeft;
    private bool _isLiftingToolRight;
    private bool _isLiftingToolUp;
    private bool _isRunning;
    private bool _isUsingToolDown;
    private bool _isUsingToolLeft;
    private bool _isUsingToolRight;
    private bool _isUsingToolUp;
    private bool _isSwingingToolDown;
    private bool _isSwingingToolLeft;
    private bool _isSwingingToolRight;
    private bool _isSwingingToolUp;
    private bool _isWalking;
    private bool _isPickingUp;
    private bool _isPickingDown;
    private bool _isPickingLeft;
    private bool _isPickingRight;
    private ToolEffect _toolEffect = ToolEffect.None;

    private Rigidbody2D _rb;
    private Direction _playerDirection;
    private float _moveSpeed;
    
    public bool PlayerInputIsDisabled { get; set; }

    protected override void Awake()
    {
        base.Awake();

        _rb = GetComponent<Rigidbody2D>();
        _gameInput = new GameInput();
        _gameInput.Enable();
        _moveAction = _gameInput.Player.Move;
        _walkAction = _gameInput.Player.Walk;
    }

    private void OnEnable()
    {
        _walkAction.performed += PlayerWalkInputPerformed;
        _walkAction.canceled += PlayerWalkInputCancelled;
    }

    private void OnDisable()
    {
        _walkAction.performed -= PlayerWalkInputPerformed;
        _walkAction.canceled -= PlayerWalkInputCancelled;
    }

    private void Update()
    {
        #region Player Input
        ResetAnimationTriggers();

        PlayerMovementInput();

        #endregion
    }

    private void FixedUpdate()
    {
        PlayerMovement();
    }

    private void ResetAnimationTriggers()
    {
        _isPickingRight = false;
        _isPickingLeft = false;
        _isPickingUp = false;
        _isPickingDown = false;
        _isUsingToolRight = false;
        _isUsingToolLeft = false;
        _isUsingToolUp = false;
        _isUsingToolDown = false;
        _isLiftingToolRight = false;
        _isLiftingToolLeft = false;
        _isLiftingToolUp = false;
        _isLiftingToolDown = false;
        _isSwingingToolRight = false;
        _isSwingingToolLeft = false;
        _isSwingingToolUp = false;
        _isSwingingToolDown = false;
        _toolEffect = ToolEffect.None;
    }

    private void PlayerMovementInput()
    {
        Vector2 moveInput = _moveAction.ReadValue<Vector2>();
        _xInput = moveInput.x;
        _yInput = moveInput.y;

        if (_isWalking)
        {
            _isRunning = false;
            _isIdle = false;
            _moveSpeed = Settings.WALKING_SPEED;
        }
        else if (_xInput != 0 || _yInput != 0)
        {
            _isRunning = true;
            _isWalking = false;
            _isIdle = false;
            _moveSpeed = Settings.RUNNING_SPEED;
        }
        else if (_xInput == 0 && _yInput == 0)
        {
            _isRunning = false;
            _isWalking = false;
            _isIdle = true;
        }

        // For saving direction in memory
        if (_xInput < 0)
            _playerDirection = Direction.Left;
        else if (_xInput > 0)
            _playerDirection = Direction.Right;
        else if (_yInput < 0)
            _playerDirection = Direction.Down;
        else
            _playerDirection = Direction.Up;

        EventHandler.CallMovementEvent(
            _xInput, _yInput, _isWalking, _isRunning, _isIdle, _isCarrying, _toolEffect,
            _isUsingToolRight, _isUsingToolLeft, _isUsingToolUp, _isUsingToolDown,
            _isLiftingToolRight, _isLiftingToolLeft, _isLiftingToolUp, _isLiftingToolDown,
            _isPickingRight, _isPickingLeft, _isPickingUp, _isPickingDown,
            _isSwingingToolRight, _isSwingingToolLeft, _isSwingingToolUp, _isSwingingToolDown,
            false, false, false, false);
    }

    private void PlayerWalkInputPerformed(InputAction.CallbackContext context)
    {
        _isWalking = true;
    }

    private void PlayerWalkInputCancelled(InputAction.CallbackContext context)
    {
        _isRunning = true;
        _isWalking = false;
        _isIdle = false;
        _moveSpeed = Settings.RUNNING_SPEED;
    }

    private void PlayerMovement()
    {
        Vector2 moveVector = new(_xInput * _moveSpeed * Time.deltaTime, _yInput * _moveSpeed * Time.deltaTime);
        _rb.MovePosition(_rb.position + moveVector);
    }
}