using System.Collections;
using UnityEngine;
using UnityEngine.Video;

namespace UI.Scripts
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private VideoPlayer startVideo;
        [SerializeField] private VideoPlayer winFirstVideo;
        [SerializeField] private VideoPlayer winSecondVideo;
        [SerializeField] private GameObject startCanvas;
        [SerializeField] private GameObject winCanvas;
        [SerializeField] private GameObject winChildCanvas;
        [SerializeField] private GameObject loseCanvas;
        [SerializeField] private GameObject playCanvas;
        [SerializeField] private GameObject player;
        [SerializeField] private GameObject[] lifeKit;
        [SerializeField] private GameObject lightHouse;
        [SerializeField] private GameObject guards;
        [SerializeField] private PlayerToWorld playerToWorld;
        [SerializeField] private AudioSource startMusic;
        [SerializeField] private AudioSource playMusic;
        [SerializeField] private AudioSource gameOverMusic;
        [SerializeField] private AudioSource winMusic;
        [SerializeField] private GameObject halfDeadAnimator;
        [SerializeField] private GameObject halfDeadScreen;
        [SerializeField] private float bloodDissolveSpeed = 0.5f;

        private Vector3 _playerPosition;
        private Vector3 _cameraPosition;
        private Vector3 _lightHousePosition;
        private Vector3 _lifeKitPosition;
        private Quaternion _lightHouseRotation;
        private float [] _guardsRotation;
        private SpriteRenderer _bloodScreen;

        private void Start()
        {
            playMusic.volume = 0.6f;
            _cameraPosition = Camera.main.transform.position;
            halfDeadScreen.SetActive(false);
            _playerPosition = player.transform.position;
            _lightHousePosition = lightHouse.transform.position;
            _lightHouseRotation = lightHouse.transform.rotation;
            _bloodScreen = halfDeadScreen.GetComponent<SpriteRenderer>();

            player.gameObject.SetActive(false);
            lightHouse.gameObject.SetActive(false);
            guards.gameObject.SetActive(false);
            startCanvas.gameObject.SetActive(true);
            startVideo.Play();
            startMusic.Play();
        }

        private void Update()
        {
            if (playerToWorld.GetLife() == 0)
            {
                LoseScene();
            }
            if (halfDeadAnimator.activeSelf)
            {
                Color newColor = _bloodScreen.color;
                newColor = Color.Lerp(_bloodScreen.color,
                    new Color(_bloodScreen.color.r, _bloodScreen.color.g, _bloodScreen.color.b, 0),
                    bloodDissolveSpeed * Time.deltaTime);
                _bloodScreen.color = newColor;
            }
            else
            {
                Color newColor = _bloodScreen.color;
                newColor.a = 1f;
                _bloodScreen.color = newColor;
            }
        }

        public void ShootAnimation()
        {
            halfDeadScreen.SetActive(true);
        }

        public void LoseScene()
        {
            player.SetActive(false);
            lightHouse.SetActive(false);
            guards.SetActive(false);
            ResetObjects();
            playCanvas.gameObject.SetActive(false);
            playMusic.Stop();

            loseCanvas.gameObject.SetActive(true);
            gameOverMusic.Play();
        }
        public void WinScene()
        {
            player.SetActive(false);
            lightHouse.SetActive(false);
            guards.SetActive(false);
            ResetObjects();
            playCanvas.gameObject.SetActive(false);
            winCanvas.gameObject.SetActive(true);
            winChildCanvas.gameObject.SetActive(false);
            winFirstVideo.gameObject.SetActive(true);
            playMusic.Stop();
            winFirstVideo.Play();
            StartCoroutine(PlaySecondWinVideo(20));
        }

        private IEnumerator PlaySecondWinVideo(float delay = 20f)
        {
            yield return new WaitForSeconds(delay);
            winFirstVideo.Stop();
            winFirstVideo.gameObject.SetActive(false);
            winChildCanvas.gameObject.SetActive(true);
            gameOverMusic.Play();
            winSecondVideo.Play();
        }

        public void StartClickPlay()
        {
            ActivateObjects();
            playerToWorld.shouldDelayGameOver = false;
            startCanvas.gameObject.SetActive(false);
            loseCanvas.gameObject.SetActive(false);
            winSecondVideo.gameObject.SetActive(false);
            winFirstVideo.Stop();
            winSecondVideo.Stop();
            winCanvas.gameObject.SetActive(false);
            halfDeadScreen.SetActive(false);
            gameOverMusic.Stop();
            playCanvas.gameObject.SetActive(true);
            player.gameObject.SetActive(true);
            lightHouse.gameObject.SetActive(true);
            guards.gameObject.SetActive(true);
            ResetBloodScreenOpacity();
            startMusic.Stop();
            startVideo.Stop();
            playMusic.Play();
        }

        private void ResetBloodScreenOpacity()
        {
            Color newColor = _bloodScreen.color;
            newColor.a = 255f;
            _bloodScreen.color = newColor;
        }

        public void ClickQuit()
        {
            Application.Quit();
        }

        public void ResetObjects()
        {
            // reset key, lifekit, ui, player, lightHouse, etc.
            PlayerToWorld.Shared.isDrowning = false;
            PlayerToWorld.Shared.isGotShot = false;
            playerToWorld.ResetLife();
            player.transform.position = _playerPosition;
            Camera.main.transform.position = _cameraPosition;
            lightHouse.transform.position = _lightHousePosition;
            lightHouse.transform.rotation = _lightHouseRotation;
        }

        private void ActivateObjects()
        {
            player.SetActive(true);
            lightHouse.SetActive(true);
            guards.SetActive(true);
            for (int i = 0; i < lifeKit.Length; i++)
            {
                lifeKit[i].SetActive(true);
            }
        }
    }
}