using System;
using Controllers.TurretArea;
using Signals;
using UnityEngine;

namespace Managers
{
    public class TurretManager : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private TurretMovementController turretMovementController;

        #endregion

        #endregion

        #region Event Subscriptions

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            TurretAreaSignals.Instance.onTurretDetect += OnTurretDetect;
            TurretAreaSignals.Instance.onExitTurretArea += OnExitTurretArea;
        }

        private void UnSubscribeEvents()
        {
            TurretAreaSignals.Instance.onTurretDetect -= OnTurretDetect;
            TurretAreaSignals.Instance.onExitTurretArea -= OnExitTurretArea;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        #endregion

        private void OnTurretDetect(GameObject detectedCollectable)
        {
            turretMovementController.DetectCollectable(detectedCollectable);
        }

        private void OnExitTurretArea()
        {
            turretMovementController.StopTurret();
        }
    }
}