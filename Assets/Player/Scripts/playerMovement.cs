using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerMovement : MonoBehaviour
{
    public static playerMovement Shared { get; private set; }
    [SerializeField] AnimationCurve curveY;
    [SerializeField] private InputAction playerControls;
    [SerializeField] private float moveSpeed = 3f;
    private Rigidbody2D _rigidbody;
    private Vector2 _moveDirection;
    private Vector2 _isometricVector;
    private Vector2 _currentPos;
    private Vector2 _landingPos;
    private float _landingDis;
    private float _isometricX, _isometricY, _timeElapsed;
    private bool _onGround = true;
    private bool _jump = false;
    private bool _isAlive = true;
    private bool _crouching = false;

    private void Awake()
    {
        Shared = this;
    }
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Update()
    {
        if(_isAlive && !PlayerToWorld.Shared.isGotShot)
        {
            IsometricMovement();
            Input();
        }
        
    }

    private void Input()
    {
        if (UnityEngine.Input.GetKeyDown("space"))
            _jump = true;
        else if (UnityEngine.Input.GetKeyDown("x"))
            _crouching = true;
        else if(UnityEngine.Input.GetKeyUp("x"))
            _crouching = false;
    }

    private void FixedUpdate()
    {
        if(_jump)
        {
            Jump();
        }
        else
        {
            if(_isAlive && !PlayerToWorld.Shared.isGotShot)
                _rigidbody.MovePosition(_rigidbody.position +
                                        _isometricVector * (moveSpeed * Time.fixedDeltaTime));
        }
    }

    private void IsometricMovement()
    {
        _moveDirection = playerControls.ReadValue<Vector2>();

        if (_moveDirection.x > 0.5f && _moveDirection.x < 0.9f || -_moveDirection.x > 0.5f && -_moveDirection.x < 0.9f)
        {
            _moveDirection.x = 0;
            _moveDirection.y = Mathf.Round(_moveDirection.y);
        }   
        _isometricX =  _moveDirection.y -_moveDirection.x;
        _isometricY = (_moveDirection.x + _moveDirection.y) / 2;
        if (_moveDirection.x != 0 && _moveDirection.y == 0)
        {
            _isometricX = -_isometricX;
            _isometricY = -_isometricY;
        }
        _isometricVector = new Vector2(_isometricX, _isometricY).normalized;
    }
    
    public Vector2 GetMoveDirection()
    {
        return _moveDirection;
    }
    
    public bool WhileJumping()
    {
        return !_onGround;
    }
    
    public bool IsCrouching()
    {
        return _crouching;
    }
    
    public void KillPlayer()
    {
        _isAlive = false;
    }
    
    public void ResetLevel()
    {
        _isAlive = true;
    }
    
    public bool IsAlive()
    {
        return _isAlive;
    }
    
    private void Jump()
    {
        if(_onGround)
        {
            _currentPos = _rigidbody.position;
            _landingPos = _currentPos + _isometricVector.normalized * moveSpeed;
            _landingDis = Vector2.Distance(_landingPos, _currentPos);
            if(_landingDis == 0)
                _landingDis = 2f;
            _timeElapsed = 0f;
            _onGround = false;
        }
        else
        {
            _timeElapsed += Time.fixedDeltaTime * moveSpeed / _landingDis;
            if(_timeElapsed <= 0.8f)
            {
                _currentPos = Vector2.MoveTowards(_currentPos, _landingPos, Time.fixedDeltaTime * moveSpeed);
                _rigidbody.MovePosition(new Vector2(_currentPos.x, _currentPos.y + curveY.Evaluate(_timeElapsed)));
            }
            else
            {
                _jump = false;
                _onGround = true;
            }
        }
    }
}