using UI.Scripts;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Enemies.LightHouse.Scripts
{
    public class LightHouseRotation : MonoBehaviour
    {
        [SerializeField] private float rotationSpeed = 5f;
        [SerializeField] private float resetAngle = 20f;
        [SerializeField] private float moveSpeed, radius;

        private float _angle;
        private Vector3 _initPos;
        private float _initRadius;
        private bool _takeDamage = false;
        private RaycastHit2D[] _hitInfo;
        private float _timeSinceLastDamage;
        private Collider2D _rightHit;
        private Collider2D _playerCollider;
        private Ray2D[] _ray2Ds;
        private playerMovement _playerMovement;

        private void Start()
        {
            _initPos = transform.position;
        }

        public void RotateLightHouse()
        {
            if (transform.eulerAngles.z > resetAngle && transform.eulerAngles.z < resetAngle * 2)
            {
                transform.Rotate(new Vector3(0, 0, resetAngle - 45f));
            }

            if (radius == 0)
            {
                transform.Rotate(Vector3.forward * (rotationSpeed * Time.deltaTime));
                _angle += moveSpeed / Time.deltaTime;
            }
            else
            {
                transform.Rotate(Vector3.forward * (rotationSpeed * Time.deltaTime));
                _angle += (moveSpeed / (radius * Mathf.PI * 2.0f)) * Time.deltaTime;
                transform.position = _initPos + new Vector3(Mathf.Cos(_angle), Mathf.Sin(_angle), 0);
            }
        }
    }
}