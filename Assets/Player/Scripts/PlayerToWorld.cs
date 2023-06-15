using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerToWorld : MonoBehaviour
{
    public static PlayerToWorld Shared { get; private set; }
    public bool shouldDelayGameOver = false;
    public bool isDrowning = false;
    public bool isGotShot = false;
    [SerializeField] private ParticleSystem particleSystem;
    private int _life = 2;
    private int _totalLives = 2;
    private bool _isGrounded = true;
    private bool _gotShot = false;

    private void Awake()
    {
        Shared = this;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ground"))
            _isGrounded = true;
        if (other.CompareTag("FirstAidKit") && _life < _totalLives)
        {
            other.gameObject.SetActive(false);
            TakeDamage(-1);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("ground"))
            _isGrounded = true;
        if (other.CompareTag("water") && !_isGrounded &&
            !playerMovement.Shared.WhileJumping())
        {
            isDrowning = true;
           
            StartCoroutine(DelayGameOver());
            playerMovement.Shared.KillPlayer();
            shouldDelayGameOver = true;
        }
    }
    private IEnumerator DelayGameOver()
    {
        yield return new WaitForSeconds(3f);
        _life = 0;
        isDrowning = false;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("ground"))
            _isGrounded = false;
    }

    public int GetLife()
    {
        return _life;
    }
    
    public bool GetShot()
    {
        return _gotShot;
    }

    public void SetShot(bool state)
    {
        _gotShot = state;
    }

    public void TakeDamage(int damage)
    {
        if (damage == 1 && _life == 1)
        {
            isGotShot = true;
            StartCoroutine(DelayGameOver());
        }
        else
            _life -= damage;
        if(_life > _totalLives) 
            _life = _totalLives;
        if (_life == 1)
        {
            particleSystem.Play();
        }
        else
            particleSystem.Stop();    
    }
    
    public void ResetLife()
    {
        _life = _totalLives;
        _isGrounded = true;
        playerMovement.Shared.ResetLevel();
    }
    
    public int GetTotalLives()
    {
        return _totalLives;
    }

  
}
