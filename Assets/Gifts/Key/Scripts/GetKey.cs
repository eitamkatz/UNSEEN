using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetKey : MonoBehaviour
{
    [SerializeField] private Sprite fullKeySprite;
    [SerializeField] private Sprite emptyKeySprite;
    [SerializeField] private Image keyImage;
    private static bool _hasKey = false;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            _hasKey = true;
            Destroy(gameObject);
        }
    }

    public static bool HasKey()
    {
        return _hasKey;
    }

    private void Update()
    {
        keyImage.sprite = _hasKey ? fullKeySprite : emptyKeySprite;
    }
}