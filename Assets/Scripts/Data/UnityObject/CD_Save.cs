using UnityEngine;
using Data.ValueObject;

namespace Data.UnityObject
{
    [CreateAssetMenu(fileName = "CD_Save", menuName = "ColorsRunners/CD_Save", order = 0)]
    public class CD_Save : ScriptableObject
    {
        public SaveData SaveData;
    }
}