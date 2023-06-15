using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TurnOnOffShadows : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float radiusTurnComps = 20f;
    private PolygonCollider2D _polygonCollider2D;
    private ShadowCaster2D _shadowCaster2D;

    private void Start()
    {
        _polygonCollider2D = GetComponent<PolygonCollider2D>();
        _shadowCaster2D = GetComponent<ShadowCaster2D>();
    }

    private void Update()
    {
        
        if(Mathf.Abs(Vector3.Distance(transform.position, player.transform.position)) < radiusTurnComps)
        {
            _polygonCollider2D.enabled = true;
            _shadowCaster2D.enabled = true;
        }
        else
        {
            _polygonCollider2D.enabled = false;
            _shadowCaster2D.enabled = false;
        }
    }
}
