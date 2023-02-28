using UnityEngine;
using Cinemachine;
using System.Collections;

namespace Runner
{
    public class CameraController : MonoBehaviour
    {
        public CinemachineBrain brainCam;
        public CinemachineVirtualCamera startCam;
        public CinemachineVirtualCamera runningCam;
        public CinemachineVirtualCamera dieCam;

        [Header("Shake effect")]
        [SerializeField] float shakeDuration = 0.2f;
        [SerializeField] float intensity = 1.0f;
        private void OnDrawGizmos()
        {
            if (!startCam || !runningCam || !dieCam)
            {
                Debug.LogWarning("Virtual camera is null");
            }
        }
        private void Awake()
        {
            if(brainCam==null) brainCam = GetComponent<CinemachineBrain>();
        }
        private void Start()
        {
            startCam.gameObject.SetActive(true);
            runningCam.gameObject.SetActive(false);
            dieCam.gameObject.SetActive(false);
            // start game (temporary)
            StartCoroutine(OnStartRun());
            // game over
            Runner.Collision collision = FindObjectOfType<Collision>();
            RunnerManager.instance.gameOverEvent.AddListener(() =>
            {
                if(collision.isFallingInfinity == false)
                {
                    startCam.gameObject.SetActive(false);
                    runningCam.gameObject.SetActive(false);
                    dieCam.gameObject.SetActive(true);
                }
            });
            RunnerManager.instance.revivalEvent.AddListener(() =>
            {
                startCam.gameObject.SetActive(false);
                runningCam.gameObject.SetActive(true);
                dieCam.gameObject.SetActive(false);
            });

            runningCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0f;

        }
        public IEnumerator OnStartRun()
        {
            yield return new WaitForSeconds(1f);

            startCam.gameObject.SetActive(false);
            runningCam.gameObject.SetActive(true);
            dieCam.gameObject.SetActive(false);
            StartCoroutine(WaitCamSuccessful());
        }
        IEnumerator WaitCamSuccessful()
        {
            CharacterInput inputReader = FindObjectOfType<CharacterInput>();
            inputReader.hasListen = false;
            while (brainCam.IsBlending)
            {
                yield return null;
            }
            inputReader.hasListen = true;
        }
        public void Shake()
        {
            if (runningCam) StartCoroutine(ShakeEff());
        }
        IEnumerator ShakeEff()
        {
            CinemachineBasicMultiChannelPerlin shaker = runningCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            shaker.m_AmplitudeGain = intensity;

            yield return new WaitForSeconds(shakeDuration);

            shaker.m_AmplitudeGain = 0;
        }
    }
}
