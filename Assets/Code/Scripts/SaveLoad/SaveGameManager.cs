using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Rendering;
using UnityEngine;

namespace SaveLoad {
    public static class SaveGameManager {
        
        public static SaveData currentSaveData = new SaveData();

        public const string saveDirectory = "/Saves/";                                          // Folder name
        public const string fileName = "save.sav";                                              // Save file name

        public static bool Save() {                                                             // Save the game
            
            var dir = Application.persistentDataPath + saveDirectory;                           // Directory where the save is
            
            if (!Directory.Exists(dir)) {                                                       // If the directory doesn't already exist create the save directory
                Directory.CreateDirectory(dir);
            }

            string json = JsonUtility.ToJson(currentSaveData, true);                            // JSON file | bool prettyPrint = true so that JSON file isn't all on one line
            File.WriteAllText(dir + fileName, json);                                            // Write all contents of variable "json" to the file in the directory

            //GUIUtility.systemCopyBuffer = dir;                                                // Copy file path to clipboard (Debugging)


            return true;                                                                        // Confirm save was sucessful
        }

        public static void Load() {                                                             // Load the game
            
            string fullPath = Application.persistentDataPath + saveDirectory + fileName;        // Loading the save file
            SaveData tempData = new SaveData();

            if (File.Exists(fullPath)) {
                string json = File.ReadAllText(fullPath);                                       // Reading all contents of the save file and storing into variable json
                tempData = JsonUtility.FromJson<SaveData>(json);                                // Using JsonUtility to convert string into the class SaveData
            } else {                                                                            // If the save file isn't found
                Debug.LogError("Save file does not exist!");
                Debug.LogError("Creating new save file");
                var dir = Application.persistentDataPath + saveDirectory;                       // Directory where the save is
                Directory.CreateDirectory(dir);                                                 // Create a save file
                Save();                                                                         // And add the variables to the save file
            }

            currentSaveData = tempData;
        }

    }
}
