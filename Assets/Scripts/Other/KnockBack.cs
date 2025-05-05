using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class KnockBack : MonoBehaviour
{
    [SerializeField] private float _knockBackForce = 3f;
    [SerializeField] private float _knockBackMovingTimerMax = 0.3f;
    public bool isGettingKnockBack { get; private set; }
    private float _knockBackMovingTimer;

    private Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void GetKnockBack(Transform damageSource)
    {
        
        _knockBackMovingTimer = _knockBackMovingTimerMax;
        Vector2 difference = (transform.position - damageSource.position).normalized * _knockBackForce / rb.mass;
        rb.AddForce(difference, ForceMode2D.Impulse);
    }
    
    private void Update()
    {
        isGettingKnockBack = true;
        _knockBackMovingTimer -= Time.deltaTime;
        if (_knockBackMovingTimer < 0)
            StopKnockMovement();
        
    }

    public void StopKnockMovement()
    {
        rb.linearVelocity = Vector2.zero;
        isGettingKnockBack = false;
    }
}
