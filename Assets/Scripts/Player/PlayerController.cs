using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    public event Action OnEPressed = delegate {  };
    public event Action OnEscPressed = delegate {  };
    
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private Vector2 _movement;
    private bool _canMoveInput = true;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            OnEPressed();
        }
        else if(Input.GetKeyDown(KeyCode.Escape))
        {
            OnEscPressed();
        }
        
        if(!_canMoveInput) return;
        
        _movement.x = Input.GetAxisRaw("Horizontal");
        _movement.y = Input.GetAxisRaw("Vertical");
        
        _animator.SetFloat("Horizontal", _movement.x);
        _animator.SetFloat("Vertical", _movement.y);
        _animator.SetFloat("Speed", _movement.sqrMagnitude);
    }

    private void FixedUpdate()
    {
        _rigidbody.MovePosition(_rigidbody.position + _movement * (moveSpeed * Time.fixedDeltaTime));
    }

    #region Methods

    public void ToggleMoveInputs(bool value)
    {
        _canMoveInput = value;
    }

    #endregion
}