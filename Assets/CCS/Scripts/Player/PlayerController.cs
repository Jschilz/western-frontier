using UnityEngine;
using CCS.Managers;

namespace CCS.Player
{
    /// <summary>
    /// Handles player movement and jumping using CharacterController and InputManager events.
    /// Movement is relative to the main camera's facing direction.
    /// Logs the current render pipeline on start.
    /// 
    /// Required Components: CharacterController
    /// Place on: Player GameObject
    /// Date: 2024-06-09
    /// Author: James Schilz
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        #region Inspector
        [Header("Movement Settings")]
        [Tooltip("Movement speed in units per second.")]
        [SerializeField] private float moveSpeed = 5f;
        [Tooltip("Jump force (height in units).")]
        [SerializeField] private float jumpHeight = 2f;
        [Tooltip("Gravity force (negative value).")]
        [SerializeField] private float gravity = -9.81f;
        [Header("Camera Rotation Settings")]
        [Tooltip("Transform the camera will follow and rotate (should be a child of the player at head height).")]
        [SerializeField] private Transform cameraFollowTarget;
        [Tooltip("Mouse/gamepad look sensitivity.")]
        [SerializeField] private float rotationPower = 0.2f;
        [Tooltip("Minimum vertical angle (pitch) in degrees.")]
        [SerializeField] private float minPitch = -40f;
        [Tooltip("Maximum vertical angle (pitch) in degrees.")]
        [SerializeField] private float maxPitch = 40f;
        #endregion

        #region Private Fields
        private CharacterController characterController;
        private Vector3 velocity;
        private bool isGrounded;
        private Vector2 moveInput;
        private Vector2 lookInput;
        private float pitch = 0f;
        #endregion

        #region Unity Methods
        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
            if (InputManager.Instance == null)
            {
                Debug.LogError("[PlayerController] Critical: InputManager instance not found. PlayerController will not function.");
                enabled = false;
                return;
            }
            InputManager.Instance.OnMove += OnMove;
            InputManager.Instance.OnJump += OnJump;
            InputManager.Instance.OnLook += OnLook;
            CheckRenderPipeline();
        }

        private void OnDestroy()
        {
            if (InputManager.Instance != null)
            {
                InputManager.Instance.OnMove -= OnMove;
                InputManager.Instance.OnJump -= OnJump;
                InputManager.Instance.OnLook -= OnLook;
            }
        }

        private void Update()
        {
            isGrounded = characterController.isGrounded;
            if (isGrounded && velocity.y < 0)
                velocity.y = -2f;

            // Camera-relative movement
            Vector3 move = Vector3.zero;
            if (moveInput.sqrMagnitude > 0.01f)
            {
                Transform cam = Camera.main ? Camera.main.transform : null;
                if (cam)
                {
                    Vector3 forward = cam.forward;
                    Vector3 right = cam.right;
                    forward.y = 0; right.y = 0;
                    forward.Normalize(); right.Normalize();
                    move = forward * moveInput.y + right * moveInput.x;
                }
                else
                {
                    move = new Vector3(moveInput.x, 0, moveInput.y);
                }
            }
            characterController.Move(move * moveSpeed * Time.deltaTime);

            // Gravity
            velocity.y += gravity * Time.deltaTime;
            characterController.Move(velocity * Time.deltaTime);

            // --- Camera & Player Rotation ---
            if (cameraFollowTarget != null && lookInput.sqrMagnitude > 0.0001f)
            {
                // Horizontal (Yaw) rotation
                transform.rotation *= Quaternion.AngleAxis(lookInput.x * rotationPower, Vector3.up);

                // Vertical (Pitch) rotation
                pitch -= lookInput.y * rotationPower;
                pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
                cameraFollowTarget.localEulerAngles = new Vector3(pitch, 0, 0);
            }
            else if (cameraFollowTarget == null)
            {
                Debug.LogWarning("[PlayerController] NonCritical: CameraFollowTarget is not assigned. Camera rotation will not function.");
            }

            // Reset look input so rotation only happens when new input is received
            lookInput = Vector2.zero;
        }
        #endregion

        #region Input Handlers
        private void OnMove(Vector2 input)
        {
            moveInput = input;
        }

        private void OnJump()
        {
            if (isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }

        private void OnLook(Vector2 look)
        {
            lookInput = look;
        }
        #endregion

        #region Utilities
        private void CheckRenderPipeline()
        {
            var rp = UnityEngine.Rendering.GraphicsSettings.currentRenderPipeline;
            // No logs for pipeline, as this is not critical for runtime
        }
        #endregion
    }
} 