using System;
using UnityEngine;

namespace Core
{
    public class Movement : CoreComponent
    {
        internal Rigidbody2D rb;
        
        private Vector2 _vec2Setter;

        private Vector2 _currentVelocity;
        public Vector2 CurrentVelocity => _currentVelocity;

        protected override void Awake()
        {
            base.Awake();

            rb = GetComponentInParent<Rigidbody2D>();
        }

        private void Update()
        {
            _currentVelocity = rb.velocity;
        }

        #region Value Setter
    
        public void SetVelocity(Vector2 velocity)
        {
            rb.velocity = velocity;
            _currentVelocity = velocity;
        }

        public void SetVelocityX(float velocityX)
        {
            _vec2Setter.Set(velocityX, _currentVelocity.y);
            rb.velocity = _vec2Setter;
            _currentVelocity = _vec2Setter;
        }

        public void SetVelocityY(float velocityY)
        {
            _vec2Setter.Set(_currentVelocity.x, velocityY);
            rb.velocity = _vec2Setter;
            _currentVelocity = _vec2Setter;
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
