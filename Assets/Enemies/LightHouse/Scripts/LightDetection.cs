using UI.Scripts;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Enemies.LightHouse.Scripts
{
    public class LightDetection : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private PlayerToWorld playerToWorld;
        [SerializeField] private float radius;
        [SerializeField] private GameObject player;
        [SerializeField] private LayerMask rayLayer = default;
        [SerializeField] private int raysAmount;
        [SerializeField] private AudioSource shotSound;

        private float _angle;
        private Vector3 _initPos;
        private float _initRadius;
        private bool _takeDamage = false;
        private RaycastHit2D[] _hitInfo;
        private float _timeSinceLastDamage;
        private Collider2D _rightHit;
        private Collider2D _playerCollider;
        private Ray2D[] _ray2Ds;
        [SerializeField] private Light2D _light2D;
        private playerMovement _playerMovement;
        private LightHouseRotation _lightHouseScript;
        private GuardLight _guardScript;
        private float _rayLength;
        private float _rayLengthGizmo;

        private void Awake()
        {
            if (_light2D == null)
            {
                _light2D = GetComponentInChildren<Light2D>();
            }

            CreateRays();
        }

        private void CreateRays()
        {
            _ray2Ds = new Ray2D[raysAmount];
            for (int i = 0; i < raysAmount; i++)
            {
                _ray2Ds[i] = new Ray2D();
            }
        }

        private void Start()
        {
            _initPos = transform.position;
            _hitInfo = new RaycastHit2D[raysAmount];
            _timeSinceLastDamage = Time.time;
            _initRadius = radius;
            _playerMovement = player.GetComponent<playerMovement>();
            _lightHouseScript = GetComponent<LightHouseRotation>();
            _guardScript = GetComponent<GuardLight>();
        }

        private void RotateObject()
        {
            if (gameObject.CompareTag("lighthouse"))
            {
                _lightHouseScript.RotateLightHouse();
                _rayLength = Mathf.Infinity;
                _rayLengthGizmo = 150;
            }
            else
            {
                _guardScript.RotateGuardsLight();
                _rayLength = _light2D.pointLightInnerRadius;
                _rayLengthGizmo = _rayLength;
            }
        }


        private void Update()
        {
            SetRays();
            RotateObject();
            for (int i = 0; i < raysAmount; i++)
            {
                _hitInfo[i] = Physics2D.Raycast(_ray2Ds[i].origin, _ray2Ds[i].direction, _rayLength, rayLayer);
            }

            _takeDamage = false;
            for (int i = 0; i < _hitInfo.Length; i++)
            {
                if (_hitInfo[i])
                {
                    if (_hitInfo[i].collider.CompareTag("Player"))
                    {
                        _takeDamage = true;
                    }
                }
            }

            if (_takeDamage && Time.time - _timeSinceLastDamage > 2f)
            {
                if (playerToWorld.GetLife() != 0 && !playerToWorld.isDrowning && _playerMovement.IsAlive())
                {
                    shotSound.Play();
                    playerToWorld.SetShot(true);
                }

                if (playerToWorld.GetLife() == playerToWorld.GetTotalLives())
                {
                    gameManager.ShootAnimation();
                }

                playerToWorld.TakeDamage(1);
                _timeSinceLastDamage = Time.time;
            }
        }

        private void SetRays()
        {
            float angle = _light2D.pointLightInnerAngle;
            float startAngle = -angle / 2;
            Vector2 startDirection = Quaternion.Euler(0, 0, startAngle) * (-transform.up);
            float angleBetweenRays = angle / (raysAmount - 1);
            for (int i = 0; i < raysAmount; i++)
            {
                _ray2Ds[i].origin = transform.position;
                _ray2Ds[i].direction = Quaternion.Euler(0, 0, angleBetweenRays * i) * startDirection;
                ;
            }
        }

        private void OnDrawGizmos()
        {
            CreateRays();
            SetRays();

            Gizmos.color = Color.magenta;
            for (int i = 0; i < raysAmount; i++)
            {
                Gizmos.DrawRay(_ray2Ds[i].origin, _ray2Ds[i].direction * _rayLengthGizmo);
            }

            Gizmos.DrawSphere(transform.position, 0.1f);
        }
    }
}