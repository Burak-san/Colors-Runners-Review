using System;
using Enums;
using Managers;
using UnityEngine;
using Signals;

namespace Controllers.IdleBuilding
{
    public class SideBuildingScorePhysicsController : MonoBehaviour
    {
        #region Selft Variables

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

                    if (manager.BuildingsData.SideObject.MarketPrice > manager.BuildingsData.SideObject.PayedAmount)
                    {
                        manager.UpdateSidePayedAmount();
                        ParticleSignals.Instance.onParticleBurst?.Invoke(transform.position);
                    }
                    else
                    {
                        if (manager.BuildingsData.SideObject.IdleLevelStateType == IdleLevelStateType.Uncompleted)
                        {
                            ParticleSignals.Instance.onParticleStop.Invoke();
                            manager.UpdateSideBuildingStatus(IdleLevelStateType.Completed);
                            manager.CheckSideBuildingScoreStatus(IdleLevelStateType.Completed);
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