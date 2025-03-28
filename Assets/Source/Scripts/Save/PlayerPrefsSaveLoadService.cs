using System.Collections.Generic;
using UnityEngine;

namespace Source.Scripts.Save
{
    public class PlayerPrefsSaveLoadService : ISaveLoadService
    {
        public PlayerProgress PlayerProgress { get; set; }
        
        public override void SaveProgress()
        {
            foreach (var saveProgress in SaveProgressList)
            {
                saveProgress.Save(PlayerProgress);
            }

            var save = JsonUtility.ToJson(PlayerProgress);
            PlayerPrefs.SetString(PlayerConstants.PlayerProgressSave, save);
            PlayerPrefs.Save();
        }

        public override PlayerProgress LoadProgress()
        {
            var load = PlayerPrefs.GetString(PlayerConstants.PlayerProgressSave);
            PlayerProgress = JsonUtility.FromJson<PlayerProgress>(load) ?? new PlayerProgress
            {
                TowerData = new TowerData
                {
                    CubeData = new List<CubeData>()
                }
            };

            foreach (var loadProgress in LoadProgresses)
            {
                loadProgress.Load(PlayerProgress);
            }

            return PlayerProgress;
        }
    }
}