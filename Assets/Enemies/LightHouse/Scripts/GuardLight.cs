using System.Collections;
using UI.Scripts;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Enemies.LightHouse.Scripts
{
    public class GuardLight : MonoBehaviour
    {
        [SerializeField] private float rotationSpeed = 5f;
        [SerializeField] private float maxAngleDeflection = 50.0f;

        private float _angle;
        private float _time;
        private float _delayLight = 1f;
        private float _initRotation;


        private void Start()
        {
            _initRotation = transform.rotation.z;
        }


        public void RotateGuardsLight()
        {
            _angle = maxAngleDeflection * Mathf.Sin(_time * rotationSpeed * _delayLight);
            transform.rotation = Quaternion.Euler(0, 0, _angle) * Quaternion.Euler(0, 0, _initRotation * 180);
            _time += Time.deltaTime;
        }
    }
}