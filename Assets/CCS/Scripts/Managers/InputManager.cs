using UnityEngine;
using CCS.Utilities;
using System;
using UnityEngine.InputSystem;

namespace CCS.Managers
{
    /// <summary>
    /// Singleton InputManager for handling all player input and raising events for gameplay systems.
    /// Decouples input reading from gameplay logic for modularity and scalability.
    ///
    /// Author: James Schilz
    /// Date Created: 4/27/2025
    /// Property of: Crazy Carrot Studios
    /// </summary>
    public class InputManager : MonoSingleton<InputManager>
    {
        #region Inspector Variables
        [Header("Input Settings")]
        [Tooltip("Reference to the Game Input Actions Asset.")]
        [SerializeField] private InputActionAsset gameInputActions;
        #endregion

        #region Events
        public event Action<Vector2> OnMove;
        public event Action<Vector2> OnLook;
        public event Action<float> OnZoom;
        public event Action OnJump;
        public event Action OnSprint;
        public event Action OnInteract;
        public event Action OnFire;
        public event Action OnAim;
        public event Action OnReload;
        public event Action OnCrouch;
        public event Action OnInventory;
        public event Action OnHolsterWeapon;
        #endregion

        #region Private Fields
        private InputActionMap playerMap;
        #endregion

        #region Unity Methods
        protected override void Awake()
        {
            base.Awake();
            Initialization();
        }

        private void OnEnable()
        {
            EnableInput();
        }

        private void OnDisable()
        {
            DisableInput();
        }
        #endregion

        #region Initialization
        public override void Initialization()
        {
            if (gameInputActions == null)
            {
                Debug.LogError("[InputManager] Critical: Game Input Actions Asset not assigned in InputManager.");
                enabled = false;
                return;
            }
            playerMap = gameInputActions.FindActionMap("Player");
            if (playerMap == null)
            {
                Debug.LogError("[InputManager] Critical: Player action map not found in Game Input Actions Asset.");
                enabled = false;
                return;
            }
        }
        #endregion

        #region Input Enable/Disable
        private void EnableInput()
        {
            if (playerMap == null) return;
            playerMap.Enable();
            playerMap["Move"].performed += ctx => OnMove?.Invoke(ctx.ReadValue<Vector2>());
            playerMap["Move"].canceled += ctx => OnMove?.Invoke(Vector2.zero);
            playerMap["Look"].performed += ctx =>
            {
                var value = ctx.ReadValue<Vector2>();
                var device = ctx.control.device;
                // Only allow Mouse or Gamepad for Look
                if (device is UnityEngine.InputSystem.Mouse || device is UnityEngine.InputSystem.Gamepad)
                {
                    OnLook?.Invoke(value);
                }
                // No log for ignored devices; not critical
            };
            playerMap["Zoom"].performed += ctx => OnZoom?.Invoke(ctx.ReadValue<float>());
            playerMap["Jump"].performed += ctx => OnJump?.Invoke();
            playerMap["Sprint"].performed += ctx => OnSprint?.Invoke();
            playerMap["Interact"].performed += ctx => OnInteract?.Invoke();
            playerMap["Fire"].performed += ctx => OnFire?.Invoke();
            playerMap["Aim"].performed += ctx => OnAim?.Invoke();
            playerMap["Reload"].performed += ctx => OnReload?.Invoke();
            playerMap["Crouch"].performed += ctx => OnCrouch?.Invoke();
            playerMap["Inventory"].performed += ctx => OnInventory?.Invoke();
            playerMap["HolsterWeapon"].performed += ctx => OnHolsterWeapon?.Invoke();
        }

        private void DisableInput()
        {
            if (playerMap == null) return;
            playerMap.Disable();
        }
        #endregion
    }
} 