using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TurnOnOffGaurds : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float radiusTurnComps = 30;
    private Light2D _light2D;

    private void Start()
    {
        _light2D = GetComponent<Light2D>();
    }

    private void Update()
    {
        
        if(Mathf.Abs(Vector3.Distance(transform.position, player.transform.position)) < radiusTurnComps)
        {
            _light2D.enabled = true;
        }
        else
        {
            _light2D.enabled = false;
        }
    }
}
