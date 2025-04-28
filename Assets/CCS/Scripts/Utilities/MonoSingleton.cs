using UnityEngine;

namespace CCS.Utilities
{
    /// <summary>
    /// Generic MonoBehaviour singleton base class for persistent managers and systems.
    /// Ensures only one instance exists and persists across scenes.
    ///
    /// Author: James Schilz
    /// Date Created: 4/27/2025
    /// Property of: Crazy Carrot Studios
    /// </summary>
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static T _instance;
        private static bool applicationIsQuitting = false;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                    if (_instance == null && !applicationIsQuitting)
                    {
                        Debug.LogWarning($"The singleton of type {typeof(T)} is NULL.");
                    }
                }
                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                DontDestroyOnLoad(gameObject); // Persist across scenes
            }
            else if (_instance != this)
            {
                Destroy(gameObject); // Destroy duplicate
            }
        }

        /// <summary>
        /// Optional override for initialization logic in derived classes.
        /// </summary>
        public virtual void Initialization() { }

        protected virtual void OnApplicationQuit()
        {
            applicationIsQuitting = true;
        }
    }
} 