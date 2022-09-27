using UnityEngine;
using Enums;
using Keys;
using Managers;
using Signals;
using Controllers.Drone;

namespace Controllers.Collectable
{
    public class CollectablePhysicsController : MonoBehaviour
    {
        #region Serialized Variables

        [SerializeField] private CollectableManager manager;
        #endregion

        private void OnTriggerEnter(Collider other)
        {
            if(CompareTag("Collected") && other.CompareTag("Collectable"))
            {
                CollectableManager otherCollectableManager = other.transform.parent.GetComponent<CollectableManager>();

                if(manager.ColorType == otherCollectableManager.ColorType)
                {
                    StackSignals.Instance.onIncreaseStack?.Invoke(other.transform.parent.gameObject);
                    otherCollectableManager.SetAnim(CollectableAnimationStates.Running);
                    other.tag = "Collected";
                }
                else
                {
                    other.transform.parent.gameObject.SetActive(false);
                    StackSignals.Instance.onDecreaseStack?.Invoke(new ObstacleCollisionGOParams()
                    {
                        Collected = gameObject,
                    });
                    
                } 
            }

            if (other.CompareTag("Obstacle"))
            {
                other.gameObject.SetActive(false);
                StackSignals.Instance.onDecreaseStack?.Invoke(new ObstacleCollisionGOParams()
                {
                    Collected = gameObject,
                    Obstacle = other.gameObject
                });
                StackSignals.Instance.onCheckStack?.Invoke();
            }

            if (other.CompareTag("TurretColorArea"))
            {
                manager.SetAnim(CollectableAnimationStates.CrouchWalking);
            }
            if (other.CompareTag("DroneArea"))
            {
                manager.DelistFromStack();
            }
            if (other.CompareTag("DroneColorArea") && CompareTag("Collected"))
            {
                manager.SetCollectablePositionOnDroneArea(other.gameObject.transform);
                var droneAreaColorController = other.GetComponent<DroneAreaColorController>();

                if (manager.ColorType == droneAreaColorController.ColorType)
                {
                    manager.MatchType = CollectableMatchType.Match;
                }
                else
                {
                    manager.MatchType = CollectableMatchType.UnMatch;
                }
                tag = "Collectable";
            }

            if (other.CompareTag("AfterGround"))
            {
                if (manager.MatchType != CollectableMatchType.Match)
                {
                    manager.SetAnim(CollectableAnimationStates.Dead);
                    manager.SetActiveFalse();
                    ParticleSignals.Instance.onPlayerDeath.Invoke(transform.position);
                }
                else
                {
                    tag = "Collected";
                    manager.IncreaseStackAfterDroneArea();
                    StackSignals.Instance.onCheckStack?.Invoke();
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("TurretColorArea"))
            {
                manager.SetAnim(CollectableAnimationStates.Running);
                manager.ExitTurretArea();
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("TurretColorArea"))
            {
                manager.EnterTurretArea(other.gameObject);
            }
        }
    }
}

