using System.Collections;
using UnityEngine;
using System;

namespace Data.ValueObject
{
    [Serializable]
    public class StackData
    {
        public int StackMemberAmount = 5;
        public GameObject InitializedStack;
        public Vector3 LerpSpeed = new Vector3(0.2f, 0.2f, 1f);
        public float ScaleFactor = 2f;
        public float ReverseScaleFactor = 1.4f;
        public float StackDistanceZ = 1f;
        public float ScaleUpDelay = 0.2f;

    }
}