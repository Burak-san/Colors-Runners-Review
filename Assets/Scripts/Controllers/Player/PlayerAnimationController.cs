using UnityEngine;
using Enums;

namespace Controllers.Player
{
    public class PlayerAnimationController : MonoBehaviour
    {

        #region Self Variables

        #region Public Variables
        #endregion

        #region Serialized Variables
        [SerializeField] private Animator animator;
        #endregion

        #region Private Variables
        #endregion
        #endregion

        public void ChangeCollectableAnimation(PlayerAnimationStates playerAnimationStates)
        {
            switch (playerAnimationStates)
            {
                case PlayerAnimationStates.Idle:
                    animator.Play(playerAnimationStates.ToString());
                    break;
                case PlayerAnimationStates.Running:
                    animator.Play(playerAnimationStates.ToString());
                    break;
            }
        }

    }
}