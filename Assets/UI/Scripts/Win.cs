using UnityEngine;

namespace UI.Scripts
{
    public class Win : MonoBehaviour
    {

        [SerializeField] private GameManager gameManager;
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                gameManager.WinScene();
            }
        }

    }
}
