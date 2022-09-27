using System.Collections.Generic;
using Data.ValueObject;
using UnityEngine;

namespace Data.UnityObject
{
    [CreateAssetMenu(fileName = "CD_IdleLevel", menuName = "ColorsRunners/CD_IdleLevel", order = 0)]
    public class CD_IdleLevel : ScriptableObject
    {
        public List<IdleLevelData> IdleLevelList;
    }
}