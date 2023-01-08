using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaveLoad {

    [System.Serializable]                   // Serializes class
    public class SaveData {

        public PlayerData playerData;       // Gets data from player to save
        public OptionsData optionsData;     // Gets data from options to save
        public WorldData worldData;         // Gets data from the world to save
    }
}
