using System.Collections;
using UnityEngine;
using MzoUI.PlayScene;
using PlayerManager;

namespace Runner
{
    public class Collision : MonoBehaviour
    {
        [System.Serializable]
        public class RayCheck
        {
            public bool isActive = true;
            public float radius;
            public float length;
            public Vector3 offset;
            public Vector3 direction;
            public LayerMask layer;
            public RaycastHit hit;
            public Ray ray;
            public RayCheck(Vector3 offset, Vector3 direction)
            {
                this.length = 1.5f;
                this.offset = offset;
                this.direction = direction;
            }
            public bool HasDetected(Vector3 positionCollider)
            {
                if (!isActive) return false;

                ray = new Ray(positionCollider + offset, direction); 
                if(Physics.SphereCast(ray, radius, out hit, length, layer))
                {
                    return true;
                }
                return false;
            }
        }

        [Header("General")]
        public AudioClip impactSound;
        public ImpactBar impactBar;
        public CameraController cameraCtrl;
        public int impactLevel = 0; // 0: nothing; 1: continue..; 2: die
        public float recoveryTime = 3f;
        public bool isCheckObstacles = true;
        public bool isFallingInfinity = false;
        [Header("Rays check")]
        [SerializeField] RayCheck rayUpperForward = new RayCheck(Vector3.up * 1f, Vector3.forward);
        [SerializeField] RayCheck rayBelowForward = new RayCheck(Vector3.up * .5f, Vector3.forward);
        [SerializeField] RayCheck rayLeftSide = new RayCheck(Vector3.up * 1f, Vector3.left);
        [SerializeField] RayCheck rayRightSide = new RayCheck(Vector3.up * 1f, Vector3.right);
        private void Awake()
        {
            if (cameraCtrl == null) cameraCtrl = FindObjectOfType<CameraController>();
            if (impactBar == null) impactBar = FindObjectOfType<ImpactBar>();
        }
        private void Start()
        {
            impactLevel = 0;
            RunnerManager.instance.revivalEvent.AddListener(() =>
            {
                StartCoroutine(Revival());
            });
        }
        private void Update()
        {
            CheckForward();
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            DrawLine(rayRightSide);
            DrawLine(rayLeftSide);
            DrawLine(rayBelowForward);
            DrawLine(rayUpperForward);
        }
        
        public Interaction.Interaction GetInteractionForward()
        {
            // check was detected any thing forward
            if (rayBelowForward.HasDetected(transform.position))
            {
                // make sure hit is a interaction
                Interaction.Interaction interaction = rayBelowForward.hit.transform.GetComponent<Interaction.Interaction>();
                if (interaction)
                {
                    return interaction;
                }
                else return null;
            }
            else if (rayUpperForward.HasDetected(transform.position))
            {
                // make sure hit is a interaction
                Interaction.Interaction interaction = rayUpperForward.hit.transform.GetComponent<Interaction.Interaction>();
                if (interaction)
                {
                    return interaction;
                }
                else return null;
            }
            else return null;
        }
        public bool CheckForward()
        {
            if (!isCheckObstacles) return false;
            // check forward
            if (rayUpperForward.HasDetected(transform.position) && rayUpperForward.hit.transform.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
            {
                impactLevel = 2;
                Impact();
                // die
                return true;
            }
            else if (rayBelowForward.HasDetected(transform.position) && rayBelowForward.hit.transform.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
            {
                impactLevel++;
                Impact();
                // continue
                return true;
            }
            //.....
            return true;
        }
        public bool CheckLeftSide()
        {
            if (!isCheckObstacles) return false;

            if(rayLeftSide.HasDetected(transform.position))
            {
                impactLevel++;
                Impact();
                return true;
            }
            return false;
        }
        public bool CheckRightSide()
        {
            if (!isCheckObstacles) return false;

            if (rayRightSide.HasDetected(transform.position))
            {
                impactLevel++;
                Impact();
                return true;
            }
            return false;
        }
        public void OnSliding()
        {
            rayUpperForward.isActive = false;
        }
        public void OnDesliding()
        {
            rayUpperForward.isActive = true;
        }
        private void Impact()
        {
            if (RunnerManager.instance.IsGameOver() == true) return;
            // effects
            cameraCtrl.Shake();
            if(impactSound) Audio.instance.source.PlayOneShot(impactSound);
            //
            if (impactLevel >= 2)
            {
                // die
                if (RunnerManager.instance.isGameOver == false) RunnerManager.instance.gameOverEvent.Invoke();
            }
            else if(impactLevel == 1)
            {
                Debug.Log("impacted");
                // little falling

                // Recuperating..
                StartCoroutine(Recuperate());
                impactBar.Active(recoveryTime);
            }

        }
        IEnumerator Recuperate()
        {
            float remainTime = recoveryTime;
            while (remainTime > 0)
            {
                remainTime -= Time.deltaTime;
                impactBar.slider.value = remainTime;

                yield return null;
            }
            impactBar.Deactive();
            impactLevel = 0;
        }
        private void DrawLine(RayCheck ray)
        {
            if (!ray.isActive) return;

            Debug.DrawLine(transform.position + ray.offset, transform.position + ray.offset + ray.direction * ray.length, Color.yellow);
            Gizmos.DrawWireSphere(transform.position + ray.offset + ray.direction * ray.length, ray.radius);
        }
        IEnumerator Revival()
        {
            isCheckObstacles = false;
            yield return new WaitForSeconds(3f);
            isCheckObstacles = true;
        }
    }
}
