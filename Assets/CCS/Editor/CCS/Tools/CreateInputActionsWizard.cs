using UnityEditor;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM && UNITY_EDITOR
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.InputSystem.Editor;
using UnityEngine.InputSystem.Layouts;
#endif
using System.IO;

namespace CCS.Editor.Tools
{
    /// <summary>
    /// Editor wizard to create a pre-configured Input Actions asset for Surviving The West.
    /// Adds a menu item under CCS/Tools/CreateInputActionsAsset.
    ///
    /// Author: James Schilz
    /// Date Created: 4/27/2025
    /// Property of: Crazy Carrot Studios
    /// </summary>
    public static class CreateInputActionsWizard
    {
        private const string assetPath = "Assets/CCS/Data/GameInputActions.inputactions";

        [MenuItem("CCS/Tools/CreateInputActionsAsset")]
        public static void CreateInputActionsAsset()
        {
#if ENABLE_INPUT_SYSTEM && UNITY_EDITOR
            // Ensure Data folder exists
            if (!AssetDatabase.IsValidFolder("Assets/CCS/Data"))
            {
                if (!AssetDatabase.IsValidFolder("Assets/CCS"))
                    AssetDatabase.CreateFolder("Assets", "CCS");
                AssetDatabase.CreateFolder("Assets/CCS", "Data");
            }

            var asset = ScriptableObject.CreateInstance<InputActionAsset>();

            // Player Map
            var playerMap = new InputActionMap("Player");
            // Move: WASD (composite) + Gamepad left stick
            var move = playerMap.AddAction("Move", InputActionType.Value);
            move.AddCompositeBinding("2DVector")
                .With("Up", "<Keyboard>/w")
                .With("Down", "<Keyboard>/s")
                .With("Left", "<Keyboard>/a")
                .With("Right", "<Keyboard>/d");
            move.AddBinding("<Gamepad>/leftStick");
            // Look: Mouse delta + Gamepad right stick
            var look = playerMap.AddAction("Look", InputActionType.Value);
            look.AddBinding("<Mouse>/delta");
            look.AddBinding("<Gamepad>/rightStick");
            // Zoom: Mouse scroll wheel + Gamepad right stick Y
            var zoom = playerMap.AddAction("Zoom", InputActionType.Value);
            zoom.AddBinding("<Mouse>/scroll/y");
            zoom.AddBinding("<Gamepad>/rightStick/y");
            // Jump
            var jump = playerMap.AddAction("Jump", InputActionType.Button);
            jump.AddBinding("<Keyboard>/space");
            jump.AddBinding("<Gamepad>/buttonSouth");
            // Sprint
            var sprint = playerMap.AddAction("Sprint", InputActionType.Button);
            sprint.AddBinding("<Keyboard>/leftShift");
            sprint.AddBinding("<Gamepad>/leftStickPress");
            // Interact
            var interact = playerMap.AddAction("Interact", InputActionType.Button);
            interact.AddBinding("<Keyboard>/e");
            interact.AddBinding("<Gamepad>/buttonWest");
            // Fire
            var fire = playerMap.AddAction("Fire", InputActionType.Button);
            fire.AddBinding("<Mouse>/leftButton");
            fire.AddBinding("<Gamepad>/rightTrigger");
            // Aim
            var aim = playerMap.AddAction("Aim", InputActionType.Button);
            aim.AddBinding("<Mouse>/rightButton");
            aim.AddBinding("<Gamepad>/leftTrigger");
            // Reload
            var reload = playerMap.AddAction("Reload", InputActionType.Button);
            reload.AddBinding("<Keyboard>/r");
            reload.AddBinding("<Gamepad>/buttonNorth");
            // Crouch
            var crouch = playerMap.AddAction("Crouch", InputActionType.Button);
            crouch.AddBinding("<Keyboard>/c");
            crouch.AddBinding("<Gamepad>/buttonEast");
            // Inventory
            var inventory = playerMap.AddAction("Inventory", InputActionType.Button);
            inventory.AddBinding("<Keyboard>/tab");
            inventory.AddBinding("<Gamepad>/start");
            // Holster Weapon
            var holster = playerMap.AddAction("HolsterWeapon", InputActionType.Button);
            holster.AddBinding("<Keyboard>/h");
            holster.AddBinding("<Gamepad>/dpad/down");
            asset.AddActionMap(playerMap);

            // UI Map
            var uiMap = new InputActionMap("UI");
            // Navigate: Arrow keys (composite) + Gamepad dpad
            var navigate = uiMap.AddAction("Navigate", InputActionType.Value);
            navigate.AddCompositeBinding("2DVector")
                .With("Up", "<Keyboard>/upArrow")
                .With("Down", "<Keyboard>/downArrow")
                .With("Left", "<Keyboard>/leftArrow")
                .With("Right", "<Keyboard>/rightArrow");
            navigate.AddBinding("<Gamepad>/dpad");
            // Submit
            var submit = uiMap.AddAction("Submit", InputActionType.Button);
            submit.AddBinding("<Keyboard>/enter");
            submit.AddBinding("<Gamepad>/buttonSouth");
            // Cancel
            var cancel = uiMap.AddAction("Cancel", InputActionType.Button);
            cancel.AddBinding("<Keyboard>/escape");
            cancel.AddBinding("<Gamepad>/buttonEast");
            // Point
            var point = uiMap.AddAction("Point", InputActionType.Value);
            point.AddBinding("<Mouse>/position");
            // Click
            var click = uiMap.AddAction("Click", InputActionType.Button);
            click.AddBinding("<Mouse>/leftButton");
            click.AddBinding("<Gamepad>/buttonSouth");
            // Pause
            var pause = uiMap.AddAction("Pause", InputActionType.Button);
            pause.AddBinding("<Keyboard>/escape");
            pause.AddBinding("<Gamepad>/start");
            asset.AddActionMap(uiMap);

            // Horse Map
            var horseMap = new InputActionMap("Horse");
            // Move: WASD (composite) + Gamepad left stick
            var horseMove = horseMap.AddAction("Move", InputActionType.Value);
            horseMove.AddCompositeBinding("2DVector")
                .With("Up", "<Keyboard>/w")
                .With("Down", "<Keyboard>/s")
                .With("Left", "<Keyboard>/a")
                .With("Right", "<Keyboard>/d");
            horseMove.AddBinding("<Gamepad>/leftStick");
            // Mount/Dismount
            var mount = horseMap.AddAction("MountDismount", InputActionType.Button);
            mount.AddBinding("<Keyboard>/f");
            mount.AddBinding("<Gamepad>/buttonNorth");
            // Sprint
            var horseSprint = horseMap.AddAction("Sprint", InputActionType.Button);
            horseSprint.AddBinding("<Keyboard>/leftShift");
            horseSprint.AddBinding("<Gamepad>/leftStickPress");
            // Jump
            var horseJump = horseMap.AddAction("Jump", InputActionType.Button);
            horseJump.AddBinding("<Keyboard>/space");
            horseJump.AddBinding("<Gamepad>/buttonSouth");
            // Whistle
            var whistle = horseMap.AddAction("Whistle", InputActionType.Button);
            whistle.AddBinding("<Keyboard>/q");
            whistle.AddBinding("<Gamepad>/dpad/up");
            // Brake
            var brake = horseMap.AddAction("Brake", InputActionType.Button);
            brake.AddBinding("<Keyboard>/s");
            brake.AddBinding("<Gamepad>/dpad/down");
            // Interact
            var horseInteract = horseMap.AddAction("Interact", InputActionType.Button);
            horseInteract.AddBinding("<Keyboard>/e");
            horseInteract.AddBinding("<Gamepad>/buttonWest");
            // Inventory
            var horseInventory = horseMap.AddAction("Inventory", InputActionType.Button);
            horseInventory.AddBinding("<Keyboard>/tab");
            horseInventory.AddBinding("<Gamepad>/start");
            asset.AddActionMap(horseMap);

            // Wagon Map
            var wagonMap = new InputActionMap("Wagon");
            // Steer: A/D (composite) + Gamepad left stick
            var steer = wagonMap.AddAction("Steer", InputActionType.Value);
            steer.AddCompositeBinding("2DVector")
                .With("Left", "<Keyboard>/a")
                .With("Right", "<Keyboard>/d");
            steer.AddBinding("<Gamepad>/leftStick");
            // Mount/Dismount
            var wagonMount = wagonMap.AddAction("MountDismount", InputActionType.Button);
            wagonMount.AddBinding("<Keyboard>/f");
            wagonMount.AddBinding("<Gamepad>/buttonNorth");
            // Accelerate
            var accelerate = wagonMap.AddAction("Accelerate", InputActionType.Button);
            accelerate.AddBinding("<Keyboard>/w");
            accelerate.AddBinding("<Gamepad>/rightTrigger");
            // Brake/Reverse
            var brakeReverse = wagonMap.AddAction("BrakeReverse", InputActionType.Button);
            brakeReverse.AddBinding("<Keyboard>/s");
            brakeReverse.AddBinding("<Gamepad>/leftTrigger");
            // Whip
            var whip = wagonMap.AddAction("Whip", InputActionType.Button);
            whip.AddBinding("<Keyboard>/space");
            whip.AddBinding("<Gamepad>/buttonSouth");
            // Horn/Bell
            var horn = wagonMap.AddAction("HornBell", InputActionType.Button);
            horn.AddBinding("<Keyboard>/q");
            horn.AddBinding("<Gamepad>/dpad/up");
            // Interact
            var wagonInteract = wagonMap.AddAction("Interact", InputActionType.Button);
            wagonInteract.AddBinding("<Keyboard>/e");
            wagonInteract.AddBinding("<Gamepad>/buttonWest");
            asset.AddActionMap(wagonMap);

            // Save asset
            var json = asset.ToJson();
            File.WriteAllText(assetPath, json);
            AssetDatabase.ImportAsset(assetPath);
            EditorUtility.DisplayDialog("Input Actions Asset Created", $"Input Actions asset created at {assetPath}", "OK");
#else
            EditorUtility.DisplayDialog("Input System Not Enabled", "Please enable the Input System package in Project Settings.", "OK");
#endif
        }
    }
} 