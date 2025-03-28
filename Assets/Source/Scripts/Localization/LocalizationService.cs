using System.Collections.Generic;
using UnityEngine;

namespace Source.Scripts.Localization
{
    public class LocalizationService
    {
        private SystemLanguage _currentLanguage = SystemLanguage.Unknown;
        
        private readonly Dictionary<string, string> _localizedStrings;
        
        public LocalizationService()
        {
            _currentLanguage = SystemLanguage.English;

            _localizedStrings = new Dictionary<string, string>()
            {
                { "set_cube", "Cube Set" },
                { "explosion_cube", "Cube Explosion" },
                { "rolled_cube", "The cube is rolled" },
                { "out_range", "out range" },
            };
        }
        
        public string LocalizationByTag(string tag)
        {
            return _localizedStrings[tag];
        }
    }
}