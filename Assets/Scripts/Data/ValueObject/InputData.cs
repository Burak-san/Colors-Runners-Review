using UnityEngine;
using System;

namespace Data.ValueObject
{
    [Serializable]
    public class InputData
    {
        public float HorizontalInputSpeed = 2f;
        public Vector2 ClampSides = new Vector2(-5, 5);
        public float ClampSpeed = 0.007f;
    }
}

