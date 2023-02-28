using UnityEngine;
using System.Collections;
using PlayerManager;

namespace Runner
{
    public class Movement : MonoBehaviour
    {
        #region general
        public enum State { jumping, sliding, running }
        public State currentState;
        public float centerToBottom = 0f; // distance between Character's center and Character's bottom
        public float gravity = -10f;
        public Collision runnerCollision;
        public Animator animator;
        public bool canMove = true;
        [SerializeField] float gravitySaver;
        #endregion

        #region ground check
        [Header("Ground check")]
        public float rayGroundLength = 1.1f;
        public bool isGrounded;
        public LayerMask groundMask;
        public LayerMask infinityMask;
        private RaycastHit hitGround;
        private Ray rayCheckGround;
        #endregion

        #region jump setting
        [Header("Jump")]
        public AudioClip soundJump;
        public float jumpForce = 8f;
        [SerializeField] Vector3 velocityInAir = Vector3.zero;
        #endregion

        #region slide setting
        [Header("Slide")]
        [SerializeField] bool isSliding = false;
        public float slideDuration = 0.5f;
        public float slideCoolDown = 0.3f;
        public float downForce = 30f;
        #endregion

        #region change lane to left-right setting
        [Header("to Left-Right")]
        [SerializeField] int currentLane;   // 0 is left; 1 is middle; 2 is right
        [SerializeField] int countChangeLane = 0;
        public float laneOffset = 2f;
        public float changeLaneDuration = .3f;
        public float rotationY = 45f;
        public float speedRerotate = 1f;
        public float targetPosX = 0f;
        #endregion
        private void Start()
        {
            if(animator == null)animator = GetComponentInChildren<Animator>();
            if(runnerCollision == null) runnerCollision = GetComponent<Collision>();    
            currentLane = 1; // character will start in mid lane
            targetPosX = 0f;
            canMove = true;

            RunnerManager.instance.gameOverEvent.AddListener(() =>
            {
                gravitySaver = gravity;
                if (!runnerCollision.isFallingInfinity) animator.SetTrigger("die");
                else
                {
                    gravity = 0f;
                    velocityInAir = Vector3.zero;
                }
                animator.SetBool("gameOver", true);
                canMove = false;
            });
            RunnerManager.instance.revivalEvent.AddListener(() =>
            {
                // reset position
                targetPosX = 0;
                currentLane = 1;
                gravity = gravitySaver;
                transform.position = Vector3.up * 5;
                animator.SetBool("gameOver", false);
                canMove = true;
            });
        }
        private void Update()
        {
            //move transform with velocity
            transform.position += velocityInAir * Time.deltaTime;

            CheckGrounded();
            SetPositionOnGround();
            CheckInfinityFalling();
            if (!canMove) return;

            Rerotate();
        }
        private void OnDrawGizmosSelected()
        {
            Debug.DrawRay(transform.position + Vector3.up * centerToBottom + Vector3.up, Vector3.down * rayGroundLength, Color.red);
        }
        #region Jump
        public void OnJump()
        {
            if (canMove)
            {
                Jump();
                if(soundJump) Audio.instance.source.PlayOneShot(soundJump);
            }
        }
        private void Jump()
        {
            if (isGrounded)
            {
                // jump
                velocityInAir.y = jumpForce;
                // play animation "jump"
                if (animator) animator.SetTrigger("jump");
                currentState = State.jumping;
            }
        }
        #endregion
        #region slide state
        public void OnSlide()
        {
            if (canMove) Slide();
        }
        private void Slide()
        {
            if (isSliding) return;

            currentState = State.sliding;
            // play animation
            if (animator) animator.SetTrigger("slide");
            // 
            velocityInAir.y = downForce;
            // smooth
            StartCoroutine(SlideState());
        }
        IEnumerator SlideState()
        {
            // slide... and set hit box when sliding
            isSliding = true;

            if (runnerCollision)
            {
                runnerCollision.OnSliding();
            }
            yield return new WaitForSeconds(slideDuration);

            if (runnerCollision)
            {
                runnerCollision.OnDesliding();

            }
            //set cooldown
            yield return new WaitForSeconds(slideCoolDown);
            isSliding = false;
        }
        #endregion

        #region change lane
        public void OnChangeLane(int direction)
        {
            if (!canMove) return;
            ChangeLane(direction);
            if(direction > 0) // to right
            {
                runnerCollision.CheckRightSide();
                return;
            }

            if(direction < 0) // to left
            {
                runnerCollision.CheckLeftSide();
                return;
            }
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
        #endregion

        #region check ground
        public bool CheckGrounded()
        {
            // grounded check
            rayCheckGround = new Ray(transform.position + Vector3.up * centerToBottom + Vector3.up, Vector3.down);
            if (Physics.Raycast(rayCheckGround, out hitGround, rayGroundLength, groundMask))
            {
                isGrounded = true;
                // reset velocity
                if (velocityInAir.y < 0) velocityInAir = new Vector3(0, -0.5f, 0);

                // play animation
                if (animator) animator.SetBool("isFalling", false);
                currentState = State.running;
            }
            else
            {
                isGrounded = false;
                // set velocity in air with gravity
                velocityInAir.y += gravity * Time.deltaTime;

                // play animation
                if (animator) animator.SetBool("isFalling", true);
            }
            return isGrounded;
        }
        private void CheckInfinityFalling()
        {

            // check is falling infinity
            if (Physics.Raycast(rayCheckGround, out hitGround, rayGroundLength, infinityMask))
            {
                runnerCollision.isFallingInfinity = true;
                if(RunnerManager.instance.isGameOver == false) RunnerManager.instance.gameOverEvent.Invoke();
            }
        }
        private void SetPositionOnGround()
        {
            // 
            if (isGrounded)
            {
                transform.position = hitGround.point + Vector3.up * centerToBottom;
            }
        }
        #endregion
    }

}
