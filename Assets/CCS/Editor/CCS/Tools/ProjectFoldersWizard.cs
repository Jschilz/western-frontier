using UnityEditor;
using UnityEngine;
using System.IO;

namespace CCS.Editor.Tools
{
    /// <summary>
    /// Editor utility to reset and create a simplified folder structure for the Surviving The West project.
    /// Adds a menu item under CCS/Tools/ProjectFolders.
    ///
    /// Author: James Schilz
    /// Date Created: 4/27/2025
    /// Property of: Crazy Carrot Studios
    /// </summary>
    public static class ProjectFoldersWizard
    {
        private static readonly string[] folders = new string[]
        {
            "Art",
            "Audio",
            "Data",
            "Documentation",
            "Editor/CCS/Tools", // Wizards live here
            "Materials",
            "Networking",
            "Physics",
            "Plugins",
            "Prefabs",
            "Resources",
            "Scenes",
            "Scripts/Managers",
            "Scripts/Player",
            "Scripts/Inventory",
            "Scripts/Crafting",
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
            // 1. Delete all folders under Assets/CCS except Editor/CCS/Tools
            if (Directory.Exists(root))
            {
                foreach (var dir in Directory.GetDirectories(root))
                {
                    // Skip Editor/CCS/Tools to preserve wizards
                    string dirNorm = dir.Replace("\\", "/");
                    if (!dirNorm.EndsWith("Editor/CCS/Tools") && !dirNorm.EndsWith("Editor") && !dirNorm.EndsWith("Editor/CCS"))
                    {
                        FileUtil.DeleteFileOrDirectory(dir);
                        Debug.Log($"Deleted folder: {dir}");
                    }
                }
            }
            AssetDatabase.Refresh();

            // 2. Create new folder structure
            if (!AssetDatabase.IsValidFolder(root))
            {
                AssetDatabase.CreateFolder("Assets", "CCS");
            }
            foreach (var folder in folders)
            {
                var fullPath = Path.Combine(root, folder).Replace("\\", "/");
                CreateFolderRecursively(fullPath);
            }
            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog("Project Folders Created", "Simplified CCS project folders have been created!", "OK");
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
                string next = Path.Combine(current, parts[i]).Replace("\\", "/");
                if (!AssetDatabase.IsValidFolder(next))
                {
                    AssetDatabase.CreateFolder(current, parts[i]);
                }
                current = next;
            }
        }
    }
} 