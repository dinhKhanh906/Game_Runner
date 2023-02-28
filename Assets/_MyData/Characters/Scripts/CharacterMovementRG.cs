using UnityEngine;
using System.Collections;

namespace Runner
{

    [RequireComponent(typeof(Rigidbody))]
    public class CharacterMovementRG : MonoBehaviour
    {
        public enum State { jumping, sliding, running }
        public State currentState;
        private Rigidbody body;
        private Animator animator;

        [Header("Ground check")]
        private RaycastHit hitGround;
        private Ray rayCheckGround;
        public float rayLength = 1.1f;
        public bool isGrounded;
        public LayerMask groundMask;

        [Header("Jump")]
        public float jumpForce = 300f;

        [Header("Slide")]
        [SerializeField] bool isSliding = false;
        public float slideDuration = 0.5f;
        public float slideCoolDown = 0.3f;
        public float percentHeightSliding = 0.5f;
        public float forceDown = 100f;

        [Header("to Left-Right")]
        [SerializeField] int currentLane;   // 0 is left; 1 is middle; 2 is right
        [SerializeField] int countChangeLane = 0;
        public float laneOffset = 2f;
        public float changeLaneDuration = .3f;
        public float rotationY = 45f;
        public float speedRerotate = 1f;
        public float targetPosX;
        private void Start()
        {
            body = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
            currentLane = 1; // character will start in mid lane
        }
        private void Update()
        {
            CheckGrounded();
            if (Input.GetKeyDown(KeyCode.LeftArrow)) ChangeLane(-1);
            else if (Input.GetKeyDown(KeyCode.RightArrow)) ChangeLane(1);

            if (Input.GetKeyDown(KeyCode.UpArrow)) Jump();
            if (Input.GetKeyDown(KeyCode.DownArrow)) Slide();

            Rerotate();
        }
        private void OnDrawGizmos()
        {
            Debug.DrawRay(transform.position + Vector3.up, Vector3.down * rayLength, Color.yellow);
        }
        private void Jump()
        {
            if (isGrounded)
            {
                body.AddForce(Vector3.up * jumpForce);
                animator.SetTrigger("jump");
                currentState = State.jumping;
            }
        }
        private void Slide()
        {
            if (isSliding) return;

            currentState = State.sliding;
            body.AddForce(Vector3.down * forceDown);
            StartCoroutine(SlideSmooth());
        }
        IEnumerator SlideSmooth()
        {
            float timer = 0f;
            isSliding = true;
            CapsuleCollider collider = GetComponent<CapsuleCollider>();
            collider.height *= percentHeightSliding;
            collider.center = new Vector3(collider.center.x, collider.height * 0.5f, collider.center.z);
            animator.SetTrigger("slide");
            while (timer <= slideDuration)
            {
                timer += Time.deltaTime;
                yield return null;
            }
            collider.height /= percentHeightSliding;
            collider.center = new Vector3(collider.center.x, collider.height * 0.5f, collider.center.z);

            timer = 0f;
            while (timer <= slideCoolDown)
            {
                timer += Time.deltaTime;
                yield return null;
            }
            isSliding = false;
        }
        private void ChangeLane(int direction)
        {
            if ((currentLane == 0 && direction < 0) || (currentLane == 2 && direction > 0)) return;

            currentLane += direction;
            targetPosX += direction * laneOffset;
            countChangeLane++;
            if (countChangeLane == 1) StartCoroutine(ChangeLaneSmooth());
        }
        private void Rerotate()
        {
            if (transform.rotation.eulerAngles != Vector3.zero)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(Vector3.zero), speedRerotate * Time.deltaTime);
            }
        }
        IEnumerator ChangeLaneSmooth()
        {
            float timer = 0f;
            float posXRequire = targetPosX;
            Quaternion rotRequire = posXRequire > transform.position.x ? Quaternion.Euler(Vector3.up * rotationY) : Quaternion.Euler(Vector3.up * rotationY * -1);
            while (timer <= changeLaneDuration)
            {
                timer += Time.deltaTime;
                Vector3 target = new Vector3(posXRequire, transform.position.y, transform.position.z);
                transform.position = Vector3.Lerp(transform.position, target, timer / changeLaneDuration);
                transform.rotation = Quaternion.Lerp(rotRequire, transform.rotation, timer / changeLaneDuration);
                yield return null;
            }

            countChangeLane--;
            if (countChangeLane == 1)
            {
                yield return null;
                StartCoroutine(ChangeLaneSmooth());
            }
        }
        public bool CheckGrounded()
        {
            rayCheckGround = new Ray(transform.position + Vector3.up, Vector3.down);
            if (Physics.Raycast(rayCheckGround, out hitGround, rayLength, groundMask))
            {
                isGrounded = true;
                animator.SetBool("isFalling", false);
                currentState = State.running;
            }
            else
            {
                isGrounded = false;
                animator.SetBool("isFalling", true);
            }

            return isGrounded;
        }
    }

}