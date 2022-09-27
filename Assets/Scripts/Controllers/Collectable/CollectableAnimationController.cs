using Enums;
using UnityEngine;

namespace Controllers.Collectable
{
    public class CollectableAnimationController : MonoBehaviour
    {

        #region Serialized Variables

        [SerializeField] private Animator animator;

        #endregion

        public void ChangeCollectableAnimation(CollectableAnimationStates collectableAnimationStates)
        {
            animator.SetTrigger(collectableAnimationStates.ToString());

            if (collectableAnimationStates ==CollectableAnimationStates.Dead)
            {
                animator.Play(collectableAnimationStates.ToString());
            }
        }
    }
}