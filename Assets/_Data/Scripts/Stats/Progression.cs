using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Progression", menuName = "Anubias-Land/Progression", order = 20)]
public class Progression : ScriptableObject
{
    [SerializeField] ProgressionCharacterClass[] characterClasses = null;

    Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookupTable = null;

    private void BuildLookup()
    {
        if (lookupTable != null) return;
        lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();
        foreach (ProgressionCharacterClass progressionClass in characterClasses)
        {
            var statLookupTable = new Dictionary<Stat, float[]>();

            foreach (ProgressionStat progressionStat in progressionClass.stats)
            {
                statLookupTable[progressionStat.stat] = progressionStat.levels;
            }

            lookupTable[progressionClass.characterClass] = statLookupTable;
        }
    }

    public int GetLevels(Stat stat, CharacterClass characterClass)
    {
        BuildLookup();

        if (!lookupTable[characterClass].ContainsKey(stat))
        {
            return 0;
        }

        float[] levels = lookupTable[characterClass][stat];
        return levels.Length;
    }

    public float GetStat(Stat stat, CharacterClass characterClass, int level)
    {
        BuildLookup();

        float[] levels = lookupTable[characterClass][stat];

        if (levels.Length == 0) return 0;
        if (levels.Length < level) return levels[levels.Length - 1];
        return levels[level - 1];
    }



    [Serializable]
    class ProgressionCharacterClass
    {
        public CharacterClass characterClass;
        public ProgressionStat[] stats;
    }

    [Serializable]
    class ProgressionStat
    {
        public Stat stat;
        public float[] levels;
    }
}