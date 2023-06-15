using UnityEngine;

namespace Enemies.LightHouse.Scripts
{
    public class RaysLightHouse : MonoBehaviour
    {
        [SerializeField] private float rayLength = 500f;
        [SerializeField] private float rayRadius = 0.3f;
        [SerializeField] private LayerMask rayLayer = default;
        private const float RADIUS_RAY = 0.2f;

        private void FixedUpdate()
        {
            IsGrounded = IsTouchingGround(rayRadius) || IsTouchingGround(-rayRadius);
        }

        private bool IsTouchingGround(float radiusX)
        {
            Vector3 origin = transform.position;
            origin.x += radiusX;

            RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, rayLength, rayLayer);

            Debug.DrawRay(origin, Vector3.down * rayLength, Color.magenta);

            return hit.collider != null;
        }

        public bool IsGrounded { get; private set; }

        private void OnDrawGizmos()
        {
            Vector3 pos = transform.position;
            pos.y -= rayLength;
            Gizmos.color = IsGrounded ? Color.cyan : Color.yellow;
            Gizmos.DrawSphere(transform.position, RADIUS_RAY);
        }
    }
}