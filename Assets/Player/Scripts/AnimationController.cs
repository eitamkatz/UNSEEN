using System;
using System.Collections;
using UnityEngine;

namespace Player.Scripts
{
    public class AnimationController : MonoBehaviour
    {
        private Animator _animator;
        private Vector2 _moveDirection;

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            _moveDirection = playerMovement.Shared.GetMoveDirection();
            if (_moveDirection != Vector2.zero)
            {
                _animator.SetFloat("moveX", _moveDirection.x);
                _animator.SetFloat("moveY", _moveDirection.y);
            }

            UpdateAnimation();
        }

        private void UpdateAnimation()
        {
            if (PlayerToWorld.Shared.isDrowning)
            {
                _animator.Play("Drowning");
            }
            else if (PlayerToWorld.Shared.isGotShot)
                _animator.Play("Dying");
            else if (PlayerToWorld.Shared.GetShot())
            {
                StartCoroutine(DelayAnimation(1f));
            }
            else if (playerMovement.Shared.WhileJumping())
                _animator.Play("JumpPlayer");
            else if (_moveDirection != Vector2.zero)
            {
                if (PlayerToWorld.Shared.GetLife() == 1)
                    StartCoroutine(DelayHitAnimation(0.7f, "HitWalking"));

                else
                    _animator.Play("MovementPlayer");
            }

            else
            {
                if (playerMovement.Shared.IsCrouching())
                    _animator.Play("CrouchDown");
                else
                {
                    if (PlayerToWorld.Shared.GetLife() == 1)
                        StartCoroutine(DelayHitAnimation(0.7f, "IdleHit"));
                    else
                        _animator.Play("IdlePlayer");
                }
            }
        }

        private IEnumerator DelayAnimation(float delay)
        {
            _animator.Play("GetShot");
            yield return new WaitForSeconds(delay);
            PlayerToWorld.Shared.SetShot(false);
        }

        private IEnumerator DelayHitAnimation(float delay, String animationName)
        {
            _animator.Play(animationName);
            yield return new WaitForSeconds(delay);
        }
    }
}