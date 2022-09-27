using System.Collections;
using UnityEngine;
using Extentions;
using UnityEngine.Events;
using Keys;
using Enums;
namespace Signals
{
    public class StackSignals : MonoSingleton<StackSignals>
    {
        public UnityAction<GameObject> onIncreaseStack = delegate { };
        public UnityAction<ObstacleCollisionGOParams> onDecreaseStack = delegate { };
        public UnityAction<ColorType> onColorChange = delegate { };
        public UnityAction<GameObject> onTurretGroundControl = delegate { };
        public UnityAction<int> onEnterDroneArea = delegate { };
        public UnityAction<GameObject> onRebuildStack = delegate { };
        public UnityAction onCheckStack = delegate { };
    }
}