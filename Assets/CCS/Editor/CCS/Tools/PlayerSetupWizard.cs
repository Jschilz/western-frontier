using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;
using CCS.Player;
using System.IO;

namespace CCS.Editor.Tools
{
    /// <summary>
    /// Wizard to create a Player GameObject with CharacterController, PlayerController,
    /// yellow body, black glasses, and black gun, using the correct render pipeline materials.
    /// 
    /// Author: James Schilz
    /// Date: 2024-06-09
    /// </summary>
    public class PlayerSetupWizard : EditorWindow
    {
        [MenuItem("CCS/Tools/Player Setup")]
        public static void ShowWindow()
        {
            GetWindow<PlayerSetupWizard>("Player Setup Wizard");
        }

        private void OnGUI()
        {
            GUILayout.Label("Player Setup Wizard", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("Creates a Player with a yellow capsule, black glasses, and a black gun, using the correct render pipeline.", MessageType.Info);
            if (GUILayout.Button("Create Player"))
            {
                CreatePlayer();
            }
        }

        private void CreatePlayer()
        {
            // Remove existing Player
            var existing = GameObject.FindGameObjectWithTag("Player");
            if (existing) DestroyImmediate(existing);

            // Create Player root
            var player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            player.name = "Player";
            player.tag = "Player";
            player.transform.position = Vector3.zero;

            // Add CharacterController and PlayerController
            var cc = player.GetComponent<CharacterController>() ?? player.AddComponent<CharacterController>();
            player.AddComponent<PlayerController>();

            // Assign yellow material (pipeline aware)
            var yellowMat = GetOrCreateMaterial("PlayerYellow", new Color(1f, 0.85f, 0f), false);
            player.GetComponent<Renderer>().sharedMaterial = yellowMat;

            // Add "Glasses" child (black cube)
            var glasses = GameObject.CreatePrimitive(PrimitiveType.Cube);
            glasses.name = "Glasses";
            glasses.transform.SetParent(player.transform);
            glasses.transform.localScale = new Vector3(0.6f, 0.1f, 0.1f);
            glasses.transform.localPosition = new Vector3(0f, 0.55f, 0.5f);
            glasses.GetComponent<Renderer>().sharedMaterial = GetOrCreateMaterial("PlayerBlack", Color.black, true);

            // Add "Gun" child (black cube)
            var gun = GameObject.CreatePrimitive(PrimitiveType.Cube);
            gun.name = "Gun";
            gun.transform.SetParent(player.transform);
            gun.transform.localScale = new Vector3(0.1f, 0.1f, 0.7f);
            gun.transform.localPosition = new Vector3(0.531f, 0.349f, 0.4f);
            gun.GetComponent<Renderer>().sharedMaterial = GetOrCreateMaterial("PlayerBlack", Color.black, true);

            // Add CameraFollowTarget child
            var cameraFollowTarget = new GameObject("CameraFollowTarget").transform;
            cameraFollowTarget.SetParent(player.transform);
            cameraFollowTarget.localPosition = new Vector3(0, 1.5f, 0);
            cameraFollowTarget.localRotation = Quaternion.identity;

            // Assign CameraFollowTarget to PlayerController
            var playerController = player.GetComponent<PlayerController>();
            var so = new SerializedObject(playerController);
            var prop = so.FindProperty("cameraFollowTarget");
            prop.objectReferenceValue = cameraFollowTarget;
            so.ApplyModifiedProperties();

            // Select the player
            Selection.activeGameObject = player;
            Debug.Log("[PlayerSetupWizard] Player created and selected.");
        }

        private Material GetOrCreateMaterial(string name, Color color, bool isBlack)
        {
            string matPath = $"Assets/CCS/Materials/{name}.mat";
            if (!Directory.Exists("Assets/CCS/Materials"))
                Directory.CreateDirectory("Assets/CCS/Materials");
            var mat = AssetDatabase.LoadAssetAtPath<Material>(matPath);
            if (mat) return mat;

            Shader shader = Shader.Find("Standard");
            var rp = GraphicsSettings.currentRenderPipeline;
            if (rp != null)
            {
                if (rp.GetType().Name.Contains("UniversalRenderPipeline"))
                    shader = Shader.Find("Universal Render Pipeline/Lit");
                else if (rp.GetType().Name.Contains("HighDefinitionRenderPipeline"))
                    shader = Shader.Find("HDRP/Lit");
            }
            mat = new Material(shader);
            mat.color = color;
            AssetDatabase.CreateAsset(mat, matPath);
            AssetDatabase.SaveAssets();
            return mat;
        }
    }
} 