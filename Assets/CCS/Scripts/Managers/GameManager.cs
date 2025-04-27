using UnityEngine;
using CCS.Utilities;
using System;

namespace CCS.Managers
{
    /// <summary>
    /// Central game manager for Surviving The West. Handles global game state, initialization, and high-level events.
    ///
    /// Author: James Schilz
    /// Date Created: 4/27/2025
    /// Property of: Crazy Carrot Studios
    /// </summary>
    public class GameManager : MonoSingleton<GameManager>
    {
        #region Inspector Variables
        [Header("Game Settings")]
        [Tooltip("Initial player lives.")]
        [SerializeField] private int initialPlayerLives = 3;

        [Tooltip("Enable debug mode for extra logging.")]
        [SerializeField] private bool debugMode = false;
        #endregion

        #region Events
        /// <summary>
        /// Invoked when the game state changes.
        /// </summary>
        public event Action<GameState> OnGameStateChanged;
        #endregion

        #region Properties
        public int PlayerLives { get; private set; }
        public GameState CurrentState { get; private set; } = GameState.Initializing;
        #endregion

        #region Unity Methods
        protected override void Awake()
        {
            base.Awake();
            Initialization();
        }
        #endregion

        #region Initialization
        public override void Initialization()
        {
            PlayerLives = initialPlayerLives;
            SetGameState(GameState.MainMenu);
            if (debugMode)
                Debug.Log("GameManager initialized.");
        }
        #endregion

        #region Game State Management
        public void SetGameState(GameState newState)
        {
            if (CurrentState == newState) return;
            CurrentState = newState;
            OnGameStateChanged?.Invoke(newState);
            if (debugMode)
                Debug.Log($"Game state changed to: {newState}");
        }
        #endregion
    }

    /// <summary>
    /// Enum for high-level game states.
    /// </summary>
    public enum GameState
    {
        Initializing,
        MainMenu,
        Playing,
        Paused,
        GameOver
    }
} 