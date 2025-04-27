using UnityEditor;
using UnityEngine;
using System.IO;

/// <summary>
/// Editor utility to create the full folder structure for the Surviving The West project.
/// Adds a menu item under CCS/Tools/ProjectFolders.
/// </summary>
public static class ProjectFoldersWizard
{
    private static readonly string[] folders = new string[]
    {
        "Art/Characters",
        "Art/Environments",
        "Art/Props",
        "Art/UI",
        "Art/Animals",
        "Art/Weapons",
        "Art/VFX",
        "Audio/Music",
        "Audio/SFX",
        "Audio/Voice",
        "Data/ScriptableObjects",
        "Data/Databases",
        "Data/Localization",
        "Documentation",
        "Editor/CCS",
        "Materials",
        "Networking/Netcode",
        "Networking/MultiplayerTools",
        "Networking/Server",
        "Physics",
        "Plugins",
        "Prefabs/Characters",
        "Prefabs/Animals",
        "Prefabs/Buildings",
        "Prefabs/Items",
        "Prefabs/Vehicles",
        "Prefabs/UI",
        "Resources",
        "Scenes/Main",
        "Scenes/Towns",
        "Scenes/Wilderness",
        "Scenes/Test",
        "Scripts/AI",
        "Scripts/Audio",
        "Scripts/Camera",
        "Scripts/Combat",
        "Scripts/Core",
        "Scripts/Crafting",
        "Scripts/Economy",
        "Scripts/Environment",
        "Scripts/Input",
        "Scripts/Inventory",
        "Scripts/NPC",
        "Scripts/Networking",
        "Scripts/Player",
        "Scripts/Progression",
        "Scripts/UI",
        "Scripts/Utilities",
        "Settings",
        "Shaders",
        "StreamingAssets",
        "Tests/PlayMode",
        "Tests/EditMode"
    };

    [MenuItem("CCS/Tools/ProjectFolders")]
    public static void CreateProjectFolders()
    {
        string root = "Assets/CCS";
        if (!AssetDatabase.IsValidFolder(root))
        {
            AssetDatabase.CreateFolder("Assets", "CCS");
            Debug.Log($"Created folder: {root}");
        }

        foreach (var folder in folders)
        {
            var fullPath = Path.Combine(root, folder).Replace("\\", "/");
            if (!AssetDatabase.IsValidFolder(fullPath))
            {
                CreateFolderRecursively(fullPath);
                Debug.Log($"Created folder: {fullPath}");
            }
        }
        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("Project Folders Created", "All required CCS project folders have been created!", "OK");
    }

    /// <summary>
    /// Recursively creates folders if they do not exist.
    /// </summary>
    private static void CreateFolderRecursively(string fullPath)
    {
        string[] parts = fullPath.Split('/');
        string current = parts[0];
        for (int i = 1; i < parts.Length; i++)
        {
            string next = Path.Combine(current, parts[i]);
            if (!AssetDatabase.IsValidFolder(next))
            {
                AssetDatabase.CreateFolder(current, parts[i]);
            }
            current = next;
        }
    }
} 