using System.Collections.Generic;
using UnityEngine;

namespace Source.Scripts.Save
{
    public abstract class ISaveLoadService
    {
        public PlayerProgress PlayerProgress { get; set; }

        protected List<ISaveProgress> SaveProgressList { get; set; } = new List<ISaveProgress>();
        protected List<ILoadProgress> LoadProgresses { get; set; } = new List<ILoadProgress>();
        
        public abstract void SaveProgress();
        public abstract PlayerProgress LoadProgress();

        public void RegistryProgressSavers(GameObject gameObject)
        {
            foreach (var saveProgress in gameObject.GetComponentsInChildren<ISaveProgress>())
            {
                SaveProgressList.Add(saveProgress);

                if (saveProgress is ILoadProgress loadProgress)
                {
                    LoadProgresses.Add(loadProgress);
                }
            }
        }
    }
}