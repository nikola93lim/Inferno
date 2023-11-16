using RPG.Attributes;
using RPG.Combat;
using RPG.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [System.Serializable]
        class ProgressionData
        {
            public CharacterClass characterClass;
            public ProgressionStat[] stats;
        }

        [System.Serializable]
        class ProgressionStat
        {
            public Stat stat;
            public float[] levels;
        }

        [SerializeField] private ProgressionData[] _progressionData;
        private Dictionary<CharacterClass, Dictionary<Stat, float[]>> _lookupTable = null;

        public float GetStat(Stat stat, CharacterClass characterClass, int level)
        {
            BuildLookup();

            float[] levels = _lookupTable[characterClass][stat];

            if (levels.Length < level)
            {
                return 0;
            }

            return levels[level - 1];
        }

        public int GetLevel(CharacterClass characterClass, Stat stat)
        {
            BuildLookup();

            float[] levels = _lookupTable[characterClass][stat];
            return levels.Length;
        }

        private void BuildLookup()
        {
            if (_lookupTable != null) return;

            _lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();

            foreach (ProgressionData data in _progressionData)
            {
                var statLookupTable = new Dictionary<Stat, float[]>();

                foreach (ProgressionStat progressionStat in data.stats)
                {
                    statLookupTable[progressionStat.stat] = progressionStat.levels;
                }

                _lookupTable[data.characterClass] = statLookupTable;
            }
        }
    }
}
