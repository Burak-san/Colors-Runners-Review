using System;
using Enums;
using Managers;
using Signals;
using UnityEngine;

namespace Controllers.IdleBuilding
{
    public class BuildingScorePhysicsController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private BuildingManager manager;

        #endregion

        #region Private Variables

        private float _timer = 0f;

        #endregion

        #endregion

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("ScorePhysics"))
            {
                _timer -= Time.fixedDeltaTime;
                if (_timer <= 0)
                {
                    _timer = 0.2f;
                    if (manager.BuildingsData.BuildingMarketPrice > manager.BuildingsData.PayedAmount)
                    {
                        manager.UpdatePayedAmount();
                        ParticleSignals.Instance.onParticleBurst?.Invoke(transform.position);
                    }
                    else
                    {
                        if (manager.BuildingsData.idleLevelState == IdleLevelStateType.Uncompleted)
                        {
                            ParticleSignals.Instance.onParticleStop.Invoke();
                            manager.OpenSideObject();
                            manager.UpdateBuildingStatus(IdleLevelStateType.Completed);
                            manager.CheckBuildingScoreStatus(IdleLevelStateType.Completed);
                        }
                    }
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("ScorePhysics"))
            {
                if (manager.BuildingsData.BuildingMarketPrice == manager.BuildingsData.PayedAmount)
                {
                    manager.OpenSideObject();
                    manager.UpdateBuildingStatus(IdleLevelStateType.Completed);
                    manager.CheckBuildingScoreStatus(IdleLevelStateType.Completed);
                }

                _timer = 0f;
            }
            
            if (other.CompareTag("Player"))
            {
                ParticleSignals.Instance.onParticleStop.Invoke();
            }
        }
    }
}