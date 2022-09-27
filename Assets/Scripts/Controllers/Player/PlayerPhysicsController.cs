using UnityEngine;
using Managers;
using Enums;
using Signals;
using Controllers.Portal;
using Controllers.Drone;


namespace Controllers.Player
{
    public class PlayerPhysicsController : MonoBehaviour
    {
        #region Serialized Variables

        [SerializeField] private PlayerManager playerManager;
        [SerializeField] private GameObject closeRunnerPhysics;
        #endregion

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Roulette"))
            {
                other.gameObject.SetActive(false);
                closeRunnerPhysics.gameObject.SetActive(true);
                playerManager.ChangeState(GameStates.Roullette);
                playerManager.ActivateAllMovement(false);
            }
            else if (other.CompareTag("Portal"))
            {
                playerManager.SendGateColorData(other.GetComponent<PortalMeshController>().Color);
            }
            else if (other.CompareTag("ColorArea"))
            {
                StackSignals.Instance.onTurretGroundControl?.Invoke(other.gameObject);
            }
            else if (other.CompareTag("AfterGround"))
            {
                playerManager.EnableFinalCollider(other.gameObject);
            }
            
            else if (other.CompareTag("DroneColorArea"))
            {
                if (playerManager.PlayerColorType == other.GetComponent<DroneAreaColorController>().ColorType)
                {
                    other.GetComponent<DroneAreaColorController>().DroneAreaMatchType = CollectableMatchType.Match;
                }
            }
            else if (other.CompareTag("Collectable"))
            {
                if(playerManager.CurrentGameState == GameStates.Idle)
                {
                    var Collectable = other.transform.parent;
                    Collectable.gameObject.SetActive(false);
                    ScoreSignals.Instance.onIncreaseScore?.Invoke();
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("DroneArea"))
            {
                playerManager.StopVerticalMovement();
            }
        }
    }
}

