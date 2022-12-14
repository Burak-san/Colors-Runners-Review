using UnityEngine;
using System.Collections.Generic;
using Data.ValueObject;

namespace Data.UnityObject
{
    [CreateAssetMenu(fileName = "CD_Level", menuName = "ColorsRunners/CD_Level",order = 0)]
    public class CD_Level : ScriptableObject
    {
        public List<LevelData> Levels;
    }
}