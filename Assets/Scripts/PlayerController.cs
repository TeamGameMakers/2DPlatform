using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameInput))]
public class PlayerController : MonoBehaviour
{
    private GameInput _input;
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    
    [Header("Movement")]
    public float speed = 3.0f;

    private void Awake()
    {
        _input = GetComponent<GameInput>();
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        if (_input.moveInput == 0.0f)
        {
            _rb.velocity = new Vector2(0.0f, _rb.velocity.y);
            return;
        }
        
        // 改变人物朝向
        _spriteRenderer.flipX = _input.moveInput < 0 ? true : false;

        _rb.velocity = new Vector2(_input.moveInput * speed, _rb.velocity.y);
    }
}
