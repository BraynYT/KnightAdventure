using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Player : MonoBehaviour {

    public static Player Instance { get; private set; }
    public event EventHandler OnPlayerDeath;
    
    
    [SerializeField] private float movingSpeed = 10f;
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private float damageRecoveryTime = 0.5f;
    
    Vector2 inputVector;

    private Rigidbody2D rb;
    private KnockBack _knockBack;

    private float minMovingSpeed = 0.1f;
    private bool isRunning = false;
    private int _currentHealth;
    private bool _canTakeDamage;
    private bool _isAlive;
    
    private void Awake() {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        _knockBack = GetComponent<KnockBack>();
    }

    private void Start() {
        _currentHealth = maxHealth;
        _canTakeDamage = true;
        GameInput.Instance.OnPlayerAttack += GameInput_OnPlayerAttack;
        _isAlive = true;
    }

    private void GameInput_OnPlayerAttack(object sender, System.EventArgs e) {
        ActiveWeapon.Instance.GetActiveWeapon().Attack();
    }

    private void Update() {
        inputVector = GameInput.Instance.GetMovementVector();
    }


    private void FixedUpdate()
    {
        Debug.Log(_knockBack.isGettingKnockBack);
        if (_knockBack.isGettingKnockBack)
            return;
        
        HandleMovement();
    }


    private void HandleMovement() {
        rb.MovePosition(rb.position + inputVector * (movingSpeed * Time.fixedDeltaTime));
        if (Mathf.Abs(inputVector.x) > minMovingSpeed || Mathf.Abs(inputVector.y) > minMovingSpeed) {
            isRunning = true;
        } else {
            isRunning = false;
        }
    }

    public bool IsRunning() {
        return isRunning;
    }

    public Vector3 GetPlayerScreenPosition() {
        Vector3 playerScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
        return playerScreenPosition;
    }

    public void TakeDamage(Transform damageSource, int damage)
    {
        if (_canTakeDamage && _isAlive)
        {
            _canTakeDamage = false;
            _currentHealth = Math.Max(0, _currentHealth -= damage);
            _knockBack.GetKnockBack(damageSource);

            StartCoroutine(DamageRecoveryRoutine());
        }
        
        Death();
    }

    private void Death()
    {
        if (_currentHealth == 0 && _isAlive)
        {
            _knockBack.StopKnockMovement();
            _isAlive = false;
            GameInput.Instance.DisableMovement();
            
            OnPlayerDeath.Invoke(this, EventArgs.Empty);

        }
    }

    private IEnumerator DamageRecoveryRoutine()
    {
        yield return new WaitForSeconds(damageRecoveryTime);
        _canTakeDamage = true;
    }
    
    public bool IsAlive() { return _isAlive; }
}
