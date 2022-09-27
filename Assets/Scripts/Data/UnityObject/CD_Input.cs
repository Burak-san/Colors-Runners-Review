using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data.ValueObject;


namespace Data.UnityObject
{
    [CreateAssetMenu (fileName = "CD_Input", menuName = "ColorsRunners/CD_Input", order =0)]
    public class CD_Input : ScriptableObject
    {
        public InputData InputData;
    }
}

