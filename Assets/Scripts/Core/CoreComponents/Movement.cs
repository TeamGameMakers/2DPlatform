using System;
using UnityEngine;

namespace Core
{
    public class Movement : CoreComponent
    {
        internal Rigidbody2D rb;

        private Vector2 _currentVelocity;
        public Vector2 CurrentVelocity => _currentVelocity;
        public int FaceDirection { get; private set; }

        protected void Awake()
        {
            rb = GetComponentInParent<Rigidbody2D>();
            FaceDirection = 1;
        }
        
        // 为了顺序可控，不使用MonoBehavior Update事件
        internal void LogicUpdate()
        {
            _currentVelocity = rb.velocity;
        }
        
        public void Flip(float inputX)
        {
            if (transform.right.x * inputX >= 0) return;
            FaceDirection = -FaceDirection;
            rb.transform.Rotate(0.0f, 180.0f, 0.0f);
        }

        #region Value Setter
    
        public void SetVelocity(Vector2 velocity)
        {
            rb.velocity = velocity;
            _currentVelocity = velocity;
        }

        public void SetVelocityX(float velocityX)
        {
            vec2Setter.Set(velocityX, _currentVelocity.y);
            rb.velocity = vec2Setter;
            _currentVelocity = vec2Setter;
        }

        public void SetVelocityY(float velocityY)
        {
            vec2Setter.Set(_currentVelocity.x, velocityY);
            rb.velocity = vec2Setter;
            _currentVelocity = vec2Setter;
        }

        public void SetFriction(float friction)
        {
            var material = rb.sharedMaterial;
            material.friction = friction;
            rb.sharedMaterial = material;
        }
    
        #endregion
    }
}
